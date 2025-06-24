import {
  type RouteConfig,
  route,
} from "@react-router/dev/routes";

export default [
  route("/login", "./routes/login.tsx"),
  route("/register", "./routes/register.tsx"),
  route("/forgot-password", "./routes/forgot-password.tsx"),
  route("/reset-password", "./routes/reset-password.tsx"),
  route("/confirm-email", "./routes/confirm-email.tsx"),
  // pattern ^           ^ module file
] satisfies RouteConfig;
