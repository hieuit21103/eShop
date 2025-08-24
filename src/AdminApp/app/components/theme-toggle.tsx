import { useEffect, useState } from "react";

export default function ThemeToggle() {
  const [isDark, setIsDark] = useState(false);

  // Initialize from saved choice or system preference
  useEffect(() => {
    if (typeof window === "undefined") return;
    const saved = localStorage.getItem("theme"); // "dark" | "light" | null
    const systemDark = window.matchMedia?.("(prefers-color-scheme: dark)")?.matches;
    const shouldDark = saved ? saved === "dark" : !!systemDark;
    document.documentElement.classList.toggle("dark", shouldDark);
    setIsDark(shouldDark);
  }, []);

  const toggle = () => {
    if (typeof window === "undefined") return;
    const next = !isDark;
    document.documentElement.classList.toggle("dark", next);
    localStorage.setItem("theme", next ? "dark" : "light");
    setIsDark(next);
  };

  return (
    <button
      onClick={toggle}
      aria-pressed={isDark}
      title="Toggle theme"
      className={`
        relative inline-flex items-center select-none
        h-8 w-16 rounded-full p-1
        transition-[background,box-shadow] duration-300 ease-in-out
        shadow-[0_6px_20px_rgba(99,102,241,0.35)]
        bg-gradient-to-r from-blue-500 to-purple-600
        dark:from-indigo-500 dark:to-purple-500
        focus:outline-none focus:ring-2 focus:ring-purple-400 focus:ring-offset-2 dark:focus:ring-offset-gray-950
      `}
    >
      {/* Knob with dynamic icon */}
      <span
        className={`
          pointer-events-none inline-flex h-6 w-6 items-center justify-center
          rounded-full bg-white shadow-md ring-1 ring-black/10
          transform transition-transform duration-300 ease-in-out
          ${isDark ? "translate-x-0" : "translate-x-8"}
        `}
      >
        {isDark ? (
          // Moon when dark
          <svg
            width="16"
            height="16"
            viewBox="0 0 24 24"
            className="text-gray-800 fill-current"
          >
            <path d="M21 12.79A9 9 0 1 1 11.21 3a7 7 0 1 0 9.79 9.79Z" />
          </svg>
        ) : (
          // Sun when light
          <svg
            width="16"
            height="16"
            viewBox="0 0 24 24"
            className="text-yellow-500 fill-current"
          >
            <path d="M12 18a6 6 0 1 0 0-12 6 6 0 0 0 0 12Zm0 4a1 1 0 0 1-1-1v-1.2a1 1 0 1 1 2 0V21a1 1 0 0 1-1 1Zm0-19a1 1 0 0 1 1 1v1.2a1 1 0 1 1-2 0V4a1 1 0 0 1 1-1ZM3 13a1 1 0 0 1-1-1 1 1 0 0 1 1-1h1.2a1 1 0 1 1 0 2H3Zm16.8 0a1 1 0 1 1 0-2H21a1 1 0 1 1 0 2h-1.2ZM5.05 19.36a1 1 0 0 1 0-1.41l.85-.85a1 1 0 1 1 1.41 1.41l-.85.85a1 1 0 0 1-1.41 0Zm12.64-12.64a1 1 0 0 1 0 1.41l-.85.85a1 1 0 1 1-1.41-1.41l.85-.85a1 1 0 0 1 1.41 0Zm-12.64 0a1 1 0 0 1 1.41 0l.85.85a1 1 0 1 1-1.41 1.41l-.85-.85a1 1 0 0 1 0-1.41Zm12.64 12.64a1 1 0 0 1-1.41 0l-.85-.85a1 1 0 1 1 1.41-1.41l.85.85a1 1 0 0 1 0 1.41Z" />
          </svg>
        )}
      </span>

      <span className="sr-only">
        {isDark ? "Switch to light" : "Switch to dark"}
      </span>
    </button>
  );
}