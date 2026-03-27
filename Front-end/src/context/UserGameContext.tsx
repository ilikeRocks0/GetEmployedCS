"use client";

import { createContext, useContext } from "react";
import type { User } from "@/types/User";
import { initUserGame, acceptUser, rejectUser } from "@/api/userGame";

const UserGameContext = createContext({
  initUserGame: initUserGame as () => Promise<User | null>,
  acceptUser: acceptUser as (username: string) => Promise<User | null>,
  rejectUser: rejectUser as (username: string) => Promise<User | null>,
});

export function UserGameProvider({ children }: { children: React.ReactNode }) {
  return (
    <UserGameContext.Provider value={{ initUserGame, acceptUser, rejectUser }}>
      {children}
    </UserGameContext.Provider>
  );
}

export function useUserGame() {
  return useContext(UserGameContext);
}
