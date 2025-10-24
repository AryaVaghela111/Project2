// import React from "react";
import { Routes, Route, Link, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Dashboard from "./pages/Dashboard";
import ProjectDetails from "./pages/ProjectDetails";
import { getToken } from "./api/api";
import type { JSX } from "react";

function PrivateRoute({ children }: { children: JSX.Element }) {
  return getToken() ? children : <Navigate to="/login" />;
}

export default function App() {
  return (
    <div style={{ padding: 20 }}>
      <nav style={{ marginBottom: 20 }}>
        <Link to="/">Home</Link> | <Link to="/dashboard">Dashboard</Link>
      </nav>
      <Routes>
        <Route path="/" element={<div>Welcome — <Link to="/dashboard">Dashboard</Link></div>} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/dashboard" element={<PrivateRoute><Dashboard /></PrivateRoute>} />
        <Route path="/projects/:id" element={<PrivateRoute><ProjectDetails /></PrivateRoute>} />
      </Routes>
    </div>
  );
}
