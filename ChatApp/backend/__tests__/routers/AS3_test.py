from backend.database.schema import DBChat, DBMessage, DBAccount, DBChatMembership
from datetime import datetime, timezone

#Author: Andrew Winward
#These tests are for the AS3 requirments


#Tests if post"/chats" creates a new chat
def test_create_chat(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    session.add(a1)
    session.commit()

    chat = {"name": "groupChat", "owner_id": 1}
    
    response = client.post("/chats", json=chat)
    
    assert response.status_code == 201
    
    data = response.json()
    
    assert isinstance(data, dict)
    assert set(data.keys()) == {"id", "name", "owner_id"}
    
    assert data["name"] == "groupChat"
    assert data["owner_id"] == 1
    assert isinstance(data["id"], int)

#Tests if post"/chats" throws an error if an owner ID does not exist
def test_create_chat_error(session, client):

    chat = {"name": "groupChat", "owner_id": 4}
    
    response = client.post("/chats", json=chat)
    
    assert response.status_code == 404

#Tests if put(f"/chats/{chat.id}" updates a chat correclty
def test_update_chat(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()

    update = {"name": "groupChat", "owner_id": a2.id}
    
    response = client.put(f"/chats/{chat.id}", json=update)
    
    assert response.status_code == 200
    
    data = response.json()
    
    assert isinstance(update, dict)
    assert set(data.keys()) == {"id", "name", "owner_id"}
    
    assert data["name"] == "groupChat"
    assert data["owner_id"] == a2.id
    assert isinstance(data["id"], int)

#Tests if put(f"/chats/{chat.id}" throws an error if an owner ID does not exist
def test_update_chat_error(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()

    update = {"name": "groupChat", "owner_id": a2.id}
    response = client.put(f"/chats/3", json=update)
    assert response.status_code == 404

#Tests if .delete(f"/chats/{chat.id}") deletes a chat correclty
def test_chat_delete(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    session.add(a1)
    session.commit()

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)
    
    response = client.delete(f"/chats/{chat.id}")
    assert response.status_code == 204

#Tests if delete(f"/chats/{chat.id}" throws an error if an owner ID does not exist
def test_chat_delete_error(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    session.add(a1)
    session.commit()

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)
    
    response = client.delete(f"/chats/3")
    assert response.status_code == 404

# tests id post(f"/chats/{chat.id}/messages" creates a new message
def test_message_creation(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()

    message = {"text": "Hello", "account_id": a2.id}

    response = client.post(f"/chats/{chat.id}/messages", json=message)

    assert response.status_code == 201
    
    data = response.json()
    
    assert isinstance(data, dict)
    assert set(data.keys()) == {"id" ,
        "text",
        "chat_id", 
        "account_id", 
        "created_at"}
    
    assert data["text"] == "Hello"
    assert data["account_id"] == a2.id
    assert isinstance(data["id"], int)

#Tests if post(f"/chats/{chat_id}/messages" throws an error if an owner ID does not exist
def test_message_creation_error(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()

    message = {"text": "Hello", "account_id": a2.id}

    response = client.post(f"/chats/4/messages", json=message)

    assert response.status_code == 404

# tests if put(f"/chats/{chat.id}/messages" updates a message correctly
def test_message_update(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()

    message =DBMessage(
            text="YO YO HOMIE", 
            account_id=a2.id, chat_id=chat.id)

    session.add(message)
    session.commit()


    messageUpdate = {"text": "Hello"}

    response = client.put(f"/chats/{chat.id}/messages/{message.id}", json=messageUpdate)

    assert response.status_code == 200
    
    data = response.json()
    
    assert isinstance(data, dict)
    assert set(data.keys()) == {"id" ,
        "text",
        "chat_id", 
        "account_id", 
        "created_at"}
    
    assert data["text"] == "Hello"
    assert isinstance(data["id"], int)

#Tests if put(f"/chats/{chat_id}/messages/{message.id}" throws an error if an owner ID does not exist
def test_message_update_error(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()

    message =DBMessage(
            text="YO YO HOMIE", 
            account_id=a2.id, chat_id=chat.id)

    session.add(message)
    session.commit()


    messageUpdate = {"text": "Hello"}

    response = client.put(f"/chats/4/messages/{message.id}", json=messageUpdate)

    assert response.status_code == 404

# tests if put(f"/chats/{chat.id}/messages" deletes a message correctly
def test_message_delete(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()

    message =DBMessage(
            text="YO YO HOMIE", 
            account_id=a2.id, chat_id=chat.id)

    session.add(message)
    session.commit()


    response = client.delete(f"/chats/{chat.id}/messages/{message.id}")

    assert response.status_code == 204

#Tests if .delete(f"/chats/{chat_id}/messages/{message.id}" throws an error if an owner ID does not exist
def test_message_delete_error(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )

    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()

    message =DBMessage(
            text="YO YO HOMIE", 
            account_id=a2.id, chat_id=chat.id)

    session.add(message)
    session.commit()


    response = client.delete(f"/chats/4/messages/{message.id}")

    assert response.status_code == 404

# tests if post(f"/chats/{chat.id}/accounts" creates a membership correctly
def test_create_newMembership(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )
    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = {"account_id": a2.id}
    
    response = client.post(f"/chats/{chat.id}/accounts", json=membership)
    
    assert response.status_code == 201
    
    data = response.json()
    
    assert isinstance(data, dict)
    assert set(data.keys()) == {"chat_id", "account_id"}
    
    assert data["chat_id"] == chat.id
    assert data["account_id"] == a2.id

#Tests if post(f"/chats/{chat_id}/accounts" throws an error if an owner ID does not exist
def test_create_newMembership_error(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )
    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = {"account_id": a2.id}
    
    response = client.post(f"/chats/4/accounts", json=membership)
    
    assert response.status_code == 404

#checks if .delete(f"/chats/{chat.id}/accounts/{a2.id}" deletes a membership
def test_delete_Membership(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )
    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()
    
    response = client.delete(f"/chats/{chat.id}/accounts/{a2.id}")
    assert response.status_code == 204


#Tests if .delete(f"/chats/{chat_id}/accounts/{chat_id}" throws an error if an owner ID does not exist
def test_delete_Membership_error(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBAccount(
        username="drewblue",
        email="drewble@youknwowho.com",
        hashed_password="adsd"
    )
    session.add(a1)
    session.add(a2)
    session.commit()
    session.refresh(a1)
    session.refresh(a2)

    chat = DBChat(
        name = "betterChat",
        owner_id= a1.id
    )
    session.add(chat)
    session.commit()
    session.refresh(chat)

    membership = DBChatMembership(chat_id=chat.id, account_id=a2.id)
    session.add(membership)
    session.commit()
    
    response = client.delete(f"/chats/4/accounts/{a2.id}")
    assert response.status_code == 404
    