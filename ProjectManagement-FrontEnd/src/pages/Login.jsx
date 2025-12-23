import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { API } from "../services/api";
import { useDispatch } from "react-redux";
import { setUser } from "../features/userSlice";

export default function Login() {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("MEMBER"); // 默认 MEMBER
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      // 🔹 传 email, password, role 给后端
      const res = await API.post("/user/login", { email, password, role });

      console.log("Login response:", res.data); // 调试用

      // 保存登录信息到本地
      // localStorage.setItem("userName", res.data.userName || "");
      // localStorage.setItem("role", res.data.role || role);

      // use redux to store user info
      dispatch(
        setUser({
          userId: res.data.userId,
          userName: res.data.userName,
          email: res.data.email,
          role: res.data.role || role,
        })
      );

      // 根据 role 跳转页面
      navigate('/app');

    } catch (err) {
      console.error("Login error:", err);
      setError(err.response?.data?.message || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center p-4 bg-white">
      <form
        className="border border-gray-200 p-8 rounded-lg w-full max-w-sm shadow-sm"
        onSubmit={handleSubmit}
      >
        <h2 className="text-2xl font-light text-gray-800 mb-8 text-center tracking-tight">
          Login
        </h2>

        {error && <p className="text-red-500 mb-4 text-center">{error}</p>}

        <div className="space-y-5">
          {/* Email */}
          <div>
            <input
              type="email"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
              required
            />
          </div>

          {/* Password */}
          <div>
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
              required
            />
          </div>

          {/* Role 下拉 */}
          <div>
            <select
              value={role}
              onChange={(e) => setRole(e.target.value)}
              className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
            >
              <option value="MEMBER">Member</option>
              <option value="LEADER">Leader</option>
              <option value="ADMIN">Admin</option>
            </select>
          </div>

          {/* 登录按钮 */}
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-[#2373ff] hover:bg-blue-600 text-white py-3 rounded font-medium mt-6 transition-colors"
          >
            {loading ? "Signing in..." : "Login"}
          </button>
        </div>

        <div className="mt-8 text-center">
          <p className="text-gray-500 text-sm">
            Don't have an account?{" "}
            <Link
              to="/register"
              className="text-gray-700 hover:text-black font-medium border-b border-transparent hover:border-gray-700 transition-all"
            >
              Register
            </Link>
          </p>
        </div>
      </form>
    </div>
  );
}
