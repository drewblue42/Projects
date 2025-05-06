import os
from fastapi import APIRouter, Response, Depends, Form, Cookie, Header
from typing import Annotated

import bcrypt
from fastapi.security import OAuth2PasswordRequestForm
from backend.database import account as DB_account
from backend.database.schema import *
from fastapi.responses import JSONResponse
from backend.dependencies import CurrentUser, DBSession
from backend.exceptions import DuplicateEntityError, EntityNotFound
from backend import exceptions, auth as backend_Auth
from backend.models import auth as models_auth
from sqlmodel import select, Session
import jwt

#Author: Andrew Winward
#This is the routers for the chat potion of the pony express

router = APIRouter()
JWT_SECRET_KEY = os.environ.get("JWT_SECRET_KEY", default="jwt-dev-key")
JWT_ALGORITHM = "HS256"
JWT_ISSUER = "http://127.0.0.1"
DURATION = 3600


################Routes################################################################

#Creates a new account while storing a hashed password
#Returns a JSON object with id, username, and email.
@router.post("/auth/registration", status_code=201)
def new_account(session:DBSession, form: Annotated[models_auth.Registration, Form()]):

    existingUsername = DB_account.get_account_by_username(session, form.username )
    if existingUsername:
        raise DuplicateEntityError("username", form.username)

    
    existingEmail = DB_account.get_account_by_email(session, form.email)
    if existingEmail:
        raise DuplicateEntityError("email", form.email)


    return backend_Auth.create_user(session, form)

#Creates an access token (JWT) for the requesting account
#Successful response:
#Status code is 200 OK
#Body is the JSON access token object with two keys: access_token, token_type.


@router.post("/auth/token", status_code=200)
def login(session: DBSession, form: Annotated[models_auth.Login, Form()],)-> models_auth.AccessToken:
    stmt = select(DBAccount).where(DBAccount.username == form.username)
    user = session.exec(stmt).first()

    if user is None or not backend_Auth.verify_password(form.password, user.hashed_password):
        return JSONResponse(
            status_code=401,
            content={
                "error": "invalid_credentials",
                "message": "Authentication failed: invalid username or password"
            }
        )

    access_token = backend_Auth.generate_token(session, form.username, form.password)

    return {
        "access_token": access_token,
        "token_type": "bearer"
    }

#Creates an access token (JWT) for the requesting account is stored in cookie
#Successful response:
#Status code is 204 No Content
#The response contains the access token in a cookie
@router.post("/auth/web/login", status_code=204)
def login(session: DBSession, formData: OAuth2PasswordRequestForm = Depends()):
    stmt = select(DBAccount).where(DBAccount.username == formData.username)
    user = session.exec(stmt).first()

    if user is None or not backend_Auth.verify_password(formData.password, user.hashed_password):
        return JSONResponse(
            status_code=401,
            content={
                "error": "invalid_credentials",
                "message": "Authentication failed: invalid username or password"
            }
        )

    claims = backend_Auth.generate_claims(user)
    token = jwt.encode(claims, backend_Auth.JWT_SECRET_KEY, algorithm = backend_Auth.JWT_ALGORITHM )

    response = Response(status_code=204)
    response.set_cookie(
        key="pony_express_token",
        value=token,
        httponly=True
    )


    return response

#A logged-in account is logged out
#Successful response
#Status code is 204 No Content
#The access token cookie is deleted from the response
@router.post("/auth/web/logout", status_code=204)
def logout(session: DBSession, authorization: str = Header(None), pony_express_token: str = Cookie(None)):
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
    
    response = Response(status_code=204)
    response.delete_cookie(key="pony_express_token")
    return response