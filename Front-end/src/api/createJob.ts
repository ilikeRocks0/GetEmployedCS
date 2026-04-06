import { API_BASE_URL } from "@/config/config";
import { fetchWithAuth } from "@/utils/fetchWithAuth";
import { checkIfUserIsEmployer } from "@/api/users";

export interface NewJobRequest {
  title: string;
  jobDescription: string;
  applicationLink: string;
  employmentType: string;
  positionType: string;
  locations: string[];
  programmingLanguages: string[];
  deadline: string | null;
  hasRemote: boolean;
  hasHybrid: boolean;
}

export async function createJob(payload: NewJobRequest): Promise<void> {
  const employerPoster = await checkIfUserIsEmployer();
  const res = await fetchWithAuth(`${API_BASE_URL}/api/job/add`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ ...payload, employerPoster }),
  });
  if (!res.ok) throw new Error(`Failed to create job: ${res.status}`);
}

export async function notifyFollowers(jobTitle: string): Promise<void> {
  await fetchWithAuth(`${API_BASE_URL}/api/jobs/notify?jobTitle=${encodeURIComponent(jobTitle)}`, {
    method: "POST",
  });
}
