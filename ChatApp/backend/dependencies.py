"""Dependencies for the backend API.

Args:
    engine (sqlachemy.engine.Engine): The database engine
"""
from typing import Annotated
from fastapi import Depends
from backend import auth
from fastapi.security import APIKeyCookie, HTTPAuthorizationCredentials, HTTPBearer
from sqlmodel import SQLModel, create_engine, Session

from backend.database.schema import *
from backend.exceptions import ExpiredAccessToken, InvalidAccessToken, NotAuthenticated

_db_filename = "backend/database/development.db"
_db_url = f"sqlite:///{_db_filename}"
connect_args = {"check_same_thread": False}
engine = create_engine(_db_url, echo=True, connect_args=connect_args)
JWT_COOKIE_KEY = "pony_express_token"

cookie_scheme = APIKeyCookie(name=JWT_COOKIE_KEY, auto_error=False)
bearer_scheme = HTTPBearer(auto_error=False)

def create_db_tables():
    SQLModel.metadata.create_all(engine)


def get_session():
    with Session(engine) as session:
        yield session



def get_access_token(
    cookie_token: str | None = Depends(cookie_scheme),
    bearer_token: HTTPAuthorizationCredentials | None = Depends(bearer_scheme),
    ) -> str:
    if cookie_token is not None:
        return cookie_token
    elif bearer_token is not None:
        return bearer_token.credentials
    else:
        raise NotAuthenticated()
    

def get_current_account(
    session: Session = Depends(get_session),
    token: str = Depends(get_access_token),
) -> DBAccount:  
        return auth.extract_account(session, token)
  
    
    
CurrentUser = Annotated[DBAccount, Depends(get_current_account)]
DBSession = Annotated[Session, Depends(get_session)]
