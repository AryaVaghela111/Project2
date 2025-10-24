import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { apiFetch } from "../api/api";

type Task = { id: number; title: string; isCompleted: boolean; dueDate?: string | null; projectId: number };

export default function ProjectDetails() {
  const { id } = useParams();
  const [project, setProject] = useState<any>(null);
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState("");
  const [due, setDue] = useState<string>("");
  const [err, setErr] = useState("");

  async function load() {
    const res = await apiFetch(`/api/projects/${id}`);
    if (res.ok) {
      setProject(res.data);
      setTasks(res.data.tasks || []);
    }
  }
  useEffect(() => { load(); }, [id]);

  async function addTask(e: React.FormEvent) {
    e.preventDefault();
    setErr("");
    const body:any = { title };
    if (due) body.dueDate = due;
    const res = await apiFetch(`/api/projects/${id}/tasks`, { method: "POST", body: JSON.stringify(body) });
    if (!res.ok) { setErr(res.data?.message || "Failed to add"); return; }
    setTitle(""); setDue("");
    load();
  }

  async function toggle(t: Task) {
    const res = await apiFetch(`/api/tasks/${t.id}`, {
      method: "PUT",
      body: JSON.stringify({ title: t.title, dueDate: t.dueDate, isCompleted: !t.isCompleted })
    });
    if (res.ok) load();
  }

  async function del(taskId:number) {
    if (!confirm("Delete task?")) return;
    const res = await apiFetch(`/api/tasks/${taskId}`, { method: "DELETE" });
    if (res.ok) load();
  }

  if (!project) return <div>Loading...</div>;
  return (
    <div>
      <h2>{project.title}</h2>
      <p>{project.description}</p>

      <h3>Tasks</h3>
      <form onSubmit={addTask}>
        <input value={title} onChange={e=>setTitle(e.target.value)} placeholder="Task title" required />
        <input value={due} onChange={e=>setDue(e.target.value)} type="date" />
        <button type="submit">Add Task</button>
      </form>
      {err && <p style={{color:'red'}}>{err}</p>}
      <ul>
        {tasks.map((t: Task) => (
          <li key={t.id}>
            <input type="checkbox" checked={t.isCompleted} onChange={() => toggle(t)} />
            {t.title} {t.dueDate ? `- due ${new Date(t.dueDate).toLocaleDateString()}` : ""}
            <button onClick={() => del(t.id)} style={{marginLeft:10}}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
