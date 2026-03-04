import ALL_JOBS from "@/data/JobsStub";
import type { Job } from "@/types/Job";

// Module-level game state
let queue: Job[] = [];
let cursor = 0;

export async function initJobGame(): Promise<Job | null> {
  queue = [...ALL_JOBS].sort(() => Math.random() - 0.5);
  cursor = 0;
  return queue[0] ?? null;
}

function nextJob(): Job {
  cursor++;
  if (cursor >= queue.length) {
    // Reshuffle and loop so swipes never run out
    queue = [...queue].sort(() => Math.random() - 0.5);
    cursor = 0;
  }
  return queue[cursor];
}

export async function acceptJob(): Promise<Job> {
  return nextJob();
}

export async function rejectJob(): Promise<Job> {
  return nextJob();
}
