import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { API } from "../services/api";

export default function Register() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    password: "",
    role: "Contributor",
  });
  const [error, setError] = useState("");

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      await API.post("/user/register", formData);
      alert("Registration successful");
      navigate("/login");
    } catch (err) {
      setError(err.response?.data?.message || "Registration failed");
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
          <div>
            <input
              type="text"
              name="name"
              placeholder="Username"
              value={formData.name}
              onChange={handleChange}
              className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
              required
            />
          </div>

          <div>
            <input
              type="email"
              name="email"
              placeholder="Email"
              value={formData.email}
              onChange={handleChange}
              className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
              required
            />
          </div>

          <div>
            <input
              type="password"
              name="password"
              placeholder="Password"
              value={formData.password}
              onChange={handleChange}
              className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent placeholder-gray-400 transition-colors"
              required
            />
          </div>

          <div>
            <select
              name="role"
              value={formData.role}
              onChange={handleChange}
              className="w-full px-3 py-2.5 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-transparent text-gray-700 appearance-none cursor-pointer transition-colors"
            >
              <option value="Admin">Admin</option>
              <option value="ProjectManager">Project Manager</option>
              <option value="Contributor">Contributor</option>
            </select>
          </div>

        <button
            type="submit"
            className="w-full bg-[#2373ff] hover:bg-blue-600 text-white py-3 rounded font-medium mt-6 transition-colors"
            >
            Register
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
