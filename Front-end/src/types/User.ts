import { Experience } from "./Experience"

export interface User {
    userId: number,
    username: string,
    email: string
    firstName?: string,
    lastName?: string,
    bio?: string,
    experiences?: Experience[],
    isEmployer: boolean
    employerName?: string
}