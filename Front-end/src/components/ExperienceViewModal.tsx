import { Modal, Typography, Divider, Avatar } from "antd";
import { UserOutlined } from "@ant-design/icons";
import { Experience } from "@/types/Experience";

const { Title, Text, Paragraph } = Typography;

interface ExperienceViewModalProps {
    experience: Experience | null;
    open: boolean;
    onClose: () => void;
}

export default function ExperienceViewModal({ experience, open, onClose }: ExperienceViewModalProps) {
    if (!experience) return null;

    return (
        <Modal
            open={open}
            onCancel={onClose}
            footer={null}
            title={null}
        >
            <div style={{ padding: "10px 0" }}>
                <div style={{ display: "flex", alignItems: "center", gap: 16, marginBottom: 16 }}>
                    <Avatar size={56} icon={<UserOutlined />} style={{ backgroundColor: "#111", flexShrink: 0 }} />
                    <div>
                        <Title level={4} style={{ margin: 0 }}>{experience.companyName}</Title>
                        <Text type="secondary">{experience.positionTitle}</Text>
                    </div>
                </div>

                <Divider style={{ margin: "12px 0" }} />

                <Title level={5} style={{ marginBottom: 8 }}>Description</Title>
                <Paragraph style={{ whiteSpace: "pre-wrap", color: "#555" }}>
                    {experience.jobDescription || "No description provided."}
                </Paragraph>
            </div>
        </Modal>
    );
}