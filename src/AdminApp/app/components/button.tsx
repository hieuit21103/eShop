// components/ui/GradientButton.tsx
import * as React from "react";

type Props = React.ButtonHTMLAttributes<HTMLButtonElement> & {
  leadingIcon?: React.ReactNode;
  trailingIcon?: React.ReactNode;
};

export default function Button({
  children = "Click Me",
  className = "",
  leadingIcon,
  trailingIcon,
  ...props
}: Props) {
  return (
    <button
      {...props}
      className={`
        inline-flex items-center justify-center gap-2
        px-6 py-3 rounded-lg font-semibold text-white
        transition-all duration-300 ease-in-out
        hover:scale-[1.03] active:scale-95
        disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:scale-100

        /* Light theme gradient */
        bg-gradient-to-r from-blue-500 to-purple-600
        hover:from-purple-500 hover:to-blue-600

        /* Dark theme gradient */
        dark:from-indigo-500 dark:to-purple-500
        dark:hover:from-purple-600 dark:hover:to-indigo-600

        /* Focus rings for both themes */
        focus:outline-none focus:ring-2 focus:ring-purple-400 focus:ring-opacity-50
        dark:focus:ring-indigo-400

        shadow-sm hover:shadow-lg
        ${className}
      `}
    >
      {leadingIcon}
      <span>{children}</span>
      {trailingIcon}
    </button>
  );
}
