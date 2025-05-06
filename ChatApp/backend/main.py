"""PonyExpress backend API application.

Args:
    app (FastAPI): The FastAPI application
"""
#Author: Andrew Winward

from fastapi.middleware.cors import CORSMiddleware
from contextlib import asynccontextmanager

from fastapi import FastAPI
from sqlmodel import select
from fastapi.responses import JSONResponse

from backend.dependencies import create_db_tables
from backend.dependencies import DBSession
from backend.exceptions import EntityNotFound
from backend.database.schema import *
from backend.models.exceptions import ExceptionModel
from backend.routers import account, chat, authentication

@asynccontextmanager
async def lifespan(app: FastAPI):
    create_db_tables()
    yield


app = FastAPI(
    title="Pony Express API",
    summary="Pony Express is an instant messaging API.",
    lifespan=lifespan,
)

app.include_router(account.router)
app.include_router(chat.router)
app.include_router(authentication.router)

@app.exception_handler(ExceptionModel)
async def entity_not_found_exception_handler(request, exc: ExceptionModel):
    return exc.response()


#Checks if the API is running
@app.get("/status", response_model=None, status_code=204)
def status():
    pass

app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],
    allow_methods=["*"],
    allow_headers=["*"],
    allow_credentials=True,
)
