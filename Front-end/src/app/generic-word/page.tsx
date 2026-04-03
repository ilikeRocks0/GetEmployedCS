"use client";
import { Button, Flex, Layout, Typography, Spin, Alert } from "antd";
import SiteHeader from "@/components/SiteHeader";
import TextArea from "antd/es/input/TextArea";
import { useState } from "react";
import { GenericWordsProvider, useGenericWords } from "@/context/GenericWordContext";
import { GenericWordsAnalysis } from "@/api/genericWords";

const { Title, Text } = Typography;
const { Content } = Layout;

function GenericWordContent(){
    const { processText } = useGenericWords();
    const [text, setText] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [analysis, setAnalysis] = useState<GenericWordsAnalysis | null>(null);

    const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setText(e.target.value);
    };

    async function handleSubmit() {
        if (!text.trim()) return;
        setLoading(true);
        setError(null);
        try {
            const result = await processText(text);
            setAnalysis(result);
        } catch {
            setError("Failed to analyze text. Please try again.");
        } finally {
            setLoading(false);
        }
    }

    return (
        <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
            <SiteHeader selectedKey="/resume-help" />
            <Content style={{ padding: "40px 80px" }}>
                <Title level={1}>Generic Word Detection</Title>
                <Text>
                    Place in a paragraph below to find which words you should replace with stronger words.
                </Text>
                <Flex vertical align="center" gap={10}>
                    <TextArea
                        rows={4}
                        value={text}
                        onChange={handleChange}
                        placeholder="Enter your text here"
                        style={{ width: 800, height: 150, marginTop: 50 }}
                    />
                    <Button onClick={handleSubmit} style={{ width: 400 }} loading={loading}>
                        Process Sentence
                    </Button>
                    {error && <Alert type="error" message={error} style={{ width: 800 }} />}
                    {loading && <Spin />}
                    {analysis && !loading && (
                        <Flex vertical gap={16} style={{ width: 800 }}>
                            <HighlightedText text={text} analysis={analysis} />
                            {analysis.advice && <Text type="secondary">{analysis.advice}</Text>}
                            {analysis.recommendations.length > 0 && (
                                <Flex vertical gap={4}>
                                    {analysis.recommendations.map((r, i) => (
                                        <Text key={i}>
                                            <Text mark>{r.word}</Text> → {r.suggestion}
                                        </Text>
                                    ))}
                                </Flex>
                            )}
                        </Flex>
                    )}
                </Flex>
            </Content>
        </Layout>
    );
}

const PUNCTUATION = /[.,!?;:()\[\]{}"'"]/g;

function HighlightedText({ text, analysis }: { text: string; analysis: GenericWordsAnalysis }) {
    const usePositions = analysis.recommendations.length === 0;
    const posSet = new Set(analysis.positions);
    const wordSet = new Set(analysis.recommendations.map(r => r.word.replace(PUNCTUATION, "").toLowerCase()));

    return (
        <span>
            {text.split(" ").map((token, i) => {
                const clean = token.replace(PUNCTUATION, "").toLowerCase();
                const highlighted = usePositions ? posSet.has(i) : wordSet.has(clean);
                return (
                    <span key={i} style={{ backgroundColor: highlighted ? "yellow" : "transparent" }}>
                        {token}{" "}
                    </span>
                );
            })}
        </span>
    );
}

export default function GenericWordPage(){
    return(
        <GenericWordsProvider>
            <GenericWordContent/>
        </GenericWordsProvider>
    )
}
