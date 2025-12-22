import { Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout";
import { Toaster } from "react-hot-toast";
import Dashboard from "./pages/Dashboard";
import Projects from "./pages/Projects";
import Team from "./pages/Team";
import ProjectDetails from "./pages/ProjectDetails";
import TaskDetails from "./pages/TaskDetails";
import Login from "./pages/Login";
import Register from "./pages/Register";
import ProtectedRoute from "./components/ProtectedRoute";

const App = () => {
  return (
    <>
      <Toaster />
      <Routes>
        {/* 公共页面 */}
        <Route path="/" element={<Login />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        {/* 登录后主应用 */}
        <Route
          path="/app"
          element={
            <ProtectedRoute allowedRoles={["MEMBER", "MANAGER", "ADMIN"]}>
              <Layout />
            </ProtectedRoute>
          }
        >
          {/* 所有人都能进 */}
          <Route index element={<Dashboard />} />
          <Route path="projects" element={<Projects />} />
          <Route path="projectsDetail" element={<ProjectDetails />} />
          <Route path="taskDetails" element={<TaskDetails />} />

          {/* 只有 MANAGER / ADMIN 能进 */}
          <Route
            path="team"
            element={
              <ProtectedRoute allowedRoles={["MANAGER", "ADMIN"]}>
                <Team />
              </ProtectedRoute>
            }
          />
        </Route>
      </Routes>
    </>
  );
};

export default App;
