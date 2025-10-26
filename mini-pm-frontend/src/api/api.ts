const API_BASE = import.meta.env.VITE_API_BASE ?? "http://localhost:5032";

export function getToken() {
  return localStorage.getItem("token");
}

export function setToken(token: string) {
  localStorage.setItem("token", token);
}

export async function apiFetch(path: string, opts: RequestInit = {}) {
  const headers = opts.headers ? new Headers(opts.headers as any) : new Headers();
  const token = getToken();
  if (token) headers.set("Authorization", `Bearer ${token}`);
  headers.set("Content-Type", "application/json");
  const res = await fetch(`${API_BASE}${path}`, { ...opts, headers });
  const text = await res.text();
  try { return { ok: res.ok, status: res.status, data: text ? JSON.parse(text) : null }; }
  catch { return { ok: res.ok, status: res.status, data: text }; }
}
