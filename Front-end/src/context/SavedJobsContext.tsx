"use client";

import { fetchSavedJobs } from "@/api/fetchJobs";
import { JobsContext } from "./JobsContext";

export function SavedJobsProvider({ children }: { children: React.ReactNode }) {
  return (
    <JobsContext.Provider value={fetchSavedJobs}>
      {children}
    </JobsContext.Provider>
  );
}