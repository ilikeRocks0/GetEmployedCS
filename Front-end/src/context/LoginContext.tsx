"use client";

import { createContext, useContext } from "react";
import { login, logout } from "@/stub/loginStub";

const LoginContext = createContext({ login, logout });

export function LoginProvider({ children }: { children: React.ReactNode }) {
  return (
    <LoginContext.Provider value={{ login, logout }}>
      {children}
    </LoginContext.Provider>
  );
}

export function useLogin() {
  return useContext(LoginContext);
}
