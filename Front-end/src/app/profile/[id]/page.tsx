"use client";

import { Layout, Spin } from "antd";
import SiteHeader from "@/components/SiteHeader";
import ProfileView from "@/components/ProfileView";
import { UsersProvider, useUser } from "@/context/UserContext";
import { use, useEffect, useState } from "react";
import { User } from "@/types/User";

const { Content } = Layout;
type ProfileProps = {
    userId: number
}

function ProfilePageContent({ userId }: ProfileProps) {

    const { fetchUser } = useUser();
    const [loading, setLoading] = useState(false);
    const [user, setUser] = useState<User>();
    const [notFound, setNotFound] = useState(false);

    useEffect(() => {
        const controller = new AbortController();
        setNotFound(false);
        setLoading(true);

        const timer = setTimeout(async () => {
            try {
                const result = await fetchUser(userId, controller.signal);
                setUser(result);
            } catch (err: unknown) {
                if ((err as DOMException).name !== "AbortError") throw err;

                const error = err as { message?: string; status?: number };
                if (error.status === 404 || error.message?.includes("User not found")) {
                    setNotFound(true);
                }
            } finally {
                setLoading(false);
            }
        }, 300);

        return () => {
            clearTimeout(timer);
            controller.abort();
        };
    }, [userId, fetchUser]);

    return (
        <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
            <SiteHeader selectedKey="/" />
            <Content style={{ padding: "40px 80px" }}>
                {loading ? (
                    <div style={{ textAlign: 'center', marginTop: '50px' }}>
                        <Spin size="large" description="Loading profile..." />
                    </div>
                ) : notFound ? (
                    <div style={{ textAlign: 'center', marginTop: '50px' }}>
                        <h1>404 - User Not Found</h1>
                        <p>The user with ID {userId} does not exist.</p>
                    </div>
                ) : user ? (
                    <ProfileView user={user} isSelf={true}/>
                ) : null}
            </Content>
        </Layout>
    );
}

export default function ProfilePage({
    params,
}: {
    params: Promise<{ id: number }>
}) {
    const { id } = use(params);
    return(
        <UsersProvider>
            <ProfilePageContent userId={id}/>
        </UsersProvider>
    )
}