using Back_end.Persistence.Objects;

public class JobServiceData()
{
    public static List<Job> JobList = new List<Job>
    {
        new Job(
            jobTitle: "Canada Hydro",
            applicationDeadline: new DateOnly(2026, 2,2),
            posterName: "Marco Gerra",
            applicationLink: "https://linkdon/2025.com",
            hasRemote: true,
            hasHybrid: true,
            positionType: "Full-Stack",
            employmentType: "Internship",
            locations: ["Winnipeg, Manitoba"],
            programmingLanguages: ["React"],
            jobDescription: "Build and maintain user-facing features for our internal energy management dashboard using React and TypeScript."
        ),
        new Job(
            jobTitle: "TechCorp",
            applicationDeadline: new DateOnly(2026, 3,6),
            posterName: "Ty Paragas",
            applicationLink: "https://realsite.com",
            hasRemote: true,
            hasHybrid: false,
            positionType: "Backend",
            employmentType: "Internship",
            locations: ["Winnipeg, Manitoba"],
            programmingLanguages: ["C#"],
            jobDescription: "Design and implement RESTful APIs and microservices to support our growing suite of enterprise SaaS products."
        ),
        new Job(
            jobTitle: "InnovateX",
            applicationDeadline: new DateOnly(2027, 5,26),
            posterName: "Daniel",
            applicationLink: "https://getajob.com",
            hasRemote: false,
            hasHybrid: true,
            positionType: "Front-end",
            employmentType: "full time",
            locations: ["Saskatchuwon"],
            programmingLanguages: ["React, Java"],
            jobDescription: "Create wireframes, prototypes, and high-fidelity designs for mobile and web products in close collaboration with engineering"
        ),
        new Job(
            jobTitle: "CloudScale",
            applicationDeadline: new DateOnly(2026, 4, 15),
            posterName: "Billy Bob",
            applicationLink: "https://cloudscale.dev",
            hasRemote: true,
            hasHybrid: false,
            positionType: "Backend",
            employmentType: "Contract",
            locations: ["Toronto, Ontario"],
            programmingLanguages: ["Java", "SQL"],
            jobDescription: "Build scalable cloud-native APIs and optimize database performance."
        ),
        new Job(
            jobTitle: "PixelWorks",
            applicationDeadline: new DateOnly(2026, 5, 10),
            posterName: "John Doe",
            applicationLink: "https://pixelworks.io",
            hasRemote: true,
            hasHybrid: true,
            positionType: "Front-End",
            employmentType: "Internship",
            locations: ["Vancouver, British Columbia"],
            programmingLanguages: ["React", "TypeScript", "CSS"],
            jobDescription: "Develop interactive UI components and improve user experience."
        ),
    };

    public static List<Job> SavedJobs = new List<Job>
    {
        new Job(
            jobTitle: "Canada Hydro",
            applicationDeadline: new DateOnly(2026, 2,2),
            posterName: "Marco Gerra",
            applicationLink: "https://linkdon/2025.com",
            hasRemote: true,
            hasHybrid: true,
            positionType: "Full-Stack",
            employmentType: "Internship",
            locations: ["Winnipeg, Manitoba"],
            programmingLanguages: ["React"],
            jobDescription: "Build and maintain user-facing features for our internal energy management dashboard using React and TypeScript."
        ),
        new Job(
            jobTitle: "TechCorp",
            applicationDeadline: new DateOnly(2026, 3,6),
            posterName: "Ty Paragas",
            applicationLink: "https://realsite.com",
            hasRemote: true,
            hasHybrid: false,
            positionType: "Backend",
            employmentType: "Internship",
            locations: ["Winnipeg, Manitoba"],
            programmingLanguages: ["C#"],
            jobDescription: "Design and implement RESTful APIs and microservices to support our growing suite of enterprise SaaS products."
        ),
        new Job(
            jobTitle: "CloudScale",
            applicationDeadline: new DateOnly(2026, 4, 15),
            posterName: "Billy Bob",
            applicationLink: "https://cloudscale.dev",
            hasRemote: true,
            hasHybrid: false,
            positionType: "Backend",
            employmentType: "Contract",
            locations: ["Toronto, Ontario"],
            programmingLanguages: ["Java", "SQL"],
            jobDescription: "Build scalable cloud-native APIs and optimize database performance."
        ),
        new Job(
            jobTitle: "AppyTappy",
            applicationDeadline: new DateOnly(2026, 7, 20),
            posterName: "Jane Doe",
            applicationLink: "https://appytappy.com",
            hasRemote: false,
            hasHybrid: false,
            positionType: "Full-Stack",
            employmentType: "Contract",
            locations: ["Calgary, Alberta"],
            programmingLanguages: ["Kotlin", "Swift", "Java"],
            jobDescription: "Develop and maintain applications."
        ),
    };
}