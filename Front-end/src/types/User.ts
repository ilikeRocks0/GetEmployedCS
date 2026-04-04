import { Experience } from "./Experience"
import { ApiJob } from "@/utils/ApiJobMapper"

export interface User {
    userId: number;
    username: string;
    email: string;
    firstName?: string;
    lastName?: string;
    bio?: string;
    experiences?: Experience[];
    isEmployer: boolean;
    employerName?: string;
    postedJobs?: ApiJob[];
    isSelf?: boolean;
    isFollowing?: boolean;
}

export interface ProfileValues {
    firstName?: string;
    lastName?: string;
    bio?: string;
}