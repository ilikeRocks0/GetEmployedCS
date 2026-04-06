import { API_BASE_URL } from "@/config/config";
import type { JobComment, UserComment } from "@/types/JobComment";
import { fetchWithAuth } from "@/utils/fetchWithAuth";

export async function getComments(jobId: number): Promise<JobComment[]> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/comments/${jobId}`);
  if (!res.ok) throw new Error(`Failed to fetch comments: ${res.status}`);
  return res.json();
}

export async function createComment(jobId: number, comment: string): Promise<JobComment> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/comments/create`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ comment, jobId }),
  });
  if (!res.ok) throw new Error(`Failed to create comment: ${res.status}`);
  return res.json();
}

export async function getUserComments(username: string): Promise<UserComment[]> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/usercomments/${username}`);
  if (!res.ok) throw new Error(`Failed to fetch user comments: ${res.status}`);
  return res.json();
}

export async function createUserComment(username: string, comment: string): Promise<UserComment> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/usercomments/create`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ comment, commentedUserUsername: username }),
  });
  if (!res.ok) throw new Error(`Failed to create user comment: ${res.status}`);
  return res.json();
}

export async function notifyProfileComment(profileUsername: string, commentText: string): Promise<void> {
  await fetchWithAuth(`${API_BASE_URL}/api/usercomments/notify`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ comment: commentText, commentedUserUsername: profileUsername }),
  });
}
