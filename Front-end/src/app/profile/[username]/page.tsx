"use client";

import { Layout, Spin } from "antd";
import SiteHeader from "@/components/SiteHeader";
import ProfileView from "@/components/ProfileView";import { UsersProvider, useUser } from "@/context/UserContext";
import { use, useCallback, useEffect, useState } from "react";
import { User } from "@/types/User";

const { Content } = Layout;
type ProfileProps = {
    username: string
}

function ProfilePageContent({ username }: ProfileProps) {

    const { fetchUser } = useUser();
    const [loading, setLoading] = useState(false);
    const [user, setUser] = useState<User>();
    const [notFound, setNotFound] = useState(false);

    const loadUserData = useCallback(async (signal?: AbortSignal) => {
        try {
            if (!user) setLoading(true);

            const result = await fetchUser(username, signal);
            setUser(result);
            setNotFound(false);
        } catch (err: unknown) {
            if (err instanceof Error) {
                if (err.name === "AbortError") return;
            }
            const errorWithStatus = err as { status?: number };
            if (errorWithStatus.status === 404) {
                setNotFound(true);
            }
        } finally {
            setLoading(false);
        }
    }, [username]);

    useEffect(() => {
        const controller = new AbortController();
        loadUserData(controller.signal);
        return () => controller.abort();
    }, [loadUserData]);

    return (
        <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
            <SiteHeader selectedKey="home" />
            <Content style={{ padding: "40px 80px" }}>
                {loading ? (
                    <div style={{ textAlign: 'center', marginTop: '50px' }}>
                        <Spin size="large" description="Loading profile..." />
                    </div>
                ) : notFound ? (
                    <div style={{ textAlign: 'center', marginTop: '50px' }}>
                        <h1>404 - User Not Found</h1>
                        <p>The user with username {username} does not exist.</p>
                    </div>
                ) : user ? (
                    <ProfileView user={user} isSelf={user.isSelf ?? false} onRefresh={loadUserData}/>
                ) : null}
            </Content>
        </Layout>
    );
}

export default function ProfilePage({
    params,
}: {
    params: Promise<{ username: string }>
}) {
    const { username } = use(params);
    return(
        <UsersProvider>
            <ProfilePageContent username={username}/>
        </UsersProvider>
    )
}