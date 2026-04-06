export interface Comment {
  comment: string;
  posterUserId: number;
  posterUsername?: string;
}

export interface JobComment extends Comment {
  jobId: number;
}

export interface UserComment extends Comment {
  profileUserId: number;
}
