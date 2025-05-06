import React, { useState, useEffect } from "react";
import { NavLink, useNavigate } from "react-router";
import { useMutation } from "@tanstack/react-query";
import api from "../api";
import { useAuth } from "../hooks"; 
import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormInput from "../components/FormInput";
/**
 * This page is in charge of registering new user to the webapp.
 * 
 * Functions:
 * - Register
 * 
 * Author: Andrew Winward
 * 
 */
const headerClassName = "text-center text-4xl font-extrabold py-4";


export default function Register() {
    const { login } = useAuth();
    const navigate = useNavigate();
  
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [errorMsg, setErrorMsg] = useState("");
    const [buttonDisabled, setButtonDisabled] = useState(true);
  
    useEffect(() => {
      if (!username || !email || !password || !confirmPassword) {
        setButtonDisabled(true);
      } else if (password !== confirmPassword) {
        setButtonDisabled(true);
        setErrorMsg("Passwords do not match");
      } else {
        setButtonDisabled(false);
        setErrorMsg("");
      }
    }, [username, email, password, confirmPassword]);
  
    const mutation = useMutation({
        mutationFn: () =>
          api.postForm("/auth/registration", {}, { username, email, password }),
        onMutate: () => setButtonDisabled(true),
        onSuccess: () => {
          api
            .postForm("/auth/token", {}, { username, password })
            .then((tokenData) => {
              login(tokenData.access_token);
              navigate("/chats", { replace: true });
            })
            .catch((err) => {
              console.error("Auto-login failed:", err);
              setErrorMsg("Account created, but login failed.");
            });
        },
        onError: (error) => {
          setButtonDisabled(false);
          setErrorMsg(error.message || "Registration failed: duplicate username/email");
        },
      });
  
    const handleSubmit = (e) => {
      e.preventDefault();
      if (password !== confirmPassword) {
        setErrorMsg("Passwords do not match");
        return;
      }
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
            id="email"
            type="email"
            name="email"
            text="Email"
            value={email}
            setValue={setEmail}
          />
          <FormInput
            id="password"
            type="password"
            name="password"
            text="Password"
            value={password}
            setValue={setPassword}
          />
          <FormInput
            id="confirmPassword"
            type="password"
            name="confirmPassword"
            text="Confirm Password"
            value={confirmPassword}
            setValue={setConfirmPassword}
          />
          {errorMsg && <div className="text-red-600">{errorMsg}</div>}
          <FormButton text="Register" disabled={buttonDisabled} />
        </Form>
        <p className="mt-4 text-center">
          Already have an account?{" "}
          <NavLink to="/login" className="text-blue-500 hover:underline">
            Login here
          </NavLink>
        </p>
      </div>
    );
  }