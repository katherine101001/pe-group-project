import { useState, useEffect } from "react";
import { Plus, Search, FolderOpen } from "lucide-react"; // 移除了 Archive
import ProjectCard from "../components/ProjectCard";
import CreateProjectDialog from "../components/CreateProjectDialog";
import { getAllProjects } from "../services/Project/ProjectAPI";
import { useSelector } from "react-redux";

export default function Projects() {
  const [projects, setProjects] = useState([]);
  const [filteredProjects, setFilteredProjects] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [filters, setFilters] = useState({ status: "ALL", priority: "ALL" });

  const [refreshKey, setRefreshKey] = useState(0);

  const { role, userId } = useSelector((state) => state.user);

  // ===================== 获取项目 =====================
  const fetchProjects = async () => {
    try {
      const data = await getAllProjects();

      // 只显示未归档项目
      let visibleProjects = data.filter((p) => !p.isArchived);

      if (role !== "ADMIN") {
        visibleProjects = visibleProjects.filter((project) => {
          const memberIds = project.memberIds || [];
          return String(project.leaderId) === String(userId) ||
            memberIds.some((id) => String(id) === String(userId));
        });
      }

      const sorted = visibleProjects.sort(
        (a, b) => new Date(b.startDate) - new Date(a.startDate)
      );

      setProjects(sorted);
    } catch (error) {
      console.error("Failed to fetch projects:", error);
      setProjects([]);
    }
  };

  useEffect(() => {
    if (!role) return;
    fetchProjects();
  }, [role, userId]);

  // ===================== 搜索与过滤 =====================
  useEffect(() => {
    let filtered = [...projects];

    if (searchTerm) {
      filtered = filtered.filter(
        (project) =>
          project.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
          project.description?.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    if (filters.status !== "ALL") {
      filtered = filtered.filter((project) => project.status === filters.status);
    }

    if (filters.priority !== "ALL") {
      filtered = filtered.filter((project) => project.priority === filters.priority);
    }

    setFilteredProjects(filtered);
  }, [projects, searchTerm, filters]);

  // ===================== 创建项目回调 =====================
  const handleProjectCreated = () => {
    fetchProjects();
    setRefreshKey(prev => prev + 1); // triggers refresh in sidebar
    setIsDialogOpen(false);
  };


  // ===================== 页面 =====================
  return (
    <div className="space-y-6 max-w-6xl mx-auto">
      {/* Header */}
      <div className="flex flex-col lg:flex-row justify-between items-start lg:items-center gap-6">
        <div>
          <h1 className="text-xl sm:text-2xl font-semibold text-gray-900 dark:text-white mb-1">
            Projects
          </h1>
          <p className="text-gray-500 dark:text-zinc-400 text-sm">
            Manage and track your projects
          </p>
        </div>

        {/* 新建项目按钮 */}
        {["ADMIN", "LEADER"].includes(role) && (
          <button
            onClick={() => setIsDialogOpen(true)}
            className="flex items-center px-5 py-2 text-sm rounded bg-gradient-to-br from-blue-500 to-blue-600 text-white hover:opacity-90 transition"
          >
            <Plus className="size-4 mr-2" /> New Project
          </button>
        )}

        <CreateProjectDialog
          isDialogOpen={isDialogOpen}
          setIsDialogOpen={setIsDialogOpen}
          onProjectCreated={handleProjectCreated}
        />
      </div>

      {/* 搜索和过滤 */}
      <div className="flex flex-col md:flex-row gap-4">
        <div className="relative w-full max-w-sm">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 dark:text-zinc-400 w-4 h-4" />
          <input
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search projects..."
            className="w-full pl-10 text-sm pr-4 py-2 rounded-lg border border-gray-300 dark:border-zinc-700 text-gray-900 dark:text-white placeholder-gray-400 dark:placeholder-zinc-400 focus:border-blue-500 outline-none"
          />
        </div>

        <select
          value={filters.status}
          onChange={(e) => setFilters({ ...filters, status: e.target.value })}
          className="px-3 py-2 rounded-lg border border-gray-300 dark:border-zinc-700 text-gray-900 dark:text-white text-sm"
        >
          <option value="ALL">All Status</option>
          <option value="ACTIVE">Active</option>
          <option value="PLANNING">Planning</option>
          <option value="COMPLETED">Completed</option>
          <option value="ON_HOLD">On Hold</option>
          <option value="CANCELLED">Cancelled</option>
        </select>

        <select
          value={filters.priority}
          onChange={(e) => setFilters({ ...filters, priority: e.target.value })}
          className="px-3 py-2 rounded-lg border border-gray-300 dark:border-zinc-700 text-gray-900 dark:text-white text-sm"
        >
          <option value="ALL">All Priority</option>
          <option value="HIGH">High</option>
          <option value="MEDIUM">Medium</option>
          <option value="LOW">Low</option>
        </select>
      </div>

      {/* 项目列表 */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredProjects.length === 0 ? (
          <div className="col-span-full text-center py-16">
            <div className="w-24 h-24 mx-auto mb-6 bg-gray-200 dark:bg-zinc-800 rounded-full flex items-center justify-center">
              <FolderOpen className="w-12 h-12 text-gray-400 dark:text-zinc-500" />
            </div>
            <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-1">
              No projects found
            </h3>
            <p className="text-gray-500 dark:text-zinc-400 mb-6 text-sm">
              Create your first project to get started
            </p>
          </div>
        ) : (
          filteredProjects.map((project) => (
            <ProjectCard key={project.id} project={project} />
          ))
        )}
      </div>
    </div>
  );
}
