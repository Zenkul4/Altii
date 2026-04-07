import { createContext, useContext, useState, useCallback } from "react";
import type { ReactNode } from "react";

export type NotifType = "booking" | "payment" | "cancel" | "system";

export interface Notification {
  id:        number;
  type:      NotifType;
  title:     string;
  message:   string;
  read:      boolean;
  createdAt: Date;
}

interface NotificationContextType {
  notifications: Notification[];
  unreadCount:   number;
  addNotification: (type: NotifType, title: string, message: string) => void;
  markAllRead:   () => void;
  markRead:      (id: number) => void;
  clearAll:      () => void;
}

const NotificationContext = createContext<NotificationContextType | null>(null);

export const NotificationProvider = ({ children }: { children: ReactNode }) => {
  const [notifications, setNotifications] = useState<Notification[]>([]);

  const addNotification = useCallback((
    type: NotifType, title: string, message: string
  ) => {
    const notif: Notification = {
      id:        Date.now(),
      type,
      title,
      message,
      read:      false,
      createdAt: new Date(),
    };
    setNotifications(prev => [notif, ...prev].slice(0, 20)); // máx 20
  }, []);

  const markAllRead = useCallback(() =>
    setNotifications(prev => prev.map(n => ({ ...n, read: true }))), []);

  const markRead = useCallback((id: number) =>
    setNotifications(prev => prev.map(n => n.id === id ? { ...n, read: true } : n)), []);

  const clearAll = useCallback(() => setNotifications([]), []);

  const unreadCount = notifications.filter(n => !n.read).length;

  return (
    <NotificationContext.Provider value={{
      notifications, unreadCount,
      addNotification, markAllRead, markRead, clearAll,
    }}>
      {children}
    </NotificationContext.Provider>
  );
};

export const useNotifications = () => {
  const ctx = useContext(NotificationContext);
  if (!ctx) throw new Error("useNotifications must be used within NotificationProvider");
  return ctx;
};