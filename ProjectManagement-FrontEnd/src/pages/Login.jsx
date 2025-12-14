// // import { useState } from "react";
// // import { useNavigate, Link } from "react-router-dom";
// // import { API } from "../services/api";

// // export default function Login() {
// //   const navigate = useNavigate();
// //   const [email, setEmail] = useState("");
// //   const [password, setPassword] = useState("");
// //   const [loading, setLoading] = useState(false);
// //   const [error, setError] = useState("");

// //   const handleSubmit = async (e) => {
// //     e.preventDefault();
// //     setLoading(true);
// //     setError("");

// //     try {
// //       const res = await API.post("/user/login", { email, password });


// //       localStorage.setItem("role", res.data.role);
// //       localStorage.setItem("userName", res.data.userName);

// //       navigate("/app"); 
// //     } catch (err) {
// //       setError(err.response?.data?.message || "Login failed");
// //     } finally {
// //       setLoading(false);
// //     }
// //   };

// //   return (
// //     <div className="min-h-screen bg-white flex items-center justify-center p-4">
// //       <form
// //         className="border border-gray-200 p-8 rounded-lg w-full max-w-sm shadow-sm"
// //         onSubmit={handleSubmit}
// //       >
// //         <h2 className="text-2xl font-light text-gray-800 mb-8 text-center tracking-tight">
// //           Login
// //         </h2>

// //         {error && <p className="text-red-500 mb-4 text-center">{error}</p>}

// //         <div className="space-y-5">
// //           <div>
// //             <input
// //               type="email"
// //               name="email"
// //               placeholder="Email"
// //               value={formData.email}
// //               onChange={handleChange}
// //               className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
// //               required
// //             />
// //           </div>

// //           <div>
// //             <input
// //               type="password"
// //               name="password"
// //               placeholder="Password"
// //               value={formData.password}
// //               onChange={handleChange}
// //               className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
// //               required
// //             />
// //           </div>

// //           <button
// //             type="submit"
// //             disabled={loading}
// //             className="w-full bg-black hover:bg-gray-800 text-white py-3 rounded font-medium mt-6 transition-colors"
// //           >
// //             {loading ? (
// //               <span className="flex items-center justify-center">
// //                 <svg
// //                   className="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
// //                   xmlns="http://www.w3.org/2000/svg"
// //                   fill="none"
// //                   viewBox="0 0 24 24"
// //                 >
// //                   <circle
// //                     className="opacity-25"
// //                     cx="12"
// //                     cy="12"
// //                     r="10"
// //                     stroke="currentColor"
// //                     strokeWidth="4"
// //                   ></circle>
// //                   <path
// //                     className="opacity-75"
// //                     fill="currentColor"
// //                     d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
// //                   ></path>
// //                 </svg>
// //                 Signing in...
// //               </span>
// //             ) : (
// //               "Login"
// //             )}
// //           </button>
// //         </div>

// //         <div className="mt-8 text-center">
// //           <p className="text-gray-500 text-sm">
// //             Don't have an account?{" "}
// //             <Link
// //               to="/register"
// //               className="text-gray-700 hover:text-black font-medium border-b border-transparent hover:border-gray-700 transition-all"
// //             >
// //               Register
// //             </Link>
// //           </p>
// //         </div>
// //       </form>
// //     </div>
// //   );
// // }


// import { useState } from "react";
// import { useNavigate, Link } from "react-router-dom";
// import { API } from "../services/api";

// export default function Login() {
//   const navigate = useNavigate();
//   const [email, setEmail] = useState("");
//   const [password, setPassword] = useState("");
//   const [error, setError] = useState("");

//   const handleSubmit = async (e) => {
//     e.preventDefault();
//     setError("");

//     try {
//       const res = await API.post("/user/login", { email, password });
//       localStorage.setItem("userName", res.data.userName);
//       localStorage.setItem("role", res.data.role);
//       navigate("/app"); // 登录成功跳 Dashboard
//     } catch (err) {
//       setError(err.response?.data?.message || "Login failed");
//     }
//   };

//   return (
//     <form onSubmit={handleSubmit}>
//       <input value={email} onChange={e => setEmail(e.target.value)} placeholder="Email" />
//       <input value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" />
//       <button type="submit">Login</button>
//       {error && <p>{error}</p>}
//     </form>
//   );
// }
import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { API } from "../services/api";

export default function Login() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("MEMBER"); // 默认选 MEMBER
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      // 调用后端登录接口
      const res = await API.post("/user/login", { email, password, role });

      console.log("Login response:", res.data); // 🔹 调试用

      // 保存登录信息到本地
      localStorage.setItem("userName", res.data.userName);
      localStorage.setItem("role", res.data.role);

      navigate("/app"); // 登录成功跳转
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
              <option value="MANAGER">Manager</option>
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
