import { Card, Typography, Avatar, Button } from "antd";
import { UserOutlined } from "@ant-design/icons";
import ExperienceList from "./ExperienceList";
import { User } from "@/types/User";
const { Title, Text } = Typography;

type ProfileViewProps = {
    user: User,
    isSelf: boolean
};

export default function ProfileView({ user, isSelf }: ProfileViewProps) {
    return (
        <div
            style={{
            display: "flex",
            gap: "40px",
            marginTop: 24,
            alignItems: "flex-start",
            }}
        >
            <div
                style={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                width: 200,
                }}
            >
                <Avatar
                size={160}
                icon={<UserOutlined />}
                style={{
                backgroundColor: "#111",
                }}
                />
                {isSelf && <Button style={{ marginTop: 20 }}> Edit profile </Button>}
            </div>
            <div style={{ flex: 1 }}>
                <Title level={1} style={{ marginBottom: 0 }}>
                    {user.firstName + " " + user.lastName}
                </Title>

                <Card style={{ marginTop: 24, maxWidth: 700 }}>
                    <Title level={3}>
                        About Me
                    </Title>

                    <Text>
                        {user.bio}
                    </Text>
                </Card>

                <div style={{ marginTop: 32 }}>
                    <Title level={3}>
                        Experience
                    </Title>

                    {user.experiences && <ExperienceList experiences={user.experiences}/>}
                </div>
            </div>
        </div>
    )
}