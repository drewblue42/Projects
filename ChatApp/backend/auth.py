from fastapi import APIRouter, Response, Form, Cookie, Header
from jose import ExpiredSignatureError


from backend.database.account import get_by_id
from backend.database.schema import *
from fastapi.responses import JSONResponse
from backend import auth
from sqlmodel import select, Session
from backend.exceptions import ExpiredAccessToken, InvalidAccessToken, InvalidCredentials
from backend.models import auth
from jose import jwt
import datetime
from datetime import timezone, datetime
import bcrypt
import os


JWT_SECRET_KEY = os.environ.get("JWT_SECRET_KEY", default="jwt-dev-key")
JWT_ALGORITHM = "HS256"
JWT_ISSUER = "http://127.0.0.1"
DURATION = 3600
JWT_OPTIONS = {"require_sub": True, "require_iss": True, "require_exp": True}

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

# Validates the password and throws an error if credentials are wrong.
def validate_credentials(user: DBAccount | None, password: str) -> DBAccount:
    if user is None or not verify_password(password, user.hashed_password):
        raise InvalidCredentials()
    return user

# Generates a set of JWT claims for the specified user.
def generate_claims(user: DBAccount) -> auth.Claims:
    iat = int(datetime.now(timezone.utc).timestamp())
    exp = iat + DURATION
    return {
        "sub": str(user.id),
        "iss": JWT_ISSUER,
        "iat": iat,
        "exp": exp,
    }

# Generates a JWT token for a user.
def generate_token(session: Session, username: str, password: str) -> str:
    stmt = select(DBAccount).where(DBAccount.username == username)
    user = session.exec(stmt).first()
    user = validate_credentials(user, password)
    claims = generate_claims(user)
    return jwt.encode(claims, JWT_SECRET_KEY, algorithm=JWT_ALGORITHM)

def create_user(session: Session, form: auth.Registration) -> DBAccount:
    hashed_password = hash_password(form.password)
    user = DBAccount(**form.model_dump(), hashed_password=hashed_password)
    session.add(user)
    session.commit()
    session.refresh(user)
    return {
        "id": user.id,
        "username": user.username,
        "email": user.email,
    }

def extract_account(session: Session, token: str) -> DBAccount:
    print(token)
    try:
        payload = jwt.decode(
            token,
            JWT_SECRET_KEY,
            algorithms=[JWT_ALGORITHM],
            options=JWT_OPTIONS
        )
        print(payload)
        claims = auth.Claims(**payload)
        return get_by_id(session, int(claims.sub))
    
    except ExpiredSignatureError:
        raise ExpiredAccessToken()
    except Exception:
        raise InvalidAccessToken()