"use client";

import { createContext, useContext } from "react";
import { fetchUser, registerUser } from "@/api/users";

const UsersContext = createContext({registerUser, fetchUser});

export function UsersProvider({ children }: { children: React.ReactNode }) {
  return (
    <UsersContext.Provider value={{registerUser, fetchUser}}>
      {children}
    </UsersContext.Provider>
  );
}

export function useUser(){
  return useContext(UsersContext);
}
