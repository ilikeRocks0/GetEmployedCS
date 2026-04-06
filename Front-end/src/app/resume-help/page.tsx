"use client";

import SiteHeader from "@/components/SiteHeader"
import { App, Button, Divider, Layout, Typography } from "antd"
import { useRouter } from "next/navigation";

const { Title, Text } = Typography;
const { Content } = Layout

function ResumeHelp(){
    const router = useRouter();
    
    return(
        <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
            <SiteHeader selectedKey="/resume-help" />
            <Content style={{ padding: "40px 80px" }}>
                <Title level={1}>Resume Helper Tools</Title>
                <Text>Having issues with writing your resume? look no further here are some tools to help you get started!</Text>
                
                <Divider/>
                <Title level={3}>Quiz Game</Title>
                <Text>Look at two different sentences and pick the stronger of the two. This allows you to recognize which sentences are weak when you are writing your own resumes</Text>
                <br/>
                <Button color="blue" shape="round" variant="solid" onClick={() => router.push("/quiz-game")}>Check it out!</Button>
                
                <Divider/>
                <Title level={3}>Generic Word Detection</Title>
                <Text>Each word counts when you write a resume, this tool highlights words you should switch to stronger words!</Text>
                <br/>
                <Button color="blue" shape="round" variant="solid" onClick={() => router.push("/generic-word")}>Check it out!</Button>
            </Content>            
        </Layout>
    )
}
export default function ResumeHelpPage(){
    return(
        <App>
            <ResumeHelp/>
        </App>
    )
}