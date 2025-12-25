import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { API } from "../services/api";

export default function Register() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    password: "",       // invitation password
    newPassword: "",    // new password
    confirmPassword: "",// confirm new password
  });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    // 验证新密码是否和确认密码一致
    if (formData.newPassword !== formData.confirmPassword) {
      setError("New password and confirm password do not match.");
      return;
    }

    setLoading(true);

    try {
      // 调用后端 API
      await API.post("/user/register", {
        email: formData.email,
        password: formData.password,       // invitation password
        newPassword: formData.newPassword, // new password
        confirmPassword: formData.confirmPassword,
        name: formData.name,               // update name
      });

      alert("Registration successful! You can now login.");
      navigate("/login");
    } catch (err) {
      setError(err.response?.data?.message || "Registration failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-white flex items-center justify-center p-4">
      <form
        className="border border-gray-200 p-8 rounded-lg w-full max-w-sm shadow-sm"
        onSubmit={handleSubmit}
      >
        <h2 className="text-2xl font-light text-gray-800 mb-8 text-center tracking-tight">
          Register
        </h2>

        {error && <p className="text-red-500 mb-4 text-center">{error}</p>}

        <div className="space-y-5">
          {/* Username */}
          <input
            type="text"
            name="name"
            placeholder="Username"
            value={formData.name}
            onChange={handleChange}
            className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
            required
          />

          {/* Email */}
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={formData.email}
            onChange={handleChange}
            className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
            required
          />

          {/* Invitation password */}
          <input
            type="password"
            name="password"
            placeholder="Invitation Password"
            value={formData.password}
            onChange={handleChange}
            className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
            required
          />

          {/* New password */}
          <input
            type="password"
            name="newPassword"
            placeholder="New Password"
            value={formData.newPassword}
            onChange={handleChange}
            className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
            required
          />

          {/* Confirm new password */}
          <input
            type="password"
            name="confirmPassword"
            placeholder="Confirm New Password"
            value={formData.confirmPassword}
            onChange={handleChange}
            className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
            required
          />

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-[#2373ff] hover:bg-blue-600 text-white py-3 rounded font-medium mt-6 transition-colors"
          >
            {loading ? "Registering..." : "Register"}
          </button>
        </div>

        <div className="mt-8 text-center">
          <p className="text-gray-500 text-sm">
            Already registered?{" "}
            <Link
              to="/login"
              className="text-gray-700 hover:text-black font-medium border-b border-transparent hover:border-gray-700 transition-all"
            >
              Sign in
            </Link>
          </p>
        </div>
      </form>
    </div>
  );
}
