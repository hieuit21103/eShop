import * as React from "react";
import { Outlet } from "react-router";
import {
  LayoutDashboard,
  Package,
  FileText,
  Users,
  Settings,
  ChartNoAxesCombined,
} from "lucide-react";
import Sidebar, { type NavItem } from "../components/sidebar";
import Topbar from "../components/topbar";

const NAV: NavItem[] = [
  { to: "/admin/dashboard", label: "Dashboard", icon: <LayoutDashboard className="w-5 h-5" />, end: true },
  { to: "/admin/products", label: "Products", icon: <Package className="w-5 h-5" /> },
  { to: "/admin/orders", label: "Orders", icon: <FileText className="w-5 h-5" /> },
  { to: "/admin/users", label: "Users", icon: <Users className="w-5 h-5" /> },
  { to: "/admin/analysis", label: "Analysis", icon: <ChartNoAxesCombined className="w-5 h-5" /> },
  { to: "/admin/settings", label: "Settings", icon: <Settings className="w-5 h-5" /> },
];

export default function AdminLayout() {
  const [open, setOpen] = React.useState(false);

  React.useEffect(() => {
    const onKey = (e: KeyboardEvent) => e.key === "Escape" && setOpen(false);
    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, []);

  // Only lock body scroll when the mobile drawer is open
  React.useEffect(() => {
    document.body.classList.toggle("overflow-hidden", open);
  }, [open]);

  return (
    <div
      className={`
        h-dvh overflow-hidden bg-gray-200 text-black dark:bg-gray-950 dark:text-white
        lg:grid lg:grid-cols-[240px_1fr] lg:grid-rows-[64px_1fr]
        flex flex-col
      `}
    >
      {/* Desktop sidebar */}
      <div className="hidden lg:block lg:row-span-2">
        <Sidebar
          items={NAV}
          brand={
            <div className="text-lg font-extrabold text-transparent bg-clip-text bg-white flex justify-center align-middle">
              Shop Admin
            </div>
          }
          className="h-dvh"
        />
      </div>

      {/* Topbar (row 1 in grid / header in mobile) */}
      <Topbar
        onOpenSidebar={() => setOpen(true)}
        sidebarOpen={open}
        title="Welcome Back, Admin"
        user={{ name: "Jane Doe", email: "jane@shop.io", avatarUrl: "/avatar.png" }}
      />

      {/* Main content (row 2 in grid / flex-1 in mobile) — scrollable only here */}
      <main
        className={`
          min-h-0 overflow-y-auto
          px-4 py-3 lg:py-4
          lg:col-start-2 lg:row-start-2
          flex-1
        `}
      >
        <Outlet />
      </main>

      {/* Mobile slide-over sidebar */}
      {open && (
        <div id="mobile-sidebar" role="dialog" aria-modal="true" className="fixed inset-0 z-50 lg:hidden">
          <div className="absolute inset-0 bg-black/40" onClick={() => setOpen(false)} />
          <div className="absolute inset-y-0 left-0 w-72 max-w-[85%] translate-x-0 transition-transform duration-200">
            <Sidebar
              items={NAV}
              brand={
                <div className="text-lg font-extrabold text-transparent bg-clip-text bg-white flex justify-center align-middle">
                  Shop Admin
                </div>
              }
              className="h-full"
            />
          </div>
        </div>
      )}
    </div>
  );
}