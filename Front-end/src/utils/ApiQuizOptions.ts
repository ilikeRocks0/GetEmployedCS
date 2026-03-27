import type { QuizOptions } from "@/types/QuizOptions"
import { QuizStats } from "@/types/QuizStats";

export interface ApiQuizOptions{
    sentence1: string;
    sentence2: string;
}

export function mapQuizOptions(apiQuizOptions: ApiQuizOptions): QuizOptions{
    return{
        sentence1: apiQuizOptions.sentence1,
        sentence2: apiQuizOptions.sentence2,
    };
}

export interface ApiQuizStats{
    correct: number;
    incorrect: number;
    skipped: number;
}

export function mapQuizStats(ApiQuizStats: ApiQuizStats): QuizStats{
    return{
        correct: ApiQuizStats.correct,
        incorrect: ApiQuizStats.incorrect,
        skipped: ApiQuizStats.skipped
    };
}