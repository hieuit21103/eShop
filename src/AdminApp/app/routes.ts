import {
  type RouteConfig,
  index,
  layout,
  route,
} from "@react-router/dev/routes";

export default [
  layout("./layouts/admin-layout.tsx", [
    index("./routes/home.tsx"),
    route("admin/dashboard", "./routes/dashboard.tsx"),
    route("admin/products", "./routes/products.tsx"),
    route("admin/users", "./routes/users.tsx"),
    route("admin/orders", "./routes/orders.tsx"),
    route("admin/analysis", "./routes/analysis.tsx"),
    route("admin/settings", "./routes/settings.tsx"),
    route("logout", "./routes/logout.tsx"),
  ]),
  route("/login", "./routes/auth/login.tsx"),
  route("/register", "./routes/auth/register.tsx"),
  route("/forgot-password", "./routes/auth/forgot-password.tsx"),
  route("/reset-password", "./routes/auth/reset-password.tsx"),
  route("/confirm-email", "./routes/auth/confirm-email.tsx"),
  // pattern ^           ^ module file
] satisfies RouteConfig;
