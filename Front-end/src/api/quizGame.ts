import { API_BASE_URL } from "@/config/config";
import type { QuizOptions } from "@/types/QuizOptions";
import { ApiQuizOptions, ApiQuizStats, mapQuizOptions, mapQuizStats } from "@/utils/ApiQuizOptions";
import { fetchWithAuth } from "@/utils/fetchWithAuth";

export async function nextQuiz(): Promise<QuizOptions | null> {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/quiz/next`, {
    method: "POST",
    });
    if(!res.ok) throw new Error(`Failed to get next Quiz game: ${res.status}`);
    
    let apiQuizOptions: ApiQuizOptions | null;
    try{
        apiQuizOptions = await res.json();
    }catch{
        return null;
    }
    return apiQuizOptions ? mapQuizOptions(apiQuizOptions) : null;
}

export async function answerQuiz(answer: string) {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/quiz/answer`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ answer }),
    });
    if (!res.ok) throw new Error(`Failed to answer quiz question: ${res.status}`);
}

export async function statsQuiz() {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/quiz/stats`, {
    method: "POST",
    });
    if (!res.ok) throw new Error(`Failed to get quiz stats: ${res.status}`);
    const apiQuizStats: ApiQuizStats | null = await res.json();
    return apiQuizStats ? mapQuizStats(apiQuizStats) : null;
}

export async function initQuizGame() {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/quiz/game`, {
    method: "POST",
    });
    if (!res.ok) throw new Error(`Failed to initialize quiz game: ${res.status}`);
}