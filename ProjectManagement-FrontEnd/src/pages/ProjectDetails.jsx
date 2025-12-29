import { useState, useEffect } from "react";
import { useNavigate, useSearchParams, useOutletContext } from "react-router-dom";

import {
  ArrowLeftIcon,
  PlusIcon,
  SettingsIcon,
  BarChart3Icon,
  CalendarIcon,
  FileStackIcon,
  ZapIcon,
} from "lucide-react";
import ProjectAnalyticsWrapper from "../components/ProjectAnalyticsWrapper";
import ProjectSettings from "../components/ProjectSettings";
import CreateTaskDialog from "../components/CreateTaskDialog";
import ProjectCalendar from "../components/ProjectCalendar";
import ProjectTasks from "../components/ProjectTasks";
import { getProjectById } from "../services/Project/ProjectAPI";
import { useSelector } from "react-redux";

export default function ProjectDetail() {
  const [searchParams, setSearchParams] = useSearchParams();
  const tab = searchParams.get("tab");
  const id = searchParams.get("id");

  const { role, userId } = useSelector((state) => state.user);
  const navigate = useNavigate();

  const { handleRefresh } = useOutletContext();

  const [project, setProject] = useState(null);
  const [showCreateTask, setShowCreateTask] = useState(false);
  const [activeTab, setActiveTab] = useState(tab || "tasks");
  const [loading, setLoading] = useState(true);
  const [refreshKey, setRefreshKey] = useState(0);

  const statusColors = {
    PLANNING: "bg-zinc-200 text-zinc-900 dark:bg-zinc-600 dark:text-zinc-200",
    ACTIVE: "bg-emerald-200 text-emerald-900 dark:bg-emerald-500 dark:text-emerald-900",
    ON_HOLD: "bg-amber-200 text-amber-900 dark:bg-amber-500 dark:text-amber-900",
    COMPLETED: "bg-blue-200 text-blue-900 dark:bg-blue-500 dark:text-blue-900",
    CANCELLED: "bg-red-200 text-red-900 dark:bg-red-500 dark:text-red-900",
  };

  // Fetch project data
  useEffect(() => {
    if (!id) return;
    setLoading(true);
    getProjectById(id)
      .then((data) => {
        setProject({ ...data, isArchived: Boolean(data.isArchived) });
      })
      .catch((err) => {
        console.error("Failed to fetch project:", err);
        setProject(null);
      })
      .finally(() => setLoading(false));
  }, [id]);

  useEffect(() => {
    if (tab) setActiveTab(tab);
  }, [tab]);

  const handleTaskCreated = () => {
    handleRefresh();
    setRefreshKey(prev => prev + 1);
    setShowCreateTask(false);
  };

  const isArchived = project?.isArchived || false;
  const canManageProject = project
    ? role === "ADMIN" || (role === "LEADER")
    : false;

  // Prevent unauthorized access to Settings tab only when archived
  useEffect(() => {
    if (activeTab === "settings" && isArchived) {
      setActiveTab("tasks");
      setSearchParams({ id, tab: "tasks" });
    }
  }, [activeTab, isArchived, id, setSearchParams]);

  // Archive / Unarchive handlers
  const handleArchiveProject = async () => {
    if (!window.confirm("Do you want to archive this project?")) return;
    try {
      await fetch(`http://localhost:5272/api/projects/${id}/archive`, { method: "POST" });
      setProject({ ...project, isArchived: true });
    } catch (err) {
      console.error(err);
      alert(err.message || "Failed to archive project");
    }
  };

  const handleUnarchiveProject = async () => {
    if (!window.confirm("Do you want to unarchive this project?")) return;
    try {
      await fetch(`http://localhost:5272/api/projects/${id}/unarchive`, { method: "POST" });
      setProject({ ...project, isArchived: false });
    } catch (err) {
      console.error(err);
      alert(err.message || "Failed to unarchive project");
    }
  };

  if (loading) return <div className="p-6 text-center text-zinc-900 dark:text-zinc-200">Loading project...</div>;

  if (!project) {
    return (
      <div className="p-6 text-center text-zinc-900 dark:text-zinc-200">
        <p className="text-3xl md:text-5xl mt-40 mb-10">Project not found</p>
        <button
          onClick={() => navigate("/app/projects")}
          className="mt-4 px-4 py-2 rounded bg-zinc-200 text-zinc-900 hover:bg-zinc-300 dark:bg-zinc-700 dark:text-white dark:hover:bg-zinc-600"
        >
          Back to Projects
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-5 max-w-6xl mx-auto text-zinc-900 dark:text-white">
      {/* Header */}
      <div className="flex max-md:flex-col gap-4 flex-wrap items-start justify-between max-w-6xl">
        <div className="flex items-center gap-4">
          <button
            className="p-1 rounded hover:bg-zinc-200 dark:hover:bg-zinc-700 text-zinc-600 dark:text-zinc-400"
            onClick={() => navigate("/app/projects")}
          >
            <ArrowLeftIcon className="w-4 h-4" />
          </button>
          <div className="flex items-center gap-3">
            <h1 className="text-xl font-medium">{project.title}</h1>
            <span
              className={`px-2 py-1 rounded text-xs capitalize ${statusColors[project.status]}`}
            >
              {project.status.replace("_", " ")}
            </span>
          </div>
        </div>

        {/* Buttons */}
        <div className="flex gap-2">
          {/* Leader or Admin can add task if not archived */}
          {canManageProject && !isArchived && (
            <button
              onClick={() => setShowCreateTask(true)}
              className="flex items-center gap-2 px-5 py-2 text-sm rounded bg-gradient-to-br from-blue-500 to-blue-600 text-white"
            >
              <PlusIcon className="size-4" />
              New Task
            </button>
          )}

          {/* Leader or Admin can archive */}
          {canManageProject && !isArchived && (
            <button
              onClick={handleArchiveProject}
              className="flex items-center gap-2 px-4 py-2 text-sm rounded bg-red-500 text-white hover:bg-red-600"
            >
              Archive
            </button>
          )}

          {canManageProject && isArchived && (
            <button
              onClick={handleUnarchiveProject}
              className="flex items-center gap-2 px-4 py-2 text-sm rounded bg-green-500 text-white hover:bg-green-600"
            >
              Unarchive
            </button>
          )}
        </div>
      </div>

      {/* Info Cards */}
      <div className="grid grid-cols-2 sm:flex flex-wrap gap-6">
        {[
          { label: "Total Tasks", value: project.totalTasks || 0, color: "text-zinc-900 dark:text-white" },
          { label: "Complete", value: project.completedTasks || 0, color: "text-emerald-700 dark:text-emerald-400" },
          { label: "In Progress", value: project.inProgressTasks || 0, color: "text-amber-700 dark:text-amber-400" },
          { label: "Team Members", value: project.totalTeamMembers || 0, color: "text-blue-700 dark:text-blue-400" },
        ].map((card, idx) => (
          <div key={idx} className="dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border border-zinc-200 dark:border-zinc-800 flex justify-between sm:min-w-60 p-4 py-2.5 rounded">
            <div>
              <div className="text-sm text-zinc-600 dark:text-zinc-400">{card.label}</div>
              <div className={`text-2xl font-bold ${card.color}`}>{card.value}</div>
            </div>
            <ZapIcon className={`size-4 ${card.color}`} />
          </div>
        ))}
      </div>

      {/* Tabs */}
      <div className="inline-flex flex-wrap max-sm:grid grid-cols-3 gap-2 border border-zinc-200 dark:border-zinc-800 rounded overflow-hidden">
        {[
          { key: "tasks", label: "Tasks", icon: FileStackIcon },
          { key: "calendar", label: "Calendar", icon: CalendarIcon },
          { key: "analytics", label: "Analytics", icon: BarChart3Icon, hide: !canManageProject },
          { key: "settings", label: "Settings", icon: SettingsIcon, hide: isArchived || !canManageProject },
        ]
          .filter((tab) => !tab.hide)
          .map((tabItem) => (
            <button
              key={tabItem.key}
              onClick={() => { setActiveTab(tabItem.key); setSearchParams({ id, tab: tabItem.key }); }}
              className={`flex items-center gap-2 px-4 py-2 text-sm transition-all ${activeTab === tabItem.key ? "bg-zinc-100 dark:bg-zinc-800/80" : "hover:bg-zinc-50 dark:hover:bg-zinc-700"
                }`}
            >
              <tabItem.icon className="size-3.5" />
              {tabItem.label}
            </button>
          ))}
      </div>

      {/* Tab Content */}
      <div className="mt-6">
        <div style={{ display: activeTab === "tasks" ? "block" : "none" }}>
          <ProjectTasks projectId={id} refreshKey={refreshKey} />
        </div>
        <div style={{ display: activeTab === "calendar" ? "block" : "none" }}>
          <ProjectCalendar project={project} />
        </div>
        <div style={{ display: activeTab === "analytics" && canManageProject ? "block" : "none" }}>
          <ProjectAnalyticsWrapper project={project} />
        </div>
        <div style={{ display: activeTab === "settings" && canManageProject && !isArchived ? "block" : "none" }}>
          <ProjectSettings project={project} />
        </div>
      </div>

      {/* Create Task Modal */}
      {showCreateTask && !isArchived && (
        <CreateTaskDialog
          showCreateTask={showCreateTask}
          setShowCreateTask={setShowCreateTask}
          projectId={id}
          onTaskCreated={handleTaskCreated}
        />
      )}
    </div>
  );
}
