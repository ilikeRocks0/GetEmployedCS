import { API_BASE_URL } from "@/config/config";
import type { Job } from "@/types/Job";
import type { JobFilters } from "@/types/JobFilters";
import { mapJob, ApiJob } from "@/utils/ApiJobMapper"


function buildParams(filters: JobFilters): URLSearchParams {
  const params = new URLSearchParams();
  if (filters.searchText) params.set("searchTerm", filters.searchText);
  if (filters.selectedType) params.set("employmentTypes", filters.selectedType);
  filters.selectedLanguages.forEach((lang) => params.append("languages", lang));
  return params;
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

export async function fetchSavedJobs(
  filters: JobFilters,
  page: number,
  pageSize: number,
  signal: AbortSignal
): Promise<{ data: Job[]; total: number }> {
  const params = buildParams(filters);

  const res = await fetch(`${API_BASE_URL}/api/jobs/saved?${params}`, { signal });

  if (!res.ok) throw new Error(`Failed to fetch jobs: ${res.status}`);

  const apiJobs: ApiJob[] = await res.json();
  const total = apiJobs.length;
  const start = (page - 1) * pageSize;
  const data = apiJobs.slice(start, start + pageSize).map(mapJob);

  return { data, total };
}
