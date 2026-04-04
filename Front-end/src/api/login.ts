import { API_BASE_URL } from "@/config/config";

export interface UserInfo {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
}

export async function login(email: string, password: string): Promise<void> {
  const res = await fetch(`${API_BASE_URL}/api/users/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify({ email, password }),
  });

  if (res.status === 401) throw new Error("Invalid username or password.");
  if (!res.ok) throw new Error(`Login failed: ${res.status}`);

  const userInfo: UserInfo = await res.json();
  localStorage.setItem("user", JSON.stringify(userInfo));
  document.cookie = "is_logged_in=true; path=/; SameSite=Lax; Secure";
}

export async function logout(): Promise<void> {
  localStorage.removeItem("user");
  document.cookie = "is_logged_in=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; SameSite=Lax; Secure";
  await fetch(`${API_BASE_URL}/api/users/logout`, {
    method: "POST",
    credentials: "include",
  });
}
