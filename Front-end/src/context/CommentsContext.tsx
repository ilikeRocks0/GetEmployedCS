"use client";

import { createContext, useContext } from "react";
import type { JobComment } from "@/types/JobComment";
import { getComments, createComment } from "@/api/comments";

const CommentsContext = createContext({
  getComments: getComments as (jobId: number) => Promise<JobComment[]>,
  createComment: createComment as (jobId: number, comment: string) => Promise<JobComment>,
});

export function CommentsProvider({ children }: { children: React.ReactNode }) {
  return (
    <CommentsContext.Provider value={{ getComments, createComment }}>
      {children}
    </CommentsContext.Provider>
  );
}

export function useComments() {
  return useContext(CommentsContext);
}
