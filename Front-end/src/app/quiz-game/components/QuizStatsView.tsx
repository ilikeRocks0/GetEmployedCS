import { QuizStats } from "@/types/QuizStats";

export function QuizStatsView({quizStats}: {quizStats: QuizStats | undefined}){
    return (
        <div style={{ display: "flex", gap: 32, marginBottom: 32, fontSize: 15, fontWeight: 500 }}>
            <span style={{ color: "#ff4d4f" }}>✕ {quizStats ? quizStats.incorrect : 0} Incorrect</span>
            <span style={{ color: "#52c41a" }}>✓ {quizStats ? quizStats.correct : 0} Correct</span>
            <span style={{ color: "#adb3a9" }}>{quizStats ? quizStats.skipped : 0} Skipped</span>
        </div>
    )
}