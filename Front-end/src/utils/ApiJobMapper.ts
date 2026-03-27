import type { Job } from "@/types/Job";

// Shape of a job object returned by the backend
export interface ApiJob {
  jobId: number;
  jobTitle: string;
  applicationDeadline: string | null;
  posterName: string | null;
  employerPoster: boolean;
  applicationLink: string;
  hasRemote: boolean | null;
  hasHybrid: boolean | null;
  positionType: string;
  employmentType: string;
  locations: string[] | null;
  programmingLanguages: string[] | null;
  jobDescription: string;
}

export function mapJob(apiJob: ApiJob): Job {
  return {
    id: apiJob.jobId,
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
    posterName: apiJob.posterName ?? null,
    employerPoster: apiJob.employerPoster,
  };
}