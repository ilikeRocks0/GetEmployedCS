"use client";

import { createContext, useContext } from "react";
import type { Job } from "@/types/Job";
import type { JobFilters } from "@/types/JobFilters";
import { fetchJobs } from "@/api/fetchJobs";

export type FetchJobs = (
  filters: JobFilters,
  page: number,
  pageSize: number,
  signal: AbortSignal
) => Promise<{ data: Job[]; total: number }>;

const JobsContext = createContext<FetchJobs>(fetchJobs);

export function JobsProvider({ children }: { children: React.ReactNode }) {
  return (
    <JobsContext.Provider value={fetchJobs}>
      {children}
    </JobsContext.Provider>
  );
}

export function useJobs(): FetchJobs {
  return useContext(JobsContext);
}
