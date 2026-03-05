"use client";

import { createContext, useContext } from "react";
import { registerUser } from "@/api/users";

const UsersContext = createContext(registerUser);

export function UsersProvider({ children }: { children: React.ReactNode }) {
  return (
    <UsersContext.Provider value={registerUser}>
      {children}
    </UsersContext.Provider>
  );
}

export function useUser(){
  return useContext(UsersContext);
}
