import { Button, Popconfirm, Typography } from "antd";
import { DeleteOutlined, EditOutlined, BankOutlined } from "@ant-design/icons";
import { Experience } from "@/types/Experience";
import ExperienceViewModal from "./ExperienceViewModal";
import { useState } from "react";

const { Text } = Typography;

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
    const code = name.charCodeAt(0) + (name.charCodeAt(1) ?? 0);
    return AVATAR_COLORS[code % AVATAR_COLORS.length];
}

type ExperienceCardProps = {
    isSelf: boolean;
    experience: Experience;
    onEdit: (exp: Experience) => void;
    onDelete: (exp: Experience) => void;
};

export default function ExperienceCard({ isSelf, experience, onEdit, onDelete }: ExperienceCardProps) {
    const [viewModalOpen, setViewModalOpen] = useState(false);
    const color = avatarColor(experience.companyName || "?");

    return (
        <>
            <div
                onClick={() => setViewModalOpen(true)}
                style={{
                    background: "#fff",
                    border: "1px solid #f0f0f0",
                    borderRadius: 12,
                    padding: "16px 20px",
                    display: "flex",
                    flexDirection: "column",
                    gap: 12,
                    cursor: "pointer",
                    transition: "box-shadow 0.2s",
                    boxShadow: "0 1px 4px rgba(0,0,0,0.06)",
                    width: 260,
                    minHeight: 220,
                }}
                onMouseEnter={e => (e.currentTarget.style.boxShadow = "0 4px 16px rgba(0,0,0,0.10)")}
                onMouseLeave={e => (e.currentTarget.style.boxShadow = "0 1px 4px rgba(0,0,0,0.06)")}
            >
                {/* Header row */}
                <div style={{ display: "flex", alignItems: "center", gap: 12 }}>
                    <div style={{
                        width: 40,
                        height: 40,
                        borderRadius: 8,
                        backgroundColor: color + "1a",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        flexShrink: 0,
                    }}>
                        <BankOutlined style={{ fontSize: 18, color }} />
                    </div>
                    <div style={{ flex: 1, minWidth: 0 }}>
                        <Text strong style={{ display: "block", fontSize: 15, lineHeight: "20px" }}>
                            {experience.positionTitle}
                        </Text>
                        <Text type="secondary" style={{ fontSize: 13 }}>
                            {experience.companyName}
                        </Text>
                    </div>
                </div>

                {/* Description preview */}
                <Text type="secondary" style={{ fontSize: 13, lineHeight: "1.6", flex: 1 }}>
                    {experience.jobDescription?.length > 120
                        ? experience.jobDescription.slice(0, 120) + "..."
                        : experience.jobDescription}
                </Text>

                {/* Actions */}
                {isSelf && (
                    <div
                        style={{ display: "flex", gap: 8, justifyContent: "flex-end" }}
                        onClick={e => e.stopPropagation()}
                    >
                        <Button
                            type="text"
                            size="small"
                            icon={<EditOutlined />}
                            onClick={() => onEdit(experience)}
                        />
                        <Popconfirm
                            title="Delete experience?"
                            description="Are you sure you want to remove this?"
                            onConfirm={() => onDelete(experience)}
                            okText="Yes"
                            cancelText="No"
                        >
                            <Button
                                type="text"
                                size="small"
                                danger
                                icon={<DeleteOutlined />}
                            />
                        </Popconfirm>
                    </div>
                )}
            </div>

            <ExperienceViewModal
                experience={experience}
                open={viewModalOpen}
                onClose={() => setViewModalOpen(false)}
            />
        </>
    );
}
