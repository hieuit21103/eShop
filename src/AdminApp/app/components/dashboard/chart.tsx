import * as React from "react";
import {
  LineChart,
  Line,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  ReferenceArea,
  ReferenceDot,
} from "recharts";

/**
 * Chart.tsx – Purchase Count line chart (Recharts)
 *
 * Features:
 * - Smooth line with custom tooltip
 * - Legend chip ("Purchase Count")
 * - Optional highlighted month band + marker dot
 * - Fully responsive and themed with Tailwind
 */

export type SeriesPoint = {
  /** X-axis label, e.g. "Aug", "Sep", "Oct" or specific date */
  x: string;
  /** value to plot (0 – 1.0 in this sample; pass any number) */
  y: number;
  /** Optional: month name for tooltip display when this point is highlighted */
  month?: string;
  /** Optional: display value for tooltip (if not provided, will format y) */
  displayValue?: number;
};

export type PurchaseChartProps = {
  data?: SeriesPoint[];
  /** Line label displayed at top-right */
  legendLabel?: string;
  /** Index of the point to highlight with a background band + orange dot */
  highlightIndex?: number | null;
  /** Prefix shown in tooltip for value, e.g. "$" */
  valuePrefix?: string;
  className?: string;
  /** Height of chart area */
  height?: number | string;
};

const defaultData: SeriesPoint[] = [
  { x: "Aug", y: 0.12 },
  { x: " ", y: 0.15 },
  { x: "  ", y: 0.13 },
  { x: "   ", y: 0.18 },
  { x: "    ", y: 0.11 },
  { x: "Sep", y: 0.36, month: "September", displayValue: 12738 },
  { x: " ", y: 0.26 },
  { x: "  ", y: 0.10 },
  { x: "   ", y: 0.58 },
  { x: "    ", y: 0.22 },
  { x: "     ", y: 0.02 },
  { x: "Oct", y: 0.34 },
  { x: "      ", y: 0.12 },
];

function formatCompact(n: number) {
  try {
    return new Intl.NumberFormat(undefined, {
      notation: "compact",
      maximumFractionDigits: 1,
    }).format(n);
  } catch {
    return String(n);
  }
}

const CustomTooltip: React.FC<{
  active?: boolean;
  payload?: any[];
  label?: string;
  valuePrefix?: string;
}> = ({ active, payload, valuePrefix }) => {
  if (!active || !payload || !payload.length) return null;
  const p = payload[0]?.payload as SeriesPoint | undefined;
  if (!p) return null;
  const value = p.displayValue ?? p.y;
  const valueText = typeof value === "number" && valuePrefix
    ? `${valuePrefix}${formatCompact(value)}`
    : typeof value === "number"
    ? formatCompact(value)
    : String(value);

  return (
    <div className="rounded-xl bg-white/95 dark:bg-gray-900/95 shadow-md ring-1 ring-black/5 px-3 py-2">
      <div className="text-sm font-semibold">{valueText}</div>
      {p.month ? (
        <div className="text-xs text-gray-500 dark:text-gray-400">{p.month}</div>
      ) : null}
    </div>
  );
};

const LegendChip: React.FC<{ label: string }> = ({ label }) => (
  <div className="flex items-center gap-2 absolute right-4 top-4 select-none">
    <span className="inline-block size-3 rounded-full bg-blue-600" />
    <span className="text-xs font-medium text-gray-700 dark:text-gray-300">{label}</span>
  </div>
);

export default function PurchaseChart({
  data = defaultData,
  legendLabel = "Purchase Count",
  highlightIndex = 5,
  valuePrefix = "$",
  className = "",
  height = "12rem", // default height (48 Tailwind units)
}: PurchaseChartProps) {
  // Build band coordinates for the highlighted point
  const hasHighlight =
    typeof highlightIndex === "number" && highlightIndex >= 0 && highlightIndex < data.length;

  // The x labels are strings; to create a band we approximate by using the label at index and index+1
  const x1 = hasHighlight ? data[highlightIndex].x : undefined;
  const x2 = hasHighlight ? data[Math.min(highlightIndex + 1, data.length - 1)].x : undefined;

  return (
    <div
      className={[
        "relative rounded-2xl border border-gray-200 bg-white shadow-sm p-4",
        "dark:border-gray-800 dark:bg-gray-900",
        className,
      ].join(" ")}
    >
      <div className="text-xs text-gray-400 mb-2">2021–2022</div>
      <LegendChip label={legendLabel} />

      <div style={{ height }} className="w-full"> {/* ensure box covers chart content */}
        <ResponsiveContainer>
          <LineChart data={data} margin={{ top: 8, right: 8, bottom: 8, left: 0 }}>
            {/* light grid */}
            <CartesianGrid strokeOpacity={0.25} vertical={false} />

            {/* X / Y axes */}
            <XAxis
              dataKey="x"
              axisLine={false}
              tickLine={false}
              tick={{ fontSize: 12, fill: "currentColor", opacity: 0.6 }}
              interval={Math.ceil(data.length / 6)}
            />
            <YAxis
              width={30}
              ticks={[0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6]}
              domain={[0, 0.6]}
              axisLine={false}
              tickLine={false}
              tick={{ fontSize: 12, fill: "currentColor", opacity: 0.6 }}
            />

            {/* Highlight band */}
            {hasHighlight && (
              <ReferenceArea x1={x1 as any} x2={x2 as any} fill="#3b82f6" fillOpacity={0.08} />
            )}

            <Tooltip cursor={false} content={<CustomTooltip valuePrefix={valuePrefix} />} />

            {/* The line */}
            <Line
              type="monotone"
              dataKey="y"
              stroke="#2563eb"
              strokeWidth={3}
              dot={false}
              activeDot={{ r: 5 }}
            />

            {/* Orange marker at highlight point */}
            {hasHighlight && (
              <ReferenceDot
                x={data[highlightIndex].x as any}
                y={data[highlightIndex].y}
                r={6}
                fill="#f59e0b"
                stroke="#fff"
                strokeWidth={2}
              />
            )}
          </LineChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}