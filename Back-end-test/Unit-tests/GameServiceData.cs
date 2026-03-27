using Back_end.Persistence.Objects;

public class GameServiceData
{
    public static List<Job> Empty = new List<Job>();
    public static List<Job> OneJob = new List<Job>
    {
            new Job(
            jobTitle: "Canada Hydro",
            applicationDeadline: new DateOnly(2026, 2,2),
            posterName: "Marco Gerra",
            applicationLink: "https://linkdon/2025.com",
            employerPoster: false,
            hasRemote: true,
            hasHybrid: true,
            positionType: "Full-Stack",
            employmentType: "Internship",
            locations: ["Winnipeg, Manitoba"],
            programmingLanguages: ["React"],
            jobDescription: "Build and maintain user-facing features for our internal energy management dashboard using React and TypeScript."
        ),
    };

    public static List<Job> JobsList = new List<Job>
    {
        new Job(
            jobTitle: "Canada Hydro",
            applicationDeadline: new DateOnly(2026, 2,2),
            posterName: "Marco Gerra",
            applicationLink: "https://linkdon/2025.com",
            employerPoster: false,
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
            employerPoster: false,
            hasRemote: true,
            hasHybrid: true,
            positionType: "Backend",
            employmentType: "Internship",
            locations: ["Winnipeg, Manitoba"],
            programmingLanguages: ["C# asp.net"],
            jobDescription: "Design and implement RESTful APIs and microservices to support our growing suite of enterprise SaaS products."
        ),
        new Job(
            jobTitle: "InnovateX",
            applicationDeadline: new DateOnly(2027, 5,26),
            posterName: "Daniel",
            applicationLink: "https://getajob.com",
            employerPoster: false,
            hasRemote: true,
            hasHybrid: true,
            positionType: "Front-end",
            employmentType: "full time",
            locations: ["Saskatchuwon"],
            programmingLanguages: ["React"],
            jobDescription: "Create wireframes, prototypes, and high-fidelity designs for mobile and web products in close collaboration with engineering"
        ),
    };
}