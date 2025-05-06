import React, { useState, useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import Form from "../components/Form";
import FormButton from "../components/FormButton";
import FormInput from "../components/FormInput";
import { useAuth } from "../hooks";
import { NavLink, useNavigate } from "react-router";
/**
 * This page is in charge of the settings. Allows the user to update account, change password, or logout/delete account
 * 
 * Functions:
 * - UpdateAccount
 * - UpdatePassword
 * - ManageAccount
 * - Settings
 * 
 * Author: Andrew Winward
 * 
 */
const headerClassName = "text-center text-4xl font-extrabold py-4";
const SettingsHeaderClassName = "text-center text-4xl font-extrabold py-4"

function UpdateAccount() {
    const { headers } = useAuth();
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    useEffect(() => {
        async function getAccountDetails() {
            try {
                const response = await fetch("http://localhost:8000/accounts/me", { headers });
                if (response.ok) {
                    const data = await response.json();
                    setUsername(data.username);
                    setEmail(data.email);
                } else {
                    console.error("Failed to get account details");
                }
            } catch (error) {
                console.error("Error getting account details:", error);
            }
        }
        getAccountDetails();
    }, [headers]);


    const mutation = useMutation({
        mutationFn: async ({ username, email }) => {
            const response = await fetch("http://localhost:8000/accounts/me", {
                method: "PUT",
                headers: { ...headers, "Content-Type": "application/json" },
                body: JSON.stringify({ username, email }),
            });

            if (!response.ok) {
                let errorData = {};
                try {
                    errorData = await response.json();
                } catch (err) {
                }
                throw new Error(errorData.message || "Update failed");
            }

            if (response.status === 204) {
                return {};
            }

            return response.json();
        },
        onSuccess: () => {
            setSuccess("Account updated successfully!");
            setError("");
        },
        onError: (error) => {
            setError(error.message);
            setSuccess("");
        },
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        mutation.mutate({ username, email });
    };

    return (
        <section className="update-account-section">
            <h2 className={SettingsHeaderClassName}>Update Account</h2>
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
                {error && <div style={{ color: "red" }}>{error}</div>}
                {success && <div style={{ color: "green" }}>{success}</div>}
                <FormButton
                    text={mutation.isLoading ? "Updating..." : "Update Account"}
                    disabled={mutation.isLoading}
                />
            </Form>
        </section>
    );
}

function UpdatePassword() {
    const { headers } = useAuth();
    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmNewPassword, setConfirmNewPassword] = useState("");
    const [pwdError, setPwdError] = useState("");
    const [pwdSuccess, setPwdSuccess] = useState("");

    const mutation = useMutation({
        mutationFn: async ({ oldPassword, newPassword }) => {
            const formData = new FormData();
            formData.append("old_password", oldPassword);
            formData.append("new_password", newPassword);
            const response = await fetch("http://localhost:8000/accounts/me/password", {
                method: "PUT",
                headers: { ...headers },
                body: formData,
            });
            if (!response.ok) {
                const errData = await response.json();
                throw new Error(errData.message || "Password update failed");
            }
            return response;
        },
        onSuccess: () => {
            setPwdSuccess("Password updated successfully!");
            setPwdError("");
            setOldPassword("");
            setNewPassword("");
            setConfirmNewPassword("");
        },
        onError: (error) => {
            setPwdError(error.message);
            setPwdSuccess("");
        },
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        if (newPassword && newPassword !== confirmNewPassword) {
            setPwdError("New passwords do not match");
            return;
        }
        mutation.mutate({ oldPassword, newPassword });
    };

    return (
        <section className="update-password-section">
            <h2 className={SettingsHeaderClassName}>Update Password</h2>
            <Form onSubmit={handleSubmit}>
                <FormInput
                    id="oldPassword"
                    type="password"
                    name="oldPassword"
                    text="Current Password"
                    value={oldPassword}
                    setValue={setOldPassword}
                />
                <FormInput
                    id="newPassword"
                    type="password"
                    name="newPassword"
                    text="New Password"
                    value={newPassword}
                    setValue={setNewPassword}
                />
                <FormInput
                    id="confirmNewPassword"
                    type="password"
                    name="confirmNewPassword"
                    text="Confirm New Password"
                    value={confirmNewPassword}
                    setValue={setConfirmNewPassword}
                />
                {pwdError && <div style={{ color: "red" }}>{pwdError}</div>}
                {pwdSuccess && <div style={{ color: "green" }}>{pwdSuccess}</div>}
                <FormButton
                    text={mutation.isLoading ? "Updating..." : "Update Password"}
                    disabled={mutation.isLoading || (newPassword && newPassword !== confirmNewPassword)}
                />
            </Form>
        </section>
    );
}

function ManageAccount() {
    const { logout, headers } = useAuth();
    const navigate = useNavigate();
    const [error, setError] = useState("");

    const deleteMutation = useMutation({
        mutationFn: async () => {
            const response = await fetch("http://localhost:8000/accounts/me", {
                method: "DELETE",
                headers,
            });
            if (!response.ok) {
                let errorData = {};
                try {
                    errorData = await response.json();
                } catch (err) {
                }
                throw new Error(errorData.message || "Delete account failed");
            }
            return response;
        },
        onSuccess: () => {
            logout();
            navigate("/", { replace: true });
        },
        onError: (error) => {
            setError(error.message);
        },
    });

    const handleDelete = () => {
        deleteMutation.mutate();
    };

    const handleLogout = () => {
        logout();
        navigate("/", { replace: true });
    };

    return (
        <div>
            <h2 className={SettingsHeaderClassName}>Manage Account</h2>
            <div style={{ display: "flex", justifyContent: "center", flexDirection: "column", alignItems: "center" }}>
                <div style={{ marginBottom: "1rem" }}>
                    <FormButton text="Logout" onClick={handleLogout} />
                </div>
                <div>
                    <FormButton
                        text={"Delete Account"}
                        onClick={handleDelete}
                        disabled={deleteMutation.isLoading}
                    />
                </div>
                {error && <div style={{ color: "red" }}>{error}</div>}
            </div>
        </div>
    );
}

export default function Settings() {
    return (
        <div >
            <h1 className={headerClassName}>Account Settings</h1>
            <div style={{
                overflowY: "auto", height: 600
            }}>
                <UpdateAccount />
                <UpdatePassword />
                <ManageAccount />
            </div>
        </div>
    );
}