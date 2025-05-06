import { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { AuthContext } from "../contexts";

const tokenKey = "recipes_access_token";

export default function AuthProvider({ children }) {
  const [token, setToken] = useState(null);
  const [account, setAccount] = useState(null);
  const [loggedIn, setLoggedIn] = useState(false);

  const login = (newToken) => {
    setToken(newToken);
    setLoggedIn(true);
    localStorage.setItem("access_token", newToken);
  };

  const logout = () => {
    setToken(null);
    setLoggedIn(false);
    localStorage.removeItem("access_token");
  };

  const headers = token ? { Authorization: `Bearer ${token}` } : {};


  ///to get the auth for all pages, instead of doing it function by functionx
  useEffect(() => {
    async function fetchAccount() {
      if (!token) return;

      try {
        const response = await fetch("http://localhost:8000/accounts/me", { headers });
        
        if (response.ok && response.headers.get("Content-Type")?.includes("application/json")) {
          const data = await response.json();
          setAccount(data);
        } else {
          console.error("Failed to fetch account data; response:", response.status);
          if (response.status === 401 || response.status === 403) {
            logout();
          }
        }
      } catch (error) {
        console.error("Error fetching account:", error);
      }
    }
    fetchAccount();
  }, [token, headers]);

  const contextValue = {
    token,
    account,
    loggedIn,
    headers,
    login,
    logout,
  };
  return (
    <AuthContext.Provider value={contextValue}>
      {children}
    </AuthContext.Provider>
  );
};