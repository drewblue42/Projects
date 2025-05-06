from backend.database.schema import DBAccount

#Author: Andrew Winward
#Performs test in line with the account routes

#Tests if the /accounts performs correctly
def test_accounts(session, client):
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

    response = client.get("/accounts")
    assert response.status_code == 200
    assert response.json() == {
        "metadata": {
        "count": 2
        },
        "accounts": [
        {
         "id": 1,
         "username": "drewster"
        },
        {
        "id": 2,
        "username": "drewblue"
        }
    ]
    }

#tests if the /accounts/{account_id} perfroms correctly 
def test_account(session, client):
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

    response = client.get("/accounts/1")
    assert response.status_code == 200
    assert response.json() == {
            "id": 1,
            "username": "drewster"
    }

#tests if the /accounts/{account_id} perfroms correctly 
# when given an account_id that does not exist
def test_account_error(session, client):
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

    response = client.get("/accounts/3")
    assert response.status_code == 404
    assert response.json() == {
                "error": "entity_not_found",
                "message": f"Unable to find account with id=3"
            }