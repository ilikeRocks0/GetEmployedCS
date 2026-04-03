"use client";

import { analyzeGenericWords, getGenericPositions, getGroqStatus, GenericWordsAnalysis } from "@/api/genericWords";
import { useContext, createContext, useState, useEffect } from "react";

interface GenericWordContextType {
    processText: (text: string) => Promise<GenericWordsAnalysis>;
}

const GenericWordContext = createContext<GenericWordContextType>({
    processText: async () => ({ positions: [], advice: "", recommendations: [] }),
});

export function GenericWordsProvider({ children }: { children: React.ReactNode }) {
    const [groqAvailable, setGroqAvailable] = useState(false);

    useEffect(() => {
        getGroqStatus().then(setGroqAvailable);
    }, []);

    async function processText(text: string): Promise<GenericWordsAnalysis> {
        if (groqAvailable) {
            return analyzeGenericWords(text);
        }
        const positions = await getGenericPositions(text);
        return { positions, advice: "", recommendations: [] };
    }

    return (
        <GenericWordContext.Provider value={{ processText }}>
            {children}
        </GenericWordContext.Provider>
    );
}

export function useGenericWords() {
    return useContext(GenericWordContext);
}
