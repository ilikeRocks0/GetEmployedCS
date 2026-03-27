"use client";

import { nextQuiz, answerQuiz, statsQuiz, initQuizGame } from "@/api/quizGame";
import { QuizOptions } from "@/types/QuizOptions";
import { QuizStats } from "@/types/QuizStats";
import { createContext, useContext } from "react";

const QuizGameContext = createContext({
    initQuizGame: initQuizGame as () => Promise<void>,
    answerQuiz: answerQuiz as (answer: string) => Promise<void>,
    statsQuiz: statsQuiz as () => Promise<QuizStats| null>,
    nextQuiz: nextQuiz as () => Promise<QuizOptions| null>
})

export function QuizGameProvider({children}: {children: React.ReactNode}) {
    return (
        <QuizGameContext.Provider
            value={{
                initQuizGame,
                answerQuiz,
                statsQuiz,
                nextQuiz,
            }}
        >
            {children}
        </QuizGameContext.Provider>
    )
}

export function useQuizGame() {
    return useContext(QuizGameContext)
}