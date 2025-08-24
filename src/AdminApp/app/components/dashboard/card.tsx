// components/card.tsx
import * as React from "react";
import { MessageSquare } from "lucide-react";

export type CardProps = {
  title: string;
  value: string | number;
  BadgeIcon?: React.ComponentType<React.SVGProps<SVGSVGElement>>;
  className?: string;
};

export default function Card({
  title,
  value,
  BadgeIcon = MessageSquare,
  className = "",
}: CardProps) {
  return (
    <div
      className={[
        "rounded-2xl border border-gray-200 bg-white p-4 shadow-sm",
        "dark:border-gray-800 dark:bg-gray-900",
        className,
      ].join(" ")}
      role="group"
      aria-label={title}
    >
      {/* Header */}
      <div className="flex items-start justify-between">
        <span className="text-xs font-semibold tracking-widest text-gray-500 dark:text-gray-400">
          {title}
        </span>

        <span
          className="inline-flex h-8 w-8 items-center justify-center rounded-full bg-gray-200 dark:bg-white"
          aria-hidden="true"
        >
          <span className="inline-flex h-7 w-7 items-center justify-center rounded-full bg-gradient-to-r from-blue-500 to-purple-500">
            <BadgeIcon className="h-4 w-4 text-white" />
          </span>
        </span>
      </div>

      {/* Value */}
      <div className="mt-3 text-3xl font-semibold text-gray-900 dark:text-white">
        {value}
      </div>
    </div>
  );
}