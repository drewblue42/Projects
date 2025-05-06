import React, { useState } from "react";
import { NavLink, Navigate, useNavigate } from "react-router";
import { useMutation } from "@tanstack/react-query";
import api from "../api";
import { useAuth } from "../hooks";
import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormInput from "../components/FormInput";
/**
 * This page is in charge of logging in users, and applying tokens for contiued authenticated use
 * 
 * Functions:
 * - Login
 * 
 * Author: Andrew Winward
 * 
 */
const headerClassName = "text-center text-4xl font-extrabold py-4";


export default function Login() {
    const { login, loggedIn } = useAuth();
    const navigate = useNavigate();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMsg, setErrorMsg] = useState("");
    const [buttonDisabled, setButtonDisabled] = useState(false);

    const mutation = useMutation({
        mutationFn: () =>
            api.postForm("/auth/token", {}, { username, password }),
        onMutate: () => setButtonDisabled(true),
        onSuccess: (data) => {
            login(data.access_token);
            navigate("/chats", { replace: true });
        },
        onError: (error) => {
            setButtonDisabled(false);
            setErrorMsg(error.message);
        },
    });

    if (loggedIn) {
        return <Navigate to="/chats" replace />;
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        mutation.mutate();
    };

    return (
        <div>
            <h1 className={headerClassName}>Pony Express</h1>
            <Form onSubmit={handleSubmit}>
                <FormInput
                    id="username"
                    type="text"
                    name="username"
                    text="Username"
                    value={username}
                    setValue={setUsername}
                />
                <FormInput
                    id="password"
                    type="password"
                    name="password"
                    text="Password"
                    value={password}
                    setValue={setPassword}
                />
                {errorMsg && <div className="text-red-600">{errorMsg}</div>}
                <FormButton text="Login" disabled={buttonDisabled} />
                <button type="button">
                    <NavLink
                        to="/register"
                        style={{ color: "blue", display: "flex", justifyContent: "flex-start" }}
                        className="underline"
                    >
                        Register new account
                    </NavLink>
                </button>
            </Form>
        </div>
    );
}