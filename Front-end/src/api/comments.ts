import { API_BASE_URL } from "@/config/config";
import type { JobComment } from "@/types/JobComment";
import { fetchWithAuth } from "@/utils/fetchWithAuth";

export async function getComments(jobId: number): Promise<JobComment[]> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/comments/${jobId}`);
  if (!res.ok) throw new Error(`Failed to fetch comments: ${res.status}`);
  return res.json();
}

export async function createComment(jobId: number, comment: string, posterUserId: number): Promise<JobComment> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/comments/create`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ comment, posterUserId, jobId }),
  });
  if (!res.ok) throw new Error(`Failed to create comment: ${res.status}`);
  return res.json();
}
