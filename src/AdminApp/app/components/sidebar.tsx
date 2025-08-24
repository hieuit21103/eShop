import { NavLink } from "react-router";
import * as React from "react";
import { LogOut } from "lucide-react"; // optional icon

export type NavItem = {
  to: string;
  label: string;
  icon?: React.ReactNode;
  /** Use end when you want exact match, e.g. "/admin" vs "/admin/*" */
  end?: boolean;
};

type SidebarProps = {
  items: NavItem[];
  brand?: React.ReactNode;
  footer?: React.ReactNode; // e.g., <ThemeToggle />
  className?: string;
};

export default function Sidebar({
  items,
  brand,
  footer,
  className = "",
}: SidebarProps) {
  return (
    <aside
      role="navigation"
      aria-label="Sidebar"
      className={[
        "bg-gradient-to-br from-blue-800 to-purple-900 shadow-sm",
        "dark:border-gray-800 dark:bg-gradient-to-br dark:from-indigo-800 dark:to-purple-800",
        className,
      ].join(" ")}
    >
      <div className="pt-3 pb-3 h-full flex flex-col">
        {brand ? (
          <div className="mb-4">{brand}</div>
        ) : (
          <div className="mb-4 text-lg font-extrabold text-transparent bg-clip-text bg-white">
            Admin
          </div>
        )}

        {/* Top nav */}
        <nav className="space-y-1">
          {items.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              end={item.end}
              className={({ isActive }) =>
                [
                  "flex items-center gap-3 px-3 py-2",
                  isActive
                    ? "text-black bg-white"
                    : "text-white hover:text-gray-800 hover:bg-white/70 dark:hover:text-white dark:hover:bg-gray-800",
                ].join(" ")
              }
            >
              {item.icon && <span aria-hidden>{item.icon}</span>}
              <span>{item.label}</span>
            </NavLink>
          ))}
        </nav>

        {/* Push footer + logout to the bottom */}
        <div className="mt-auto space-y-2">
          {footer && <div className="pt-2">{footer}</div>}

          {/* Logout item */}
          <NavLink
            to="/logout"
            className="flex items-center gap-3 px-3 py-2 text-white hover:text-gray-800 hover:bg-white/70 dark:hover:text-white dark:hover:bg-gray-800"
          >
            <LogOut className="w-5 h-5" aria-hidden />
            <span>Logout</span>
          </NavLink>
        </div>
      </div>
    </aside>
  );
}
