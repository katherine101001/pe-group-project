import { useState } from "react";
import { Calendar as CalendarIcon } from "lucide-react";
import { format } from "date-fns";
import { createTask } from "../services/ProjectTask/ProjectTaskAPI";

export default function CreateTaskDialog({
  showCreateTask,
  setShowCreateTask,
  projectId,
  members = [],
}) {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    type: "TASK",
    status: "TO_DO",
    priority: "MEDIUM",
    assignToUserId: "", // string | ""
    dueDate: "",
  });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);

    const payload = {
      projectId,
      assignToUserId: formData.assignToUserId || null, // null = Unassigned
      title: formData.title,
      description: formData.description,
      type: formData.type,
      status: formData.status,
      priority: formData.priority,
      dueDate: formData.dueDate || null,
    };

    console.log("Create task payload:", payload);

    try {
      await createTask(payload);
      setShowCreateTask(false);
      setFormData({
        title: "",
        description: "",
        type: "TASK",
        status: "TO_DO",
        priority: "MEDIUM",
        assignToUserId: "",
        dueDate: "",
      });
    } catch (err) {
      console.error("Failed to create task:", err);
      alert("Failed to create task");
    } finally {
      setIsSubmitting(false);
    }
  };

  if (!showCreateTask) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/20 dark:bg-black/60 backdrop-blur">
      <div className="bg-white dark:bg-zinc-950 border border-zinc-300 dark:border-zinc-800 rounded-lg shadow-lg w-full max-w-md p-6">
        <h2 className="text-xl font-bold mb-4 text-zinc-900 dark:text-white">
          Create New Task
        </h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          {/* Title */}
          <div>
            <label className="text-sm font-medium">Title</label>
            <input
              value={formData.title}
              onChange={(e) =>
                setFormData({ ...formData, title: e.target.value })
              }
              required
              className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
            />
          </div>

          {/* Description */}
          <div>
            <label className="text-sm font-medium">Description</label>
            <textarea
              value={formData.description}
              onChange={(e) =>
                setFormData({ ...formData, description: e.target.value })
              }
              className="w-full mt-1 px-3 py-2 rounded border h-24 dark:bg-zinc-900"
            />
          </div>

          {/* Type & Priority */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="text-sm font-medium">Type</label>
              <select
                value={formData.type}
                onChange={(e) =>
                  setFormData({ ...formData, type: e.target.value })
                }
                className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
              >
                <option value="TASK">Task</option>
                <option value="BUG">Bug</option>
                <option value="FEATURE">Feature</option>
                <option value="IMPROVEMENT">Improvement</option>
                <option value="OTHER">Other</option>
              </select>
            </div>

            <div>
              <label className="text-sm font-medium">Priority</label>
              <select
                value={formData.priority}
                onChange={(e) =>
                  setFormData({ ...formData, priority: e.target.value })
                }
                className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
              >
                <option value="LOW">Low</option>
                <option value="MEDIUM">Medium</option>
                <option value="HIGH">High</option>
              </select>
            </div>
          </div>

          {/* Assignee & Status */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="text-sm font-medium">Assignee</label>
              <select
                value={formData.assignToUserId}
                onChange={(e) =>
                  setFormData({
                    ...formData,
                    assignToUserId: e.target.value,
                  })
                }
                className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
              >
                <option value="">Unassigned</option>
                {members.map((m) => (
                  <option key={m.id} value={m.id}>
                    {m.email}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="text-sm font-medium">Status</label>
              <select
                value={formData.status}
                onChange={(e) =>
                  setFormData({ ...formData, status: e.target.value })
                }
                className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
              >
                <option value="TO_DO">To Do</option>
                <option value="IN_PROGRESS">In Progress</option>
                <option value="DONE">Done</option>
              </select>
            </div>
          </div>

          {/* Due Date */}
          <div>
            <label className="text-sm font-medium">Due Date</label>
            <div className="flex items-center gap-2">
              <CalendarIcon className="w-5 h-5 text-zinc-400" />
              <input
                type="date"
                value={formData.dueDate}
                onChange={(e) =>
                  setFormData({ ...formData, dueDate: e.target.value })
                }
                className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
              />
            </div>
            {formData.dueDate && (
              <p className="text-xs text-zinc-500 mt-1">
                {format(new Date(formData.dueDate), "PPP")}
              </p>
            )}
          </div>

          {/* Footer */}
          <div className="flex justify-end gap-2 pt-4">
            <button
              type="button"
              onClick={() => setShowCreateTask(false)}
              className="px-4 py-2 border rounded"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="px-4 py-2 bg-blue-600 text-white rounded"
            >
              {isSubmitting ? "Creating..." : "Create Task"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
