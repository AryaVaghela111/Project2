import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { apiFetch } from "../api/api";

export default function Register() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [err, setErr] = useState("");
  const nav = useNavigate();

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    setErr("");
    const res = await apiFetch("/api/auth/register", {
      method: "POST",
      body: JSON.stringify({ username, password })
    });
    if (!res.ok) {
      setErr(res.data?.message || "Register failed");
      return;
    }
    nav("/login");
  }

  return (
    <div>
      <h2>Register</h2>
      <form onSubmit={submit}>
        <div><input value={username} onChange={e => setUsername(e.target.value)} placeholder="username" required /></div>
        <div><input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="password" required minLength={6} /></div>
        <button type="submit">Register</button>
      </form>
      {err && <p style={{color:'red'}}>{err}</p>}
    </div>
  );
}
