import { Avatar, Button, Card, Typography } from "antd";
import { UserOutlined } from "@ant-design/icons";
import { Experience } from "@/types/Experience";

const { Title, Text } = Typography;
type ExperienceCardProps = {
    experience: Experience
};

export default function ExperienceCard({ experience }: ExperienceCardProps) {
    return (
        <Card
            style={{
                width: 260,
                display: "flex",
                flexDirection: "column",
            }}
        >
            <div style={{ display: "flex", alignItems: "center", gap: 12, marginBottom: 10 }}>
                <Avatar
                size="large"
                icon={<UserOutlined />}
                style={{ backgroundColor: "#111" }}
                />

                <Title level={4} style={{ margin: 0 }}>
                    {experience.company}
                </Title>
            </div>

            <Text>
                {experience.title}
            </Text>

            <div style={{ marginTop: 10 }}>
                <Button type="primary">
                    See More
                </Button>
            </div>
        </Card>
    )
}