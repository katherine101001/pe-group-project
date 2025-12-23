import { useState, useEffect, useMemo } from "react";
import { format } from "date-fns";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import { getTasksByProjectId } from "../services/ProjectTask/ProjectTaskAPI";
import { Bug, Zap, Square, GitCommit, MessageSquare, XIcon } from "lucide-react";

const typeIcons = {
  BUG: { icon: Bug, color: "text-red-600 dark:text-red-400" },
  FEATURE: { icon: Zap, color: "text-blue-600 dark:text-blue-400" },
  TASK: { icon: Square, color: "text-green-600 dark:text-green-400" },
  IMPROVEMENT: { icon: GitCommit, color: "text-purple-600 dark:text-purple-400" },
  OTHER: { icon: MessageSquare, color: "text-amber-600 dark:text-amber-400" },
};

const priorityTexts = {
  LOW: { background: "bg-red-100 dark:bg-red-950", prioritycolor: "text-red-600 dark:text-red-400" },
  MEDIUM: { background: "bg-blue-100 dark:bg-blue-950", prioritycolor: "text-blue-600 dark:text-blue-400" },
  HIGH: { background: "bg-emerald-100 dark:bg-emerald-950", prioritycolor: "text-emerald-600 dark:text-emerald-400" },
};

const ProjectTasks = ({ projectId }) => {
  const navigate = useNavigate();
  const [tasks, setTasks] = useState([]);
  const [filters, setFilters] = useState({ status: "", type: "", priority: "", assignee: "" });

  useEffect(() => {
    if (!projectId) return;
    const fetchTasks = async () => {
      try {
        const data = await getTasksByProjectId(projectId);
        setTasks(data);
      } catch (err) {
        console.error(err);
        toast.error("Failed to fetch tasks");
      }
    };
    fetchTasks();
  }, [projectId]);

  const assigneeList = useMemo(
    () => Array.from(new Set(tasks.map(t => t.assignee?.name).filter(Boolean))),
    [tasks]
  );

  const filteredTasks = useMemo(
    () =>
      tasks.filter(task => {
        const { status, type, priority, assignee } = filters;
        return (!status || task.status === status) &&
               (!type || task.type === type) &&
               (!priority || task.priority === priority) &&
               (!assignee || task.assignee?.name === assignee);
      }),
    [tasks, filters]
  );

  const handleFilterChange = e => {
    const { name, value } = e.target;
    setFilters(prev => ({ ...prev, [name]: value }));
  };

  return (
    <div>
      {/* Filters */}
      <div className="flex flex-wrap gap-4 mb-4">
        {["status", "type", "priority", "assignee"].map(name => {
          const options = {
            status: [
              { label: "All Statuses", value: "" },
              { label: "To Do", value: "TO_DO" },
              { label: "In Progress", value: "IN_PROGRESS" },
              { label: "Done", value: "DONE" }
            ],
            type: [
              { label: "All Types", value: "" },
              { label: "Task", value: "TASK" },
              { label: "Bug", value: "BUG" },
              { label: "Feature", value: "FEATURE" },
              { label: "Improvement", value: "IMPROVEMENT" },
              { label: "Other", value: "OTHER" }
            ],
            priority: [
              { label: "All Priorities", value: "" },
              { label: "Low", value: "LOW" },
              { label: "Medium", value: "MEDIUM" },
              { label: "High", value: "HIGH" }
            ],
            assignee: [
              { label: "All Assignees", value: "" },
              ...assigneeList.map(n => ({ label: n, value: n }))
            ]
          };
          return (
            <select
              key={name}
              name={name}
              onChange={handleFilterChange}
              value={filters[name]}
              className="border not-dark:bg-white border-zinc-300 dark:border-zinc-800 outline-none px-3 py-1 rounded text-sm text-zinc-900 dark:text-zinc-200"
            >
              {options[name].map((opt, idx) => (
                <option key={idx} value={opt.value}>{opt.label}</option>
              ))}
            </select>
          );
        })}
        {(filters.status || filters.type || filters.priority || filters.assignee) && (
          <button
            onClick={() => setFilters({ status: "", type: "", priority: "", assignee: "" })}
            className="px-3 py-1 flex items-center gap-2 rounded bg-gradient-to-br from-purple-400 to-purple-500 text-zinc-100 dark:text-zinc-200 text-sm"
          >
            <XIcon className="size-3" /> Reset
          </button>
        )}
      </div>

      {/* Tasks Table */}
      <div className="overflow-auto rounded-lg lg:border border-zinc-300 dark:border-zinc-800">
        <table className="min-w-full text-sm text-left not-dark:bg-white text-zinc-900 dark:text-zinc-300">
          <thead className="text-xs uppercase dark:bg-zinc-800/70 text-zinc-500 dark:text-zinc-400">
            <tr>
              <th className="pl-2 pr-1">#</th>
              <th className="px-4 pl-0 py-3">Title</th>
              <th className="px-4 py-3">Type</th>
              <th className="px-4 py-3">Priority</th>
              <th className="px-4 py-3">Status</th>
              <th className="px-4 py-3">Assignee</th>
              <th className="px-4 py-3">Due Date</th>
            </tr>
          </thead>
          <tbody>
            {filteredTasks.length > 0 ? (
              filteredTasks.map((task, idx) => {
                const { icon: Icon, color } = typeIcons[task.type] || {};
                const { background, prioritycolor } = priorityTexts[task.priority] || {};
                return (
                  <tr
                    key={task.id}
                    className="border-t border-zinc-300 dark:border-zinc-800 group hover:bg-zinc-50 dark:hover:bg-zinc-700 transition-all cursor-pointer"
                    onClick={() => navigate(`/app/taskDetails?projectId=${task.projectId}&taskId=${task.id}`)}
                  >
                    <td className="pl-2 pr-1">{idx + 1}</td>
                    <td className="px-4 pl-0 py-2">{task.title}</td>
                    <td className="px-4 py-2 flex items-center gap-2">{Icon && <Icon className={`size-4 ${color}`} />}<span className={`uppercase text-xs ${color}`}>{task.type}</span></td>
                    <td className="px-4 py-2"><span className={`text-xs px-2 py-1 rounded ${background} ${prioritycolor}`}>{task.priority}</span></td>
                    <td className="px-4 py-2">{task.status}</td>
                    <td className="px-4 py-2">{task.assignee?.name || "Unassigned"}</td>
                    <td className="px-4 py-2">{task.due_date ? format(new Date(task.due_date), "dd MMM yyyy") : "-"}</td>
                  </tr>
                );
              })
            ) : (
              <tr>
                <td colSpan="7" className="text-center text-zinc-500 dark:text-zinc-400 py-6">
                  No tasks found.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ProjectTasks;
