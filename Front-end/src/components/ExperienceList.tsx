import { Experience } from "@/types/Experience";
import ExperienceCard from "./ExperienceCard";

type ExperienceListProps = {
    experiences: Experience[]
}

export default function ExperienceList({experiences}: ExperienceListProps) {
    return (
        <div style={{display: "flex", gap: 20,flexWrap: "wrap",}}>
            {experiences.map(exp => (
                <ExperienceCard key={exp.experienceId} experience={exp} />
            ))}
        </div>
    )
}
