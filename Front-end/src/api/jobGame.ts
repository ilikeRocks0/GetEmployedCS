import { API_BASE_URL } from "@/config/config";
import type { Job } from "@/types/Job";
import { ApiJob, mapJob } from "@/utils/ApiJobMapper";
import { getUserIdFromSession } from "@/utils/getIdsFromStubSession";

export async function initJobGame(): Promise<Job | null> {
  const userId = getUserIdFromSession();
  console.log(JSON.stringify({ userId }))
  const res = await fetch(`${API_BASE_URL}/api/job/game`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ userId }),
  });
  if (!res.ok) throw new Error(`Failed to initialize job game: ${res.status}`);
  const apiJob: ApiJob | null = await res.json();
  return apiJob ? mapJob(apiJob) : null;
}

export async function acceptJob(jobId: number): Promise<Job | null> {
  const userId = getUserIdFromSession();
  const res = await fetch(`${API_BASE_URL}/api/job/game/accept`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ userId, jobId }),
  });
  if (!res.ok) throw new Error(`Failed to accept job: ${res.status}`);
  const apiJob: ApiJob | null = await res.json();
  return apiJob ? mapJob(apiJob) : null;
}

export async function rejectJob(jobId: number): Promise<Job | null> {
  const userId = getUserIdFromSession();
  const res = await fetch(`${API_BASE_URL}/api/job/game/reject`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ userId, jobId }),
  });
  if (!res.ok) throw new Error(`Failed to reject job: ${res.status}`);
  const apiJob: ApiJob | null = await res.json();
  return apiJob ? mapJob(apiJob) : null;
}
