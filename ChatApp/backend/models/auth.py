from typing import Annotated
from fastapi import Depends, FastAPI, Form, Request
from pydantic import BaseModel
from backend.database.schema import *

app = FastAPI()

class Registration(BaseModel):
    username: str
    email: str
    password: str

class Login(BaseModel):
    username: str
    password: str

class AccessToken(BaseModel):
    access_token: str
    token_type: str

class Claims(BaseModel):
    sub: str
    iss: str
    iat: int
    exp: int

