import React from "react";
import { BrowserRouter } from "react-router-dom";
import Router from "./routes";
import "./App.css";
import { UserProvider } from "./contexts/userContext";

function App() {
  return (
    <BrowserRouter>
      <UserProvider>
        <Router />
      </UserProvider>
    </BrowserRouter>
  );
}

export default App;
