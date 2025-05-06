from fastapi import APIRouter,  Response, Depends, Form, Cookie, Header


from backend.auth import hash_password, verify_password
from backend.database.schema import *
from fastapi.security import OAuth2PasswordRequestForm
from fastapi.responses import JSONResponse
from backend.dependencies import *
from backend.exceptions import EntityNotFound
from backend.models import auth
from sqlmodel import select, Session

#Author: Andrew Winward
#This file handles the account routes for the Pony Express API

router = APIRouter()

#Returns a JSON object with two keys: metadata(number of accounts), accounts(id, username)
@router.get("/accounts", response_model = dict)
def get_accounts(session: DBSession)-> dict:
    stmt= select(DBAccount).order_by(DBAccount.id)
    results = session.exec(stmt).all()

    account_results = [{"id": account.id, "username": account.username} for account in results]

    return{
        "metadata": {"count": len(account_results)},
        "accounts": account_results
    }


#Gets the authenticated user's account from either the Header or the Cookie
@router.get("/accounts/me", status_code=200)
def get_account(current_user: CurrentUser):
    return {
        "id": current_user.id,
        "username": current_user.username,
        "email": current_user.email
    }

#updates an authenticated users account
@router.put("/accounts/me", status_code=200)
def update_account(
    account_update: UpdateAccount,
    current_user: CurrentUser,  
    session: DBSession
):
    
    if account_update.username is not None and account_update.username != current_user.username:
        stmt = select(DBAccount).where(
            DBAccount.username == account_update.username,
            DBAccount.id != current_user.id
        )
        existingUsername = session.exec(stmt).first()
        if existingUsername:
            return JSONResponse(
                status_code=422,
                content={
                    "error": "duplicate_entity_value",
                    "message": f"Duplicate value: account with username={account_update.username} already exists"
                }
            )
        current_user.username = account_update.username

    if account_update.email is not None and account_update.email != current_user.email:
        stmt = select(DBAccount).where(
            DBAccount.email == account_update.email,
            DBAccount.id != current_user.id
        )
        existingEmail = session.exec(stmt).first()
        if existingEmail:
            return JSONResponse(
                status_code=422,
                content={
                    "error": "duplicate_entity_value",
                    "message": f"Duplicate value: account with email={account_update.email} already exists"
                }
            )
        current_user.email = account_update.email

    session.add(current_user)
    session.commit()
    session.refresh(current_user)
    return {"id": current_user.id, "username": current_user.username, "email": current_user.email}

#Changes an authenticated user's password to a new one after verifying the old one
@router.put("/accounts/me/password", status_code=204)
def update_password(
    session: DBSession,
    current_user: CurrentUser ,
    old_password: str = Form(...),
    new_password: str = Form(...), 
):
    if not verify_password(old_password, current_user.hashed_password):
        return JSONResponse(
            status_code=401,
            content={
                "error": "invalid_credentials",
                "message": "Authentication failed: invalid username or password"
            }
        )
    
    current_user.hashed_password = hash_password(new_password)
    session.add(current_user)
    session.commit()
    session.refresh(current_user)

    return Response(status_code=204)

#Delete's the authenticated user's account
@router.delete("/accounts/me", status_code=204)
def delete_account(
    session: DBSession,
    current_user: CurrentUser  
):
    stmt = select(DBChat).where(DBChat.owner_id == current_user.id)
    chatOwner = session.exec(stmt).first()
    if chatOwner:
        return JSONResponse(
            status_code=422,
            content={
                "error": "chat_owner_removal",
                "message": "Unable to remove the owner of a chat"
            }
        )
    
    session.delete(current_user)
    session.commit()
    
    return Response(status_code=204)



#Takes account_id as an integer path parameter
#Returns a JSON object for the account with the corresponding id
@router.get("/accounts/{account_id}", response_model = dict)
def get_account(session: DBSession, account_id: int)-> dict:

    stmt= select(DBAccount).where(DBAccount.id==account_id)
    result = session.exec(stmt).first()

    if not result:
        raise EntityNotFound("account", f"{account_id}")

    accountObj = {"id": result.id, "username": result.username}

    return accountObj
