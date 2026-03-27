import { API_BASE_URL } from "@/config/config";
import { fetchWithAuth } from "@/utils/fetchWithAuth";


export async function getGenericPositions(genericWord: string): Promise<number[]>{
    const res = await fetchWithAuth(`${API_BASE_URL}/api/genericWord`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ genericWord }),
    });
    if (!res.ok) throw new Error(`Failed to fetch word index: ${res.status}`);
    return res.json();
}