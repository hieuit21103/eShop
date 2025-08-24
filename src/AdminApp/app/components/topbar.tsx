import * as React from "react";
import ThemeToggle from "../components/theme-toggle"; // ⬅️ import your toggle

type UserInfo = {
  name: string;
  avatarUrl?: string;
  email?: string;
};

type TopbarProps = {
  onOpenSidebar: () => void;
  sidebarOpen?: boolean;
  title?: string;
  rightActions?: React.ReactNode;
  user?: UserInfo;
  className?: string;
};

export default function Topbar({
  onOpenSidebar,
  sidebarOpen = false,
  title = "Admin",
  rightActions,
  user = { name: "Admin User" },
  className = "",
}: TopbarProps) {
  const initials = getInitials(user.name);

  return (
    <header
      className={[
        "border border-gray-200 bg-white shadow-sm",
        "dark:border-gray-800 dark:bg-gray-900",
        className,
      ].join(" ")}
    >
      <div className="flex h-16 items-center justify-between px-3 lg:px-4">
        {/* Left cluster */}
        <div className="flex items-center gap-2">
          {/* Mobile: hamburger */}
          <button
            onClick={onOpenSidebar}
            className="
              lg:hidden inline-flex items-center justify-center rounded-lg
              border border-gray-200 bg-white p-2
              dark:border-gray-700 dark:bg-gray-950
              focus:outline-none focus-visible:ring-2 focus-visible:ring-purple-400 dark:focus-visible:ring-indigo-400
            "
            aria-label="Open sidebar"
            aria-haspopup="dialog"
            aria-expanded={sidebarOpen}
            aria-controls="mobile-sidebar"
          >
            <svg width="20" height="20" viewBox="0 0 24 24" className="fill-current">
              <path d="M3 6h18v2H3zM3 11h18v2H3zM3 16h18v2H3z" />
            </svg>
          </button>

          {/* Title visible on lg+ */}
          <span className="hidden lg:inline-flex items-center gap-2">
            <span className="inline-block size-2 rounded-full bg-blue-500 dark:bg-indigo-500" />
            <strong>{title}</strong>
          </span>
        </div>

        {/* Right cluster: actions + theme toggle + user */}
        <div className="flex items-center gap-3">
          {rightActions}

          <div className="flex items-center gap-3">
            {/* Theme toggle BEFORE user info */}
            <ThemeToggle />

            {/* Name + email */}
            <div className="flex flex-col leading-tight text-right">
              <span className="text-sm font-medium">{user.name}</span>
              {user.email && (
                <span className="text-xs text-gray-600 dark:text-gray-400">
                  {user.email}
                </span>
              )}
            </div>

            {/* Avatar */}
            <span
              className="
                inline-flex rounded-full p-[2px]
                bg-gradient-to-r from-blue-500 to-purple-600
                dark:from-indigo-500 dark:to-purple-500
              "
              aria-hidden
            >
              {user.avatarUrl ? (
                <img
                  src={user.avatarUrl}
                  alt={user.name}
                  className="size-9 rounded-full object-cover"
                />
              ) : (
                <span
                  className="
                    size-9 rounded-full bg-gray-200 text-gray-800
                    dark:bg-gray-800 dark:text-gray-100
                    inline-flex items-center justify-center text-sm font-semibold
                  "
                  aria-label={user.name}
                  title={user.name}
                >
                  {initials}
                </span>
              )}
            </span>
          </div>
        </div>
      </div>
    </header>
  );
}

function getInitials(name: string) {
  return (
    name
      .trim()
      .split(/\s+/)
      .slice(0, 2)
      .map((s) => s[0]?.toUpperCase() ?? "")
      .join("") || "AD"
  );
}