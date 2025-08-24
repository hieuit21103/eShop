import { Coins, FileText, MessagesSquare, Users } from "lucide-react";
import Card from "~/components/dashboard/card";
import PurchaseChart from "~/components/dashboard/chart";

type TopProduct = {
  id: string;
  name: string;
  orders: number;
  revenue: number;
  aov: number;
};

const TOP_PRODUCTS: TopProduct[] = [
  { id: "p1", name: "Air Max 90", orders: 124, revenue: 24500000, aov: 197580 },
  { id: "p2", name: "Leather Backpack", orders: 98, revenue: 18750000, aov: 191327 },
  { id: "p3", name: "Wireless Earbuds", orders: 156, revenue: 31200000, aov: 200000 },
  { id: "p4", name: "Denim Jacket", orders: 76, revenue: 12900000, aov: 169737 },
];

export default function Dashboard() {
  const fmt = (v: number) =>
    new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
      maximumFractionDigits: 0,
    }).format(v);

  return (
    <div className="flex flex-col h-full overflow-y-auto gap-6 p-2">
      {/* Stats */}
      <div className="grid gap-4 md:grid-cols-4">
        <Card className="flex-1" title="MESSAGES" value={8} BadgeIcon={MessagesSquare} />
        <Card className="flex-1" title="ORDERS" value={23} BadgeIcon={FileText} />
        <Card className="flex-1" title="USERS" value={152} BadgeIcon={Users} />
        <Card className="flex-1" title="REVENUE" value={fmt(31200000)} BadgeIcon={Coins} />
      </div>

      {/* Charts */}
      <div className="grid gap-4 md:grid-cols-2">
        <PurchaseChart className="h-full w-full" />
        <PurchaseChart className="h-full w-full" />
      </div>

      {/* Top sale products table */}
      <div>
        <div className="rounded-xl border border-gray-200 dark:border-gray-800 overflow-hidden">
          <div className="px-4 py-3 bg-gray-50 dark:bg-gray-800/50">
            <h2 className="text-sm font-semibold">Top Sale Products</h2>
          </div>
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead className="text-left bg-gray-50 dark:bg-gray-900/40">
                <tr className="[&>th]:px-4 [&>th]:py-2 text-gray-600 dark:text-gray-300">
                  <th className="w-[50%]">Product</th>
                  <th>Orders</th>
                  <th>Revenue</th>
                  <th>AOV</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100 dark:divide-gray-800">
                {TOP_PRODUCTS.map((p) => (
                  <tr key={p.id} className="[&>td]:px-4 [&>td]:py-2">
                    <td className="font-medium">{p.name}</td>
                    <td>{p.orders}</td>
                    <td>{fmt(p.revenue)}</td>
                    <td>{fmt(p.aov)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <div className="px-4 py-2 text-xs text-gray-500 dark:text-gray-400 bg-gray-50/60 dark:bg-gray-900/40">
            Showing top 5 by revenue
          </div>
        </div>
      </div>
    </div>
  );
}