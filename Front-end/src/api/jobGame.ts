import { API_BASE_URL } from "@/config/config";
import type { Job } from "@/types/Job";
import { ApiJob, mapJob } from "@/utils/ApiJobMapper";

export async function initJobGame(): Promise<Job | null> {
  const res = await fetch(`${API_BASE_URL}/api/job/game`);
  if (!res.ok) throw new Error(`Failed to initialize job game: ${res.status}`);
  const apiJob: ApiJob = await res.json();
  return mapJob(apiJob, 0);
}

export async function acceptJob(): Promise<Job> {
  const res = await fetch(`${API_BASE_URL}/api/job/game/accept`, { method: "POST" });
  if (!res.ok) throw new Error(`Failed to accept job: ${res.status}`);
  const apiJob: ApiJob = await res.json();
  return mapJob(apiJob, 0);
}

export async function rejectJob(): Promise<Job> {
  const res = await fetch(`${API_BASE_URL}/api/job/game/reject`, { method: "POST" });
  if (!res.ok) throw new Error(`Failed to reject job: ${res.status}`);
  const apiJob: ApiJob = await res.json();
  return mapJob(apiJob, 0);
}
