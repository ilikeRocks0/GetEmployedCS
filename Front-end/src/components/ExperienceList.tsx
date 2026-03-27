import { Experience } from "@/types/Experience";
import ExperienceCard from "./ExperienceCard";

type ExperienceListProps = {
    isSelf: boolean,
    experiences: Experience[]
    onEdit: (exp: Experience) => void;
    onDelete: (id: number) => void;
}

export default function ExperienceList({isSelf, experiences, onEdit, onDelete}: ExperienceListProps) {
    return (
        <div style={{display: "flex", gap: 20,flexWrap: "wrap",}}>
            {experiences.map(exp => (
                <ExperienceCard key={exp.experienceId} isSelf={isSelf} experience={exp} onEdit={() => onEdit(exp)} onDelete={() => onDelete(exp.experienceId)}/>
            ))}
        </div>
    )
}
