import React from "react";
import { NavLink, Navigate } from "react-router";
import { useAuth } from "../hooks";
/**
 * This page is in charge of the start page. When an unauthenticated user goes to app, they will be directed here
 * Functions:
 * - home
 * 
 * Author: Andrew Winward
 * 
 */
const headerClassName = "text-center text-4xl font-extrabold py-4";

export default function Home() {
    const { loggedIn } = useAuth();
    if (loggedIn) {
        return <Navigate to="/chats" replace />;
    }
    return (
        <div style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
        }}>
            <h1 className={headerClassName}>Pony Express</h1>

            <div style={{
                display: "flex",
                flexDirection: "column",
                gap: "10px",
                alignItems: "center"
            }}>
                <NavLink to="/login" style={{
                    border: "2px solid black",
                    borderRadius: "12px",
                    padding: "10px 20px",
                    textDecoration: "none",
                    width: "fit-content",
                    textAlign: "center"
                }}>
                    Please login here
                </NavLink>

                <span>or</span>

                <NavLink to="/register" style={{
                    border: "2px solid black",
                    borderRadius: "12px",
                    padding: "10px 20px",
                    textDecoration: "none",
                    width: "fit-content",
                    textAlign: "center"
                }}>
                    register a new account here
                </NavLink>
            </div>
        </div>
    );
}
