import { Card, Typography, Avatar, Carousel, Button, Tag, Empty } from "antd";
import { PlusCircleOutlined } from "@ant-design/icons";
import { User } from "@/types/User";
import { useState } from "react";
import ProfileEditModal from "./ProfileEditModal";
import { Experience, ExperienceValues } from "@/types/Experience";
import ExperienceEditModal from "./ExperienceEditModal";
import { createExperience, deleteExperience, updateExperience } from "@/api/experience";
import { updateUser, UpdateUserRequest } from "@/api/users";
import ExperienceCard from "./ExperienceCard";
import JobCard from "./JobCard";
import { deleteJob } from "@/api/fetchJobs";
import { mapJob } from "@/utils/ApiJobMapper";
import CreateJobModal from "./CreateJobModal";

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
  const code = name.charCodeAt(0) + (name.charCodeAt(1) ?? 0);
  return AVATAR_COLORS[code % AVATAR_COLORS.length];
}

const { Title, Text } = Typography;

type ProfileViewProps = {
    user: User;
    isSelf: boolean;
    onRefresh: () => void;
};

export default function ProfileView({ user, isSelf, onRefresh }: ProfileViewProps) {
    const [modalOpen, setModalOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const [expModalOpen, setExpModalOpen] = useState(false);
    const [createJobModalOpen, setCreateJobModalOpen] = useState(false);
    const [editingExp, setEditingExp] = useState<Experience | null>(null);

    const handleSaveProfile = async (values: UpdateUserRequest) => {
        setLoading(true);
        try {
            await updateUser(values);
            setModalOpen(false);
            onRefresh();
        } catch (error) {
            console.error("Profile update failed", error);
        } finally {
            setLoading(false);
        }
    }

    const handleExperienceDelete = async (exp: Experience) => {
        await deleteExperience({ companyName: exp.companyName, positionTitle: exp.positionTitle, jobDescription: exp.jobDescription });
        onRefresh();
    };

    const handleExpEditClick = (experience: Experience) => {
        setEditingExp(experience);
        setExpModalOpen(true);
    };

    const handleAddExpClick = () => {
        setEditingExp(null);
        setExpModalOpen(true);
    };

    const handleJobDelete = async (id: number) => {
        await deleteJob(id);
        onRefresh();
    }

    const handleSaveExperience = async (values: ExperienceValues) => {
        try {
        if (editingExp) {
            await updateExperience({ companyName: editingExp.companyName, positionTitle: editingExp.positionTitle, jobDescription: editingExp.jobDescription }, values);
        } else {
            await createExperience(values);
        }
        
        setExpModalOpen(false);
        onRefresh();
    } catch (error) {
        console.error("Save failed", error);
    }
    };

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
                style={{
                    backgroundColor: avatarColor(user.username || "?"),
                    fontWeight: 700,
                    fontSize: 56,
                }}
                >
                    {(user.username || "?").slice(0, 2).toUpperCase()}
                </Avatar>
                {isSelf && <Button onClick={() => setModalOpen(true)} style={{ marginTop: 20 }}> Edit profile </Button>}
            </div>
            <div style={{ flex: 1 }}>
                <div style={{ display: "flex", alignItems: "center", gap: "16px", flexWrap: "wrap" }}>
                    <Title level={1} style={{ marginBottom: 0 }}>
                        { (user.isEmployer) ?
                            (user.employerName) ? `${user.employerName}` : `${user.username}` : 
                            (user.firstName || user.lastName) ?
                                `${user.firstName ?? ''} ${user.lastName ?? ''}`.trim() : user.username 
                        }
                    </Title>

                    {user.isEmployer ? (
                        <Tag color="blue" style={{ fontSize: '18px', padding: '6px 12px', borderRadius: '12px', marginTop: '5px' }}>
                            Employer
                        </Tag>
                    ) : (
                        <Tag color="green" style={{ fontSize: '18px', padding: '6px 12px', borderRadius: '12px', marginTop: '5px' }}>
                            Job Seeker
                        </Tag>
                    )}
                </div>

                <div style={{ marginTop: 4 }}>
                    <Text type="secondary">
                        {user.email}
                    </Text>
                </div>

                <Card style={{ marginTop: 24, maxWidth: 700 }}>
                    <Title level={3}>
                        {(user.isEmployer ) ? "About the Company" : "About Me"}
                    </Title>

                    <Text>
                        {user.bio}
                    </Text>
                </Card>

                {!user.isEmployer && (
                    <div style={{ marginTop: 32, maxWidth: 900 }}>
                        <Title level={3}>Experience</Title>
                        <Carousel
                            dots={true}
                            arrows={true}
                            draggable={true}
                            slidesToShow={3}
                            slidesToScroll={1}
                            infinite={false}
                            responsive={[
                                { breakpoint: 1024, settings: { slidesToShow: 2 } },
                                { breakpoint: 600, settings: { slidesToShow: 1 } }
                            ]}
                        >
                            {isSelf && (
                                <div style={{ padding: '0 10px' }}>
                                    <Card
                                        hoverable
                                        onClick={handleAddExpClick}
                                        style={{
                                            width: 260,
                                            height: 220,
                                            display: 'flex',
                                            justifyContent: 'center',
                                            alignItems: 'center',
                                            border: '2px dashed #d9d9d9',
                                            cursor: 'pointer'
                                        }}
                                    >
                                        <div style={{ textAlign: 'center' }}>
                                            <PlusCircleOutlined style={{ fontSize: 32, color: '#1890ff' }} />
                                            <div style={{ marginTop: 8 }}>Add Experience</div>
                                        </div>
                                    </Card>
                                </div>
                            )}
                            {user.experiences && user.experiences.length > 0 ? (
                                user.experiences?.map((exp) => (
                                    <div key={exp.experienceId} style={{ padding: '0 10px' }}>
                                        <ExperienceCard isSelf={isSelf} experience={exp} onEdit={handleExpEditClick} onDelete={handleExperienceDelete} />
                                    </div>
                                ))
                            ) : (
                                <Empty description="No experiences" />
                            )}
                        </Carousel>
                    </div>
                )}

                <div style={{ marginTop: 32, maxWidth: 900 }}>
                    <Title level={3}>Active Postings</Title>
                    {isSelf && (
                        <Button
                            type="dashed"
                            icon={<PlusCircleOutlined />}
                            block
                            style={{ height: 60, marginBottom: 12 }}
                            onClick={() => setCreateJobModalOpen(true)}
                        >
                            Post a New Job
                        </Button>
                    )}
                    {user.postedJobs && user.postedJobs.length > 0 ? (
                        user.postedJobs.map((job, i) => (
                            <JobCard key={job.jobId ?? i} job={mapJob(job)} isCurrentUsers={isSelf} onDelete={handleJobDelete}/>
                        ))
                    ) : (
                        <Empty description="No active job postings" />
                    )}
                </div>
            </div>
            <ProfileEditModal open={modalOpen} user={user} loading={loading} onClose={() => setModalOpen(false)} onSave={handleSaveProfile}/>
            <ExperienceEditModal open={expModalOpen} initialValues={editingExp} onClose={() => setExpModalOpen(false)} onSave={handleSaveExperience} />
            <CreateJobModal open={createJobModalOpen} onClose={() => setCreateJobModalOpen(false)} onCreated={onRefresh} />
        </div>
    )
}