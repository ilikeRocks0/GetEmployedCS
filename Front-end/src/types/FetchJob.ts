import { Job } from "./Job";
import { JobFilters } from "./JobFilters";

export type FetchJobs = (
  filters: JobFilters,
  page: number,
  pageSize: number,
  signal: AbortSignal
) => Promise<{ data: Job[]; total: number }>;