"use client";

import SiteHeader from "@/components/SiteHeader"
import { App, Layout, Typography } from "antd"

const { Title, Text } = Typography;
const { Content } = Layout

function ResumeHelp(){
    return(
        <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
            <SiteHeader selectedKey="Resume Help" />
            <Content style={{ padding: "40px 80px" }}>
                <Title level={1}>Resume Helper Tools</Title>
                <Text>Looks like this page is still being worked on. Come back later when its finished!</Text>
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