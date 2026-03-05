"use client";

import { createContext, useContext } from "react";
import type { Job } from "@/types/Job";
import {
  initJobGame,
  acceptJob,
  rejectJob,
} from "@/api/jobGame";

const JobGameContext = createContext({
  initJobGame: initJobGame as () => Promise<Job | null>,
  acceptJob,
  rejectJob,
});

export function JobGameProvider({ children }: { children: React.ReactNode }) {
  return (
    <JobGameContext.Provider
      value={{
        initJobGame,
        acceptJob,
        rejectJob,
      }}
    >
      {children}
    </JobGameContext.Provider>
  );
}

export function useJobGame() {
  return useContext(JobGameContext);
}
