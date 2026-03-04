export interface Job {
  id: number;
  company: string;
  position: string;
  languages: string[];
  locations: string[];
  isHybrid: boolean;
  isRemote: boolean;
  position_type: string;
  employment_type: string;
  logo?: string;
  description: string;
  deadline: string | null;
  applicationLink: string;
}
