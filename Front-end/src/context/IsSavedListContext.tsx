"use client";

import { createContext, useContext } from "react";

export const IsSavedListContext = createContext(false);

export const useIsSavedList = () => useContext(IsSavedListContext);
