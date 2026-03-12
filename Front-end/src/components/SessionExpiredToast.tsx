"use client";

import { useEffect } from "react";
import { notification } from "antd";

export default function SessionExpiredToast() {
  useEffect(() => {
    if (sessionStorage.getItem("session-expired")) {
      sessionStorage.removeItem("session-expired");
      notification.warning({
        message: "Session Expired",
        description: "Your session has expired. Please log in again.",
        placement: "topRight",
      });
    }
  }, []);

  return null;
}
