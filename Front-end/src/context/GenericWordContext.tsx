"use client";

import { getGenericPositions } from "@/api/genericWords";
import { useContext, createContext } from "react";


const GenericWordContext = createContext({
    getGenericPositions: getGenericPositions as (genericWord: string) => Promise<number[]>,
});

export function GenericWordsProvider({children}:{children: React.ReactNode}){
    return(
    <GenericWordContext.Provider value={{ getGenericPositions }}>
        {children}
    </GenericWordContext.Provider>
    )
}

export function useGenericWords() {
  return useContext(GenericWordContext);
}
