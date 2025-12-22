// ProtectedRoute.jsx
import { Navigate } from "react-router-dom";

export default function ProtectedRoute({ children, allowedRoles }) {
  const role = localStorage.getItem("role"); // 登录时存的 role

  if (!allowedRoles.includes(role)) {
    return <Navigate to="/app" />; // 无权限就回到默认页面
  }

  return children;
}
