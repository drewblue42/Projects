from backend.database.schema import DBChat, DBMessage, DBAccount, DBChatMembership
from datetime import datetime, timezone

#Author: Andrew Winward
#Performs test in line with the chat routes

#tests if the /chats perfroms correctly
def test_chats(session, client):
    a1 = DBChat(
        name = "Drew",
        owner_id="2"
    )
    a2 = DBChat(
        name = "Madi",
        owner_id="3"
    )

    session.add(a1)
    session.add(a2)
    session.commit()

    response = client.get("/chats")
    assert response.status_code == 200
    assert response.json() == {
            "metadata": {
                "count": 2
            },
            "chats": [
            {
                "id": 1,
                "name": "Drew",
                "owner_id": 2

            },
            {
                "id": 2,
                "name": "Madi",
                "owner_id": 3
            }
    ]
    }

#tests if the /chats/{chat_id} perfroms correctly
def test_chat(session, client):
    a1 = DBChat(
        name = "Drew",
        owner_id="2"
    )
    a2 = DBChat(
        name = "Madi",
        owner_id="3"
    )

    session.add(a1)
    session.add(a2)
    session.commit()

    response = client.get("/chats/1")
    assert response.status_code == 200
    assert response.json() == {
           "id": 1,
           "name": "Drew",
           "owner_id": 2
    }

#tests if the /chats/{chat_id} perfroms correctly 
# when given a chat_id that does not exist
def test_chat_error(session, client):
    a1 = DBChat(
        name = "Drew",
        owner_id="2"
    )
    a2 = DBChat(
        name = "Madi",
        owner_id="3"
    )

    session.add(a1)
    session.add(a2)
    session.commit()

    response = client.get("/chats/3")
    assert response.status_code == 404
    assert response.json() == {
          "error": "entity_not_found",
          "message": f"Unable to find chat with id=3"
    }
#tests if the /chats/{chat_id}/messages perfroms correctly 
def test_message(session, client):
    time = datetime(2025, 1, 28, 17, 50, 0, tzinfo=timezone.utc)
    a1 = DBMessage(
      text = "YO YO HOMIE",
      account_id = 1,
      chat_id = 2,
      created_at = time
    )
    a2 = DBMessage(
      text =  "PICK ME UP",
      account_id =  3,
      chat_id = 4,
      created_at = time
    )

    session.add(a1)
    session.add(a2)
    session.commit()

    response = client.get("/chats/2/messages")
    assert response.status_code == 200
    assert response.json() == {
     "metadata": {
        "count": 1
    },
        "messages": [
        {
         "id": 1,
         "text": "YO YO HOMIE",
         "account_id": 1,
        "chat_id": 2,
        "created_at": "2025-01-28T17:50:00"
        }
    ]
    }

#tests if the /chats/{chat_id}/messages perfroms 
#correctly when a message has more than one in a chat_id
def test_messages(session, client):
    time = datetime(2025, 1, 28, 17, 50, 0, tzinfo=timezone.utc)
    messages = [
        DBMessage(
            text="YO YO HOMIE", 
            account_id=1, chat_id=2, 
            created_at=time),
        DBMessage(
            text="HELLO WORLD", 
            account_id=2, 
            chat_id=2, 
            created_at=time),
        DBMessage(
            text="HOW ARE YOU?", 
            account_id=3, 
            chat_id=2, 
            created_at=time),
        DBMessage(
            text="PICK ME UP", 
            account_id=3, 
            chat_id=4, 
            created_at=time) 
    ]

    session.add_all(messages)
    session.commit()

    response = client.get("/chats/2/messages")
    assert response.status_code == 200
    assert response.json() == {
        "metadata": {
            "count": 3 
        },
        "messages": [
            {
                "id": 1,
                "text": "YO YO HOMIE",
                "account_id": 1,
                "chat_id": 2,
                "created_at": "2025-01-28T17:50:00"
            },
            {
                "id": 2,
                "text": "HELLO WORLD",
                "account_id": 2,
                "chat_id": 2,
                "created_at": "2025-01-28T17:50:00"
            },
            {
                "id": 3,
                "text": "HOW ARE YOU?",
                "account_id": 3,
                "chat_id": 2,
                "created_at": "2025-01-28T17:50:00"
            }
        ]
    }

#tests if the /chats/{chat_id}/messages perfroms correctly 
# when given a chat_id that does not exist
def test_message_error(session, client):
    time = datetime(2025, 1, 28, 17, 50, 0, tzinfo=timezone.utc)
    a1 = DBMessage(
      text = "YO YO HOMIE",
      account_id = 1,
      chat_id = 2,
      created_at = time
    )
    a2 = DBMessage(
      text =  "PICK ME UP",
      account_id =  3,
      chat_id = 4,
      created_at = time
    )

    session.add(a1)
    session.add(a2)
    session.commit()

    response = client.get("/chats/5/messages")
    assert response.status_code == 404
    assert response.json() == {
          "error": "entity_not_found",
          "message": f"Unable to find chat with id=5"
    }

#tests if the /chats/{chat_id}/account perfroms correctly 
def test_membership(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBChatMembership(
        account_id=1,
        chat_id=1
    )

    session.add(a1)
    session.add(a2)
    session.commit()

    response = client.get("/chats/1/accounts")
    assert response.status_code == 200
    assert response.json() == {
        "metadata": {
        "count": 1
        },
            "accounts": [
        {
            "id": 1,
            "username": "drewster"
        }
    ]
    }

#tests if the /chats/{chat_id}/account perfroms correctly 
# when given a chat_id that does not exist
def test_membership_error(session, client):
    a1 = DBAccount(
        username="drewster",
        email="drewster@youknwowho.com",
        hashed_password="sdsdf"
    )
    a2 = DBChatMembership(
        account_id=1,
        chat_id=1
    )

    session.add(a1)
    session.add(a2)
    session.commit()

    response = client.get("/chats/3/accounts")
    assert response.status_code == 404
    assert response.json() == {
                "error": "entity_not_found",
                "message": f"Unable to find chat with id=3"
            }
    

    