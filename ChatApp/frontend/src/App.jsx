import { BrowserRouter, Routes, Route, NavLink, Outlet, useParams, Navigate, useNavigate, } from "react-router";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import React, { useState, useEffect } from "react";
import AuthProvider from "./providers/AuthProvider";
import { useAuth } from "./hooks";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Settings from "./pages/Settings";
import { Chats, ChatsWithID } from "./pages/chats";
/**
 * This page is in charge of loading and controlling the main app, with authentication
 * 
 * Functions:
 * - RequiredAuth
 * - App
 * 
 * Author: Andrew Winward
 * 
 */

const headerClassName = "text-center text-4xl font-extrabold py-4";
const queryClient = new QueryClient();

function RequireAuth({ children }) {
  const { loggedIn } = useAuth();
  if (!loggedIn) {
    return <Navigate to="/" replace />;
  }
  return children;
}

function NotFound() {
  return <h1 className={headerClassName}>404: Not Found</h1>;
}

export default function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route
              path="/settings"
              element={
                <RequireAuth>
                  <Settings />
                </RequireAuth>
              }
            />
            <Route
              path="/chats"
              element={
                <RequireAuth>
                  <Chats />
                </RequireAuth>
              }
            >
              <Route path=":chatId" element={<ChatsWithID />} />
              <Route path="settings" element={<Settings />} />
            </Route>
            <Route path="*" element={<NotFound />} />
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </QueryClientProvider>
  );
}
