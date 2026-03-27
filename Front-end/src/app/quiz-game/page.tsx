"use client";

import SiteHeader from "@/components/SiteHeader"
import { App, Flex, Layout, Space, Spin, Typography } from "antd"
import { useEffect, useRef, useState } from "react";
import { QuizGameProvider, useQuizGame } from "@/context/QuizGameContext";
import { QuizOptions } from "@/types/QuizOptions";
import { QuizStats } from "@/types/QuizStats";
import { QuizView } from "./components/QuizView";
import { QuizStatsView } from "./components/QuizStatsView";

const { Title, Text } = Typography;
const { Content } = Layout

function QuizGame(){
    const {initQuizGame, answerQuiz, nextQuiz, statsQuiz} = useQuizGame();
    const [quizStats, setQuizStats] = useState<QuizStats>();
    const [sentenceOne, setSentenceOne] = useState("");
    const [sentenceTwo, setSentenceTwo] = useState("");
    const [loading, setLoading] = useState(true);
    const [noMore, setNoMore] = useState(false);
    
    async function updateStats() {
        statsQuiz()
            .then((quizStats: QuizStats | null) => {
                if (quizStats != null){
                    setQuizStats(quizStats);
                }
            })
    }

    async function answerSentence(answer: string) {
        answerQuiz(answer)
            .then(() => {
                loadNextSentences();
            })
    }

    async function loadNextSentences() {
        setLoading(true);
        nextQuiz()
            .then((quiz: QuizOptions | null) =>{
                if(quiz != null){
                    setSentenceOne(quiz.sentence1);
                    setSentenceTwo(quiz.sentence2);
                    setLoading(false);
                }else{
                    setNoMore(true);
                }
                updateStats();
            })
    }

    //navigating to this page using router.push() causes the use effect to fire twice
    //this blocks it
    const hasInitialized = useRef(false);

    useEffect(() => {
        if (hasInitialized.current) return;
        hasInitialized.current = true;

        initQuizGame().then(() => {
            loadNextSentences();
        });
    }, []);

    return(
        <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
        <SiteHeader selectedKey= "Resume Help" />
        <Content
            style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            justifyContent: "center",
            padding: "40px 20px",
            }}
        >
        <Title level={1}>Quiz Game</Title>
        <Text>
            Select the stronger sentence out of the two provided below.
        </Text>
        <Space orientation="vertical" size="large" align="center">
        <QuizStatsView quizStats={quizStats}/>
        {!noMore ? (
            <div>
                {loading ? (
            <Spin size="large"/>
        ) : (   
            <Flex vertical align="center">

                <QuizView 
                    sentenceOne={sentenceOne}
                    sentenceTwo={sentenceTwo}
                    answerSentence={answerSentence}
                    loadNextSentences={loadNextSentences}
                    />
            </Flex>
        )}
            </div>
        ) : (
            <div>
                No more
            </div>
        )}
        
        </Space>
        </Content>
        </Layout>
    )
}



export default function QuizGamePage(){
    return(
        <App>
            <QuizGameProvider>
                <QuizGame/>
            </QuizGameProvider>
        </App>
    )
}

