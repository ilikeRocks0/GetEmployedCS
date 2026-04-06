"use client";

import { useLayoutEffect, useMemo } from "react";
import { useRouter, usePathname } from "next/navigation";

const PUBLIC_ROUTES: ReadonlySet<string> = new Set (["/login", "/signup", "/verify"]);

export default function AuthGuard({ children }: Readonly<{ children: React.ReactNode }>) {
  const router = useRouter();
  const pathname = usePathname();
  
  const isAuthorized = useMemo(() => {
    if (globalThis.window === undefined) return false;

    const getCookie = (name: string) => {
      const value = `; ${document.cookie}`;
      const parts = value.split(`; ${name}=`);
      if (parts.length === 2) return parts.pop()?.split(';').shift();
    };

    const user = localStorage.getItem("user");
    const hasAuthFlag = getCookie("is_logged_in") === "true";
    const isPublic = PUBLIC_ROUTES.has(pathname);

    return isPublic || !!(user || hasAuthFlag);
  }, [pathname]);

  useLayoutEffect(() => {
    if (globalThis.window === undefined) return;

    const user = localStorage.getItem("user");
    const isPublic = PUBLIC_ROUTES.has(pathname);

    if (!user && !isPublic) {
      router.replace("/login");
    } else if (user && isPublic) {
      router.replace("/");
    }
  }, [pathname, router]);

  const isPublic = PUBLIC_ROUTES.has(pathname);
  if (!isAuthorized && !isPublic) {
    return <div style={{ minHeight: "100vh", background: "#f0f2f5" }} />; 
  }

  return <>{children}</>;
}