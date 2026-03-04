import { API_BASE_URL } from "@/config/config";
import type { Job } from "@/types/Job";
import type { JobFilters } from "@/types/JobFilters";

// Shape of a job object returned by the backend
export interface ApiJob {
  jobTitle: string;
  applicationDeadline: string | null;
  posterName: string | null;
  applicationLink: string;
  hasRemote: boolean | null;
  hasHybrid: boolean | null;
  positionType: string;
  employmentType: string;
  locations: string[] | null;
  programmingLanguages: string[] | null;
  jobDescription: string;
}

export function buildParams(filters: JobFilters): URLSearchParams {
  const params = new URLSearchParams();
  if (filters.searchText) params.set("searchTerm", filters.searchText);
  if (filters.selectedType) params.set("employmentTypes", filters.selectedType);
  filters.selectedLanguages.forEach((lang) => params.append("languages", lang));
  return params;
}

export function mapJob(apiJob: ApiJob, index: number): Job {
  return {
    id: index,
    company: apiJob.posterName ?? "",
    position: apiJob.jobTitle,
    languages: apiJob.programmingLanguages ?? [],
    employment_type: apiJob.employmentType,
    description: apiJob.jobDescription,
    locations: apiJob.locations ?? [],
    isHybrid: apiJob.hasHybrid ?? false,
    isRemote: apiJob.hasRemote ?? false,
    position_type: apiJob.positionType,
    deadline: apiJob.applicationDeadline ?? null,
    applicationLink: apiJob.applicationLink,
  };
}

export async function fetchLanguages(): Promise<string[]> {
  const res = await fetch(`${API_BASE_URL}/api/jobs/languages`);
  if (!res.ok) throw new Error(`Failed to fetch languages: ${res.status}`);
  return res.json();
}

export async function fetchJobs(
  filters: JobFilters,
  page: number,
  pageSize: number,
  signal: AbortSignal
): Promise<{ data: Job[]; total: number }> {
  const params = buildParams(filters);

  const res = await fetch(`${API_BASE_URL}/api/jobs?${params}`, { signal });

  if (!res.ok) throw new Error(`Failed to fetch jobs: ${res.status}`);

  const apiJobs: ApiJob[] = await res.json();
  const total = apiJobs.length;
  const start = (page - 1) * pageSize;
  const data = apiJobs.slice(start, start + pageSize).map(mapJob);

  return { data, total };
}
