import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { apiFetch, setToken } from "../api/api";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [err, setErr] = useState("");
  const nav = useNavigate();

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    setErr("");
    const res = await apiFetch("/api/auth/login", {
      method: "POST",
      body: JSON.stringify({ username, password })
    });
    if (!res.ok) {
      setErr(res.data?.message || "Login failed");
      return;
    }
    setToken(res.data.token);
    nav("/dashboard");
  }

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={submit}>
        <div><input value={username} onChange={e => setUsername(e.target.value)} placeholder="username" required /></div>
        <div><input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="password" required /></div>
        <button type="submit">Login</button>
      </form>
      {err && <p style={{color:'red'}}>{err}</p>}
      <p>Don't have an account? <Link to="/register">Register</Link></p>
    </div>
  );
}
