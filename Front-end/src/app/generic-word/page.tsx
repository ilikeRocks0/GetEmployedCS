"use client";
import { Button, Flex, Layout, Typography } from "antd";
import SiteHeader from "@/components/SiteHeader";
import TextArea from "antd/es/input/TextArea";
import { useEffect, useState } from "react";
import { GenericWordsProvider, useGenericWords } from "@/context/GenericWordContext";

const { Title, Text } = Typography;
const { Content } = Layout;
function GenericWordContent(){
    const [text, setText] = useState('');
    const [processed, setProcessed] = useState("");

    const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setText(e.target.value);
    };
    

    function handleSubmit() {
        setProcessed(text);
    }

    return (
        <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
            <SiteHeader selectedKey="/quick-hire" />
                <Content style={{ padding: "40px 80px" }}>
                    <Title level={1}>Generic Word Detection</Title>
                    <Text>
                        Place in a paragraph below to find which words you should replace with stronger words.
                    </Text>
                    <Flex vertical align="center" gap={10}>
                        <TextArea rows={4} value={text} onChange={handleChange} placeholder="Enter your text here" style={{ width: 800, height: 150, marginTop: 50}}/>
                        <Button onClick={handleSubmit} style={{ width: 400}}>Process Sentence</Button>
                        <HighlightedText text={processed} />
                    </Flex>
                </Content>
        </Layout>
    )
}

function HighlightedText({ text }: { text: string }) {
    const {getGenericPositions} = useGenericWords();
    const [indexes, setIndexes] = useState<number[]>([]);

    useEffect(() => {
        getGenericPositions(text).then(setIndexes);
    }, [text]);

    return (
        <span>
            {text.split(" ").map((word, i) => (
                <span key={i} style={{ backgroundColor: indexes.includes(i) ? "yellow" : "transparent" }}>
                    {word}{" "}
                </span>
            ))}
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