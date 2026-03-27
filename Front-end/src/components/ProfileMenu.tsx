"use client";

import React, { useEffect, useState } from "react";
import { Avatar, Dropdown, MenuProps } from "antd";
import {
  UserOutlined,
  SettingOutlined,
  FileTextOutlined,
  LogoutOutlined,
} from "@ant-design/icons";

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
  const code = name.charCodeAt(0) + (name.charCodeAt(1) ?? 0);
  return AVATAR_COLORS[code % AVATAR_COLORS.length];
}
import { useRouter } from "next/navigation";
import { useLogin } from "@/context/LoginContext";

const ProfileMenu: React.FC = () => {
  const router = useRouter();
  const { logout } = useLogin();
  const items: MenuProps["items"] = [
    {
      key: "profile",
      icon: <UserOutlined />,
      label: "Profile",
    },
    {
      key: "applications",
      icon: <FileTextOutlined />,
      label: "My Applications",
    },
    {
      key: "settings",
      icon: <SettingOutlined />,
      label: "Settings",
    },
    {
      type: "divider",
    },
    {
      key: "logout",
      icon: <LogoutOutlined />,
      danger: true,
      label: "Logout",
    },
  ];
  
  const handleClick: MenuProps["onClick"] = async (e) => {
    switch (e.key) {
      case "profile": {
        const user = JSON.parse(localStorage.getItem("user") ?? "{}");
        if (user.username) router.push(`/profile/${user.username}`);
        break;
      }
      case "applications":
        router.push("/applications");
        break;
      case "settings":
        break;
      case "logout":
        await logout();
        router.push("/login");
        break;
    }
  };

  const [username, setUsername] = useState("");

  useEffect(() => {
    const user = JSON.parse(localStorage.getItem("user") ?? "{}");
    if (user.username) {
      // eslint-disable-next-line react-hooks/set-state-in-effect
      setUsername(user.username);
    }
  }, []);
  const initials = username ? username.slice(0, 2).toUpperCase() : "??";

  return (
    <Dropdown
      menu={{ items, onClick: handleClick }}
      placement="bottomRight"
      trigger={["click"]}
    >
      <Avatar
        size="large"
        style={{
          cursor: "pointer",
          backgroundColor: avatarColor(username || "?"),
          fontWeight: 700,
        }}
      >
        {initials || "?"}
      </Avatar>
    </Dropdown>
  );
};

export default ProfileMenu;