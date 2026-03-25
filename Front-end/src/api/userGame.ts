import { API_BASE_URL } from "@/config/config";
import type { User } from "@/types/User";
import { fetchWithAuth } from "@/utils/fetchWithAuth";

export async function initUserGame(): Promise<User | null> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/game`, {
    method: "POST",
  });
  if (!res.ok) throw new Error(`Failed to initialize quick hire: ${res.status}`);
  return res.json();
}

export async function acceptUser(username: string): Promise<User | null> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/game/accept/${username}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
  });
  if (!res.ok) throw new Error(`Failed to accept user: ${res.status}`);
  return res.json();
}

export async function rejectUser(username: string): Promise<User | null> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/game/reject/${username}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
  });
  if (!res.ok) throw new Error(`Failed to reject user: ${res.status}`);
  return res.json();
}
