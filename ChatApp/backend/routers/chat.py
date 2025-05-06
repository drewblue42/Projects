from fastapi import APIRouter, Response, Form, Cookie, Header


from backend.database.schema import *
from fastapi.responses import JSONResponse
from backend.dependencies import CurrentUser, DBSession
from backend.models import auth
from backend.exceptions import EntityNotFound
from sqlmodel import select, Session
import jwt
import datetime
from datetime import timezone, datetime
import bcrypt
import os

#Author: Andrew Winward
#This is the routers for the chat potion of the pony express

router = APIRouter()
JWT_SECRET_KEY = os.environ.get("JWT_SECRET_KEY", default="jwt-dev-key")
JWT_ALGORITHM = "HS256"
JWT_ISSUER = "http://127.0.0.1"
DURATION = 3600

def hash_password(password: str) -> str:
    return bcrypt.hashpw(
    password.encode("utf-8"),
    bcrypt.gensalt(),
    ).decode("utf-8")

def verify_password(password: str, hashed_password: str) -> bool:
    return bcrypt.checkpw(
    password.encode("utf-8"),
    hashed_password.encode("utf-8"),
    )

def validate_credentials(user: DBAccount | None, password: str) -> DBAccount:
    if user is None or not verify_password(password, user.hashed_password):
        return JSONResponse(
            status_code=401,
            content={
                "error": "invalid_credentials",
                "message": "Authentication failed: invalid username or password"
            }
        )
    return user

def generate_claims(user: DBAccount) -> auth.Claims:
    iat = int(datetime.now(timezone.utc).timestamp())
    exp = iat + DURATION
    return {
    "sub": str(user.id),
    "iss": JWT_ISSUER,
    "iat": iat,
    "exp": exp,
    }

def generate_token(session: Session, username: str, password: str) -> str:
    stmt = select(DBAccount).where(DBAccount.username == username)
    user = session.exec(stmt).first()


    claims = generate_claims(user)
    return jwt.encode(claims, JWT_SECRET_KEY, algorithm=JWT_ALGORITHM,
    )



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

#Takes account_id as an integer path parameter
#Returns a JSON object for the account with the corresponding id
@router.get("/accounts/{account_id}", response_model = dict)
def get_account(session: DBSession, account_id: int)-> dict:


    stmt= select(DBAccount).where(DBAccount.id==account_id)
    result = session.exec(stmt).first()

    if not result:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find account with id={account_id}"
            }
        )

    accountObj = {"id": result.id, "username": result.username}

    return accountObj

#Returns a JSON object with two keys: metadata, chats
# metadata(number of chats)
# chats(id, name, owner_id)   
@router.get("/chats", response_model = dict)
def get_chats(session: DBSession)-> dict:

    stmt= select(DBChat).order_by(DBChat.id)
    results = session.exec(stmt).all()

    chat_results = [{"id": chat.id, "name": chat.name, "owner_id": chat.owner_id} for chat in results]

    return{
        "metadata": {"count": len(chat_results)},
        "chats": chat_results
    }

#Takes chat_id as an integer path parameter
#Returns a JSON object for the chat with the corresponding id
@router.get("/chats/{chat_id}", response_model = dict)
def get_chat(session: DBSession, chat_id: int)-> dict:

    stmt= select(DBChat).where(DBChat.id==chat_id)
    result = session.exec(stmt).first()

    if not result:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )

    chatObj = {"id": result.id, "name": result.name, "owner_id": result.owner_id}

    return chatObj

#Takes chat_id as an integer path parameter
#Returns a JSON object with two keys: metadata, messages
# metadata(number of messages)
# messages(id, text, account_id, chat_id, created_at)
@router.get("/chats/{chat_id}/messages", response_model = dict)
def get_messages(session: DBSession, chat_id: int)-> dict:

    stmt= select(DBMessage).where(DBMessage.chat_id==chat_id).order_by(DBMessage.id)
    results = session.exec(stmt).all()

    if not results:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )

    message_results = [{"id": message.id, "text": message.text, "account_id": message.account_id, "chat_id": message.chat_id, "created_at": message.created_at }for message in results]
    chat_total = [{"chat_id": message.chat_id} for message in results]
    return{
        "metadata": {"count": len(chat_total)},
        "messages": message_results
    }

#Takes chat_id as an integer path parameter
#Returns a JSON object with two keys: metadata, accounts
#metadata(number of accounts)
#accounts(account_id, chat_id)
@router.get("/chats/{chat_id}/accounts", response_model = dict)
def get_membership(session: DBSession, chat_id: int)-> dict:
    stmt= select(DBAccount).join(DBChatMembership, DBAccount.id == DBChatMembership.account_id).where(DBChatMembership.chat_id==chat_id).order_by(DBAccount.id)
    results = session.exec(stmt).all()

    if not results:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )

    membership_results = [{"id": account.id,"username": account.username }for account in results]

    return{
        "metadata": {"count": len(membership_results)},
        "accounts": membership_results
    }


####################################   ASSIGNMENT 4 

#Creates a chat that includes the chat's name and associated owner ID
#Returns a JSON with id, name, and owner's id
@router.post("/chats", status_code=201, response_model=DBChat)
def create_chat(session: DBSession, current_user: CurrentUser, chat: CreateChat, authorization: str = Header(None), pony_express_token: str = Cookie(None)) -> DBChat:

    
    user = session.exec(select(DBAccount).where(DBAccount.id == current_user.id)).first()
    if not user:
        return JSONResponse(
            status_code=403,
            content={
                "error": "invalid_access_token",
                "message": "Authentication failed: invalid access token"
            }
        )

    stmt= select(DBAccount).where(DBAccount.id==chat.owner_id)
    account = session.exec(stmt).first()

    if not account:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find account with id={chat.owner_id}"
            }
        )
    
    if chat.owner_id != user.id:
        return JSONResponse(
            status_code=403,
            content={
                "error": "access_denied",
                "message": "Cannot create chat on behalf of different account"
            }
        )

    stmt= select(DBChat).where(DBChat.name ==chat.name)
    dupChat = session.exec(stmt).first()
    if dupChat:
        return JSONResponse(
            status_code=422,
            content={
                "error": "duplicate_entity_value",
                "message": f"Duplicate value: chat with name={chat.name} already exists"
            }
        )

    newChat = DBChat(name=chat.name, owner_id=chat.owner_id)
    session.add(newChat)
    session.commit()
    session.refresh(newChat)

    memberShip = DBChatMembership(chat_id=newChat.id, account_id=newChat.owner_id)
    session.add(memberShip)
    session.commit()
                    
    return {
        "id": newChat.id,
        "name": newChat.name,
        "owner_id": newChat.owner_id
     }   

#Updates an exisiting chat
#can either include a chat's name and/or a associated owner ID.
#Returns a JSON with id, name, and owner's id
@router.put("/chats/{chat_id}", status_code=200, response_model=DBChat)
def update_chat(session: DBSession, update: updateChat, chat_id: int) -> DBChat:
    
    stmt= select(DBChat).where(DBChat.id==chat_id)
    result = session.exec(stmt).first()
    if result is None:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )
    
    if update.name is not None and update.name != result.name:
        stmt= select(DBChat).where(DBChat.name ==update.name, DBChat.id != chat_id)
        dup = session.exec(stmt).first()
        if dup:
         return JSONResponse(
            status_code=422,
            content={
                "error": "duplicate_entity_value",
                "message": f"Duplicate value: chat with name={update.name} already exists"
            }
        )
        result.name = update.name

    if update.owner_id:
        stmt = select(DBChatMembership).where(
            DBChatMembership.chat_id == chat_id,
            DBChatMembership.account_id == update.owner_id
        )
        membership = session.exec(stmt).first()
        if not membership:
            return JSONResponse(
                status_code=422,
                content={
                    "error": "chat_membership_required",
                    "message": f"Account with id={update.owner_id} must be a member of chat with id={chat_id}"
                }
            )
        result.owner_id = update.owner_id

    session.add(result)
    session.commit()
    session.refresh(result)
                    
    return {
        "id": chat_id,
        "name": result.name,
        "owner_id":result.owner_id
    }
    

#Deletes an exisiting chat
@router.delete("/chats/{chat_id}", status_code=204)
def delete_chat(session: DBSession, chat_id: int) -> None:
    stmt= select(DBChat).where(DBChat.id==chat_id)
    result = session.exec(stmt).first()
    if not result:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )
    session.delete(result)
    session.commit()

#Creates a new message in the database belonging to the specified chat
#Takes chat_id as an integer path parameter
#Request body has two required keys: text and account_id
#text: The text value is a string for the text of the new message
#account_id: The account_id value is an integer for the account of the new message
@router.post("/chats/{chat_id}/messages", status_code=201, response_model = DBMessage)
def create_message(session: DBSession, chat_id: int, createMsg: createMessage, authorization: str = Header(None), pony_express_token: str = Cookie(None)) -> DBMessage:
    
    token = None

    if authorization and authorization.startswith("Bearer "):
        token = authorization.split("Bearer ")[1]
    elif pony_express_token:
        token = pony_express_token

    if not token:
        return JSONResponse(
            status_code=403,
            content={
                "error": "authentication_required",
                "message": "Not authenticated"
            }
        )
    
    try:
        jwtDecoded = jwt.decode(token, JWT_SECRET_KEY, algorithms=[JWT_ALGORITHM])
    except jwt.ExpiredSignatureError:
        return JSONResponse(
            status_code=403,
            content={
                "error": "expired_access_token",
                "message": "Authentication failed: expired access token"
            }
        )
    except jwt.InvalidTokenError:
        return JSONResponse(
            status_code=403,
            content={
                "error": "invalid_access_token",
                "message": "Authentication failed: invalid access token"
            }
        )
    
    user = session.exec(select(DBAccount).where(DBAccount.id == int(jwtDecoded.get("sub")))).first()

    if not user:
        return JSONResponse(
            status_code=403,
            content={
                "error": "invalid_access_token",
                "message": "Authentication failed: invalid access token"
            }
        )
    
    if createMsg.account_id != user.id:
        return JSONResponse(
            status_code=403,
            content={
                "error": "access_denied",
                "message": "Cannot create message on behalf of different account"
            }
        )
    
    stmt= select(DBChat).where(DBChat.id==chat_id)
    newChat = session.exec(stmt).first()
    if not newChat:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )
    
    stmt = select(DBChatMembership).where(
    DBChatMembership.chat_id == chat_id,
    DBChatMembership.account_id == createMsg.account_id
    )
    membership = session.exec(stmt).first()
    if membership is None:
        return JSONResponse(
        status_code=422,
        content={
            "error": "chat_membership_required",
            "message": f"Account with id={createMsg.account_id} must be a member of chat with id={chat_id}"
        }
    )
    
    newMessage = DBMessage(text=createMsg.text, account_id=createMsg.account_id, chat_id=chat_id)
   

    session.add(newMessage)
    session.commit()
    session.refresh(newMessage)
                    
    return {
        "id": newMessage.id ,
        "text": newMessage.text ,
        "chat_id": newMessage.chat_id, 
        "account_id": newMessage.account_id, 
        "created_at": newMessage.created_at
    }

#The specified message has updated text
#Takes chat_id and message_id as integer path parameters
#Request body has one required key: text
#text: The text value is a string for the new text of the message
@router.put("/chats/{chat_id}/messages/{message_id}", status_code=200, response_model = DBMessage)
def updateMessage(session: DBSession, chat_id: int, createMsg: createMessage, message_id: int)->DBMessage:
    stmt= select(DBChat).where(DBChat.id==chat_id)
    chatID = session.exec(stmt).first()
    if chatID is None:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )
    
    stmt = select(DBMessage).where(DBMessage.id == message_id, DBMessage.chat_id == chat_id )
    message = session.exec(stmt).first()
    if message is None:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find message with id={message_id}"
            }
        )
    
    message.text = createMsg.text


    session.commit()
    session.refresh(message)

    return {
        "id": message.id ,
        "text": message.text ,
        "chat_id": message.chat_id, 
        "account_id": message.account_id, 
        "created_at": message.created_at
    }

#The specified message is deleted from the database
#Takes chat_id and message_id as integer path parameters
#Successful response
#Status code is 204 No Content
@router.delete("/chats/{chat_id}/messages/{message_id}", status_code=204)
def deleteMessage(session: DBSession, chat_id: int, message_id: int) -> None:
    stmt= select(DBChat).where(DBChat.id==chat_id)
    chatID = session.exec(stmt).first()
    if chatID is None:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )
    
    stmt = select(DBMessage).where(DBMessage.id == message_id, DBMessage.chat_id == chat_id )
    message = session.exec(stmt).first()
    if message is None:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find message with id={message_id}"
            }
        )
    
    session.delete(message)
    session.commit()


#The specified account is a member of the specified chat
#If the account is already a member of the chat, no change to the database
#If the account is not a member of the chat, a new chat membership is added to the database
#Takes chat_id as an integer path parameter
#Request body has one required key: account_id
#account_id: The account_id value is an integer for the account that will be a member of the chat
@router.post("/chats/{chat_id}/accounts", status_code=201, response_model = DBChatMembership)
def addAccount(session: DBSession, chat_id: int, mberShip: createAccountMembership, response: Response)->DBChatMembership:
    stmt= select(DBChat).where(DBChat.id==chat_id)
    chatID = session.exec(stmt).first()
    if chatID is None:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )
    
    stmt= select(DBAccount).where(DBAccount.id==mberShip.account_id)
    result = session.exec(stmt).first()

    if not result:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find account with id={mberShip.account_id}"
            }
        )
    
    stmt = select(DBChatMembership).where(DBChatMembership.chat_id == chat_id, DBChatMembership.account_id == mberShip.account_id )
    memberShip = session.exec(stmt).first()

    if memberShip:
        response.status_code = 200
        return memberShip
    
    newMembership = DBChatMembership(chat_id=chat_id, account_id=mberShip.account_id) 
    session.add(newMembership)
    session.commit()
    session.refresh(newMembership)
    response.status_code = 201
    return newMembership

#A chat membership is deleted from the database
#The chat messages belonging to the corresponding account are updated to have NULL account_id
#Takes chat_id and account_id as integer path parameters
#Successful response
#Status code is 204 No Content
@router.delete("/chats/{chat_id}/accounts/{account_id}", status_code=204)
def deleteAccountMessages(session: DBSession, chat_id: int, account_id: int) -> None:
    stmt= select(DBChat).where(DBChat.id==chat_id)
    chatID = session.exec(stmt).first()
    if chatID is None:
        return JSONResponse(
            status_code=404,
            content={
                "error": "entity_not_found",
                "message": f"Unable to find chat with id={chat_id}"
            }
        )
    
    stmt= select(DBAccount).where(DBAccount.id==account_id)
    account = session.exec(stmt).first()
    if account is None:
        return JSONResponse(
            status_code=422,
            content={
                "error": "chat_membership_required",
                "message": f"Account with id={account_id} must be a member of chat with id={chat_id}"
            }
        )
    
    stmt = select(DBChatMembership).where(DBChatMembership.chat_id == chat_id, DBChatMembership.account_id == account_id )
    memberShip = session.exec(stmt).first()
    if memberShip is None:
        return JSONResponse(
            status_code=422,
            content={
                "error": "chat_membership_required",
                "message": f"Account with id={account_id} must be a member of chat with id={chat_id}"
            }
        )
    
    if account_id == chat_id:
        return JSONResponse(
            status_code=422,
            content={
                "error": "chat_owner_removal",
                "message": f"Unable to remove the owner of a chat"
            }
        )
    
    stmt = select(DBMessage). where(DBMessage.chat_id == chat_id, DBMessage.account_id == account_id)
    totalMessages = session.exec(stmt).all()
    
    for message in totalMessages:
        message.account_id = None
        session.add(message)


    session.delete(memberShip)

    session.commit()