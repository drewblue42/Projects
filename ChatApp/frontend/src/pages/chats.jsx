import React, { useState, useEffect, useRef } from "react";
import { NavLink, Outlet, useParams, useNavigate } from "react-router";
import { useAuth } from "../hooks";
import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormInput from "../components/FormInput";
import api from "../api";
/**
 * This page is in charge of loading the chat interface for the main webapp
 * 
 * Functions:
 * - CreateMessage
 * - GetAccount
 * - Chats
 * - ChatsWithId
 * - GetUsernameMessage
 * 
 * Author: Andrew Winward
 * 
 */
const headerClassName = "text-center text-4xl font-extrabold py-4";


export function CreateMessage({ chatId, onMessageAdded, isMember }) {
    const [text, setText] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const { account, token } = useAuth();

    const Submit = async (e) => {
        e.preventDefault();
        if (!text.trim()) return;

        setIsSubmitting(true);

        if (!account) {
            setError("No account was found!!");
            setIsSubmitting(false);
            return;
        }
        const response = await api.post(`/chats/${chatId}/messages`,
            { Authorization: `Bearer ${token}` },
            { text: text, account_id: account.id });

        onMessageAdded(response);
        setText("");

        setIsSubmitting(false);
    };

    return (
        <Form onSubmit={Submit}>
            <FormInput
                id="message-input"
                type="text"
                text="Type your message..."
                name="message"
                value={text}
                setValue={setText}
                disabled={!isMember}
            />
            <FormButton
                text="Send"
                disabled={!text.trim() || isSubmitting || !isMember}
            />
        </Form>
    );
}

export function GetAccount() {
    const { account, logout } = useAuth();
    const navigate = useNavigate();

    if (!account) return null;

    const handleLogout = () => {
        logout();
        navigate("/", { replace: true });
    };

    return (
        <div style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
            <h3 style={{ fontSize: "30px", fontWeight: "bold" }}>{account.username}</h3>
            <NavLink to="/chats/settings" style={({ isActive }) => ({
                padding: "5px",
                border: "2px solid black",
                backgroundColor: isActive ? "black" : "white",
                color: isActive ? "yellow" : "black",
                textDecoration: "none"
            })}>
                settings
            </NavLink>
            <button
                onClick={handleLogout}
                style={{
                    padding: "5px",
                    border: "2px solid black",
                    backgroundColor: "white",
                    color: "black",
                    cursor: "pointer",
                    textAlign: "left"
                }}
            >
                logout
            </button>
        </div>
    );
}

export function Chats() {
    const [chats, setChats] = useState([]);

    useEffect(() => {
        api.get("/chats")
            .then((data) => {
                const sortedChats = data.chats.sort((a, b) =>
                    a.name.localeCompare(b.name)
                );
                setChats(sortedChats);
            })
            .catch((err) => console.error("Error fetching chats:", err));
    }, []);

    return (
        <div style={{ display: "flex", height: "100vh" }}>
            <div style={{
                width: "300px",
                minWidth: "300px",
                maxWidth: "300px",
                borderRight: "2px solid black",
                padding: "1rem",
                display: "flex",
                flexDirection: "column",
                overflow: "hidden"
            }}>
                <h1 style={{ fontSize: "30px", fontWeight: "bold", marginBottom: "20px" }}>
                    Pony Express
                </h1>

                <GetAccount />

                <h2 style={{ fontSize: "24px", fontWeight: "bold", margin: "20px 0 10px 0" }}>
                    Chats
                </h2>
                <ul style={{ listStyle: "none", padding: 0 }}>
                    {chats.map((chat) => (
                        <li
                            key={chat.id}
                            style={{ marginBottom: "8px", border: "2px solid black" }}
                        >
                            <NavLink
                                to={`/chats/${chat.id}`}
                                style={({ isActive }) => ({
                                    padding: "8px",
                                    display: "block",
                                    whiteSpace: "nowrap",
                                    textDecoration: "none",
                                    backgroundColor: isActive ? "black" : "white",
                                    color: isActive ? "yellow" : "black",
                                    transition: "background-color 0.3s",
                                })}
                            >
                                {chat.name}
                            </NavLink>
                        </li>
                    ))}
                </ul>
            </div>
            <div style={{
                flexGrow: 1,
                padding: "1rem",
                overflowY: "auto"
            }}>
                <Outlet />
            </div>
        </div>
    );
}

export function ChatsWithID() {
    const { chatId } = useParams();
    const { account, token } = useAuth();
    const [messages, setMessages] = useState([]);

    const listRef = useRef(null);

    useEffect(() => {
        api.get(`/chats/${chatId}/messages`)
            .then((data) => {
                const sortedMessages = data.messages.sort(
                    (a, b) => new Date(a.created_at) - new Date(b.created_at)
                );
                setMessages(sortedMessages);
            })
            .catch((err) => console.error("Error fetching messages:", err));
    }, [chatId]);

    useEffect(() => {
        if (listRef.current) {
            listRef.current.scrollTo({
                top: listRef.current.scrollHeight,
                behavior: "smooth",
            });
        }
    }, [messages]);


    const handleEdit = editedMessage => {
        setMessages(messages =>
            messages.map(message => (message.id === editedMessage.id ? editedMessage : message)))
    }

    const handleDelete = deletedId => {
        setMessages(messages => messages.filter(m => m.id !== deletedId));
    };


    const isMember = true;

    return (
        <div className="flex flex-col h-[calc(100lvh-100px)]">
            <ul ref={listRef} style={{overflowY: "auto", display: "flex", flexDirection: "column"}}
            >
                {messages.map((message) => (
                    <GetUsernameMessage key={message.id}
                        message={message}
                        onEdit={handleEdit}
                        onDelete={handleDelete}
                        currentId={account?.id}
                        token={token}
                        chatId={chatId}
                    />
                ))}
            </ul>
            <CreateMessage
                chatId={chatId}
                onMessageAdded={message =>
                    setMessages(messages => [...messages, message])}
                isMember={isMember}
            />
        </div>
    );
}

export function GetUsernameMessage({ message, onEdit, onDelete, currentId, token, chatId }) {
    const [account, setAccount] = useState(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedText, setEditedText] = useState(message.text);


    useEffect(() => {
        if (message.account_id !== null) {
            api.get(`/accounts/${message.account_id}`)
                .then((data) => setAccount(data))
                .catch((err) => console.error("Error fetching account:", err));
        }
    }, [message.account_id]);

    const Owner = message.account_id === currentId;

    const handleSave = async () => {
        const updated = await api.put(
            `/chats/${chatId}/messages/${message.id}`,
            { Authorization: `Bearer ${token}` },
            { text: editedText }
        );
        onEdit(updated);
        setIsEditing(false);
    };

    const handleCancel = () => {
        setEditedText(message.text);
        setIsEditing(false);
    };

    const handleDelete = async () => {
        await api.deleteChat(
            `/chats/${chatId}/messages/${message.id}`,
            { Authorization: `Bearer ${token}` }
        );
        onDelete(message.id);
    };


    return (
        <li
            style={{
                marginBottom: 15,
                border: "2px solid black",
                borderRadius: 12,
                padding: 10,
                display: "flex",
                flexDirection: "column",
                backgroundColor: "white",
            }}
        >
            <div
                style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                }}
            >
                <strong>
                    {message.account_id === null
                        ? "[removed]"
                        : account
                            ? account.username
                            : message.account_id}
                </strong>
                <span>{new Date(message.created_at).toLocaleString()}</span>
            </div>
            {isEditing ? (
                <Form onSubmit={e => { e.preventDefault(); handleSave(); }} >
                    <FormInput
                        id={`edit-${message.id}`}
                        type="text"
                        text=""
                        name="message"
                        value={editedText}
                        setValue={setEditedText}
                    />
                    <div>
                        <FormButton text="Save" disabled={!editedText.trim()} />
                        <FormButton
                            text="Cancel"
                            type="button"
                            onClick={handleCancel}
                        />
                    </div>
                </Form>
            ) : (
                <div>
                    <p>{message.text}</p>
                    {Owner && (
                        <div style={{
                            display: "flex",
                            flexDirection: "row",
                            justifyContent: "flex-end"
                        }}>
                            <FormButton
                                text="edit"
                                type="button"
                                onClick={() => setIsEditing(true)}
                            />
                            <FormButton
                                text="delete"
                                type="button"
                                onClick={handleDelete}
                            />
                        </div>
                    )}
                </div>
            )}
        </li>
    );
}
