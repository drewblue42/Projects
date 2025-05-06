from fastapi.responses import JSONResponse
from sqlmodel import Session, select

from backend.database.schema import *
from backend.exceptions import EntityNotFound


def get_account_by_username(session: Session, username: str) -> DBAccount:
    stmt = select(DBAccount).where(DBAccount.username == username)
    return session.exec(stmt).first()

def get_account_by_email(session: Session, email: str) -> DBAccount:
    stmt = select(DBAccount).where(DBAccount.email == email)
    return session.exec(stmt).first()

def get_by_id(session: Session, account_id: int):
    stmt = select(DBAccount).where(DBAccount.id == account_id)
    result = session.exec(stmt).first()
    if not result:
        return EntityNotFound("account", f"id={account_id}")
    return result


