import React, { useEffect, useState } from "react";
import { apiFetch } from "../api/api";
import { Link } from "react-router-dom";

type Project = { id: number; title: string; description?: string; createdAt: string; tasksCount: number };

export default function Dashboard() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [title, setTitle] = useState("");
  const [desc, setDesc] = useState("");
  const [err, setErr] = useState("");

  async function load() {
    const res = await apiFetch("/api/projects");
    if (res.ok) setProjects(res.data);
  }

  useEffect(() => { load(); }, []);

  async function create(e: React.FormEvent) {
    e.preventDefault();
    setErr("");
    const res = await apiFetch("/api/projects", {
      method: "POST",
      body: JSON.stringify({ title, description: desc })
    });
    if (!res.ok) {
      setErr(res.data?.message || "Could not create");
      return;
    }
    setTitle(""); setDesc("");
    load();
  }

  async function del(id:number) {
    if (!confirm("Delete project?")) return;
    const res = await apiFetch(`/api/projects/${id}`, { method: "DELETE" });
    if (res.ok) load();
  }

  return (
    <div>
      <h2>Projects</h2>
      <form onSubmit={create}>
        <input value={title} onChange={e=>setTitle(e.target.value)} placeholder="Project title" required minLength={3} />
        <input value={desc} onChange={e=>setDesc(e.target.value)} placeholder="Description (optional)" maxLength={500} />
        <button type="submit">Create</button>
      </form>
      {err && <p style={{color:'red'}}>{err}</p>}
      <ul>
        {projects.map(p => (
          <li key={p.id}>
            <Link to={`/projects/${p.id}`}>{p.title}</Link> — {p.tasksCount} tasks
            <button onClick={()=>del(p.id)} style={{marginLeft:10}}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
