import { API_BASE_URL } from "@/config/config";
import { fetchWithAuth } from "@/utils/fetchWithAuth";

export interface WordRecommendation {
    word: string;
    suggestion: string;
}

export interface GenericWordsAnalysis {
    positions: number[];
    advice: string;
    recommendations: WordRecommendation[];
}

export async function getGenericPositions(genericWord: string): Promise<number[]>{
    const res = await fetchWithAuth(`${API_BASE_URL}/api/genericWord`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ genericWord }),
    });
    if (!res.ok) throw new Error(`Failed to fetch word index: ${res.status}`);
    return res.json();
}

export async function analyzeGenericWords(genericWord: string): Promise<GenericWordsAnalysis> {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/genericWord/analyze`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ genericWord }),
    });
    if (!res.ok) throw new Error(`Failed to analyze text: ${res.status}`);
    return res.json();
}

export async function getGroqStatus(): Promise<boolean> {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/genericWord/status`);
    if (!res.ok) return false;
    const data = await res.json();
    return data.groqAvailable as boolean;
}