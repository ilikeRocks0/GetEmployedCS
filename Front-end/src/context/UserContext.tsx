"use client";

import { createContext, useContext, useMemo } from "react";
import { fetchUser, registerUser } from "@/api/users";

const UsersContext = createContext({registerUser, fetchUser});

export function UsersProvider({ children }: { children: React.ReactNode }) {
  const value = useMemo(() => ({ registerUser, fetchUser }), []);

  return (
    <UsersContext.Provider value={value}>
      {children}
    </UsersContext.Provider>
  );
}

export function useUser(){
  return useContext(UsersContext);
}
