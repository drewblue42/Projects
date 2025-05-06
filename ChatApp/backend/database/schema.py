"""Database table models."""

from datetime import datetime

from sqlmodel import Field, Relationship, SQLModel


class DBAccount(SQLModel, table=True):
    __tablename__ = "accounts"  # type: ignore
    __table_args__ = {"extend_existing": True} 

    # fields
    id: int | None = Field(default=None, primary_key=True)
    username: str = Field(unique=True, index=True)
    email: str = Field(unique=True)
    hashed_password: str

    # relationships
    owned_chats: list["DBChat"] = Relationship(back_populates="owner")
    messages: list["DBMessage"] = Relationship(back_populates="account")
    memberships: list["DBChatMembership"] = Relationship(
        back_populates="account",
        cascade_delete=True,
    )


class DBChat(SQLModel, table=True):
    __tablename__ = "chats"  # type: ignore
    __table_args__ = {"extend_existing": True} 

    # fields
    id: int | None = Field(default=None, primary_key=True)
    name: str
    owner_id: int = Field(foreign_key="accounts.id", ondelete="RESTRICT")

    # relationships
    owner: DBAccount = Relationship(back_populates="owned_chats")
    messages: list["DBMessage"] = Relationship(
        back_populates="chat",
        cascade_delete=True,
    )
    memberships: list["DBChatMembership"] = Relationship(
        back_populates="chat",
        cascade_delete=True,
    )


class DBMessage(SQLModel, table=True):
    __tablename__ = "messages"  # type: ignore
    __table_args__ = {"extend_existing": True} 

    # fields
    id: int | None = Field(default=None, primary_key=True)
    text: str
    account_id: int | None = Field(
        default=None,
        foreign_key="accounts.id",
        ondelete="SET NULL",
    )
    chat_id: int = Field(
        foreign_key="chats.id",
        ondelete="CASCADE",
    )
    created_at: datetime | None = Field(default_factory=datetime.now)

    # relationships
    account: DBAccount = Relationship(back_populates="messages")
    chat: DBChat = Relationship(back_populates="messages")


class DBChatMembership(SQLModel, table=True):
    __tablename__ = "chat_memberships"  # type: ignore
    __table_args__ = {"extend_existing": True} 
    # fields
    account_id: int = Field(
        foreign_key="accounts.id",
        primary_key=True,
        ondelete="CASCADE",
    )
    chat_id: int = Field(
        foreign_key="chats.id",
        primary_key=True,
        ondelete="CASCADE",
    )

    # relationships
    account: DBAccount = Relationship(back_populates="memberships")
    chat: DBChat = Relationship(back_populates="memberships")

#Helps create a chat with the correct body
class CreateChat(SQLModel):
    name: str
    owner_id: int

#Helps update a chat with the correct body
# name and owner ID is optional
class updateChat(SQLModel):
    name: str | None = None
    owner_id: int | None = None

#Helps create a message with the correct body
# account_id is optional
class createMessage(SQLModel):
    text: str
    account_id: int | None = None

#Helps create a meber with the correct body
class createAccountMembership(SQLModel):
    account_id: int

class UpdateAccount(SQLModel):
    username: str | None = None
    email: str | None = None

class UpdatePassword(SQLModel):
    old_password: str | None = None
    new_password: str | None = None

