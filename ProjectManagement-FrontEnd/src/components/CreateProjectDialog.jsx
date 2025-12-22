import { useState } from "react";
import { XIcon } from "lucide-react";

import { createProject } from "../services/Project/ProjectAPI";

const CreateProjectDialog = ({ isDialogOpen, setIsDialogOpen }) => {
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    status: "PLANNING",
    priority: "MEDIUM",
    start_date: "",
    end_date: "",
    team_members: [],
    team_lead: "",
    progress: 0,
  });

  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);

    try {
      const payload = {
        title: formData.name,
        description: formData.description,
        status: formData.status,
        priority: formData.priority,
        startDate: formData.start_date || null,
        endDate: formData.end_date || null,
        teamMemberEmails: formData.team_members,
        teamLeadEmail: formData.team_lead || null,
      };

      console.log("Submitting payload:", payload);

      await createProject(payload);

      alert("Project created successfully!");
      setIsDialogOpen(false);

      setFormData({
        name: "",
        description: "",
        status: "PLANNING",
        priority: "MEDIUM",
        start_date: "",
        end_date: "",
        team_members: [],
        team_lead: "",
        progress: 0,
      });
    } catch (error) {
      console.error("Failed to create project:", error);
      alert("Failed to create project.");
    } finally {
      setIsSubmitting(false);
    }
  };

  const removeTeamMember = (email) => {
    setFormData((prev) => ({
      ...prev,
      team_members: prev.team_members.filter((m) => m !== email),
    }));
  };

  if (!isDialogOpen) return null;

  return (
    <div className="fixed inset-0 bg-black/20 dark:bg-black/60 backdrop-blur flex items-center justify-center z-50">
      <div className="bg-white dark:bg-zinc-950 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 w-full max-w-lg relative">
        <button
          className="absolute top-3 right-3 text-zinc-500 hover:text-zinc-700"
          onClick={() => setIsDialogOpen(false)}
        >
          <XIcon className="size-5" />
        </button>

        <h2 className="text-xl font-medium mb-4">Create New Project</h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          {/* Project Name */}
          <div>
            <label className="block text-sm mb-1">Project Name</label>
            <input
              type="text"
              value={formData.name}
              onChange={(e) =>
                setFormData({ ...formData, name: e.target.value })
              }
              className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
              required
            />
          </div>

          {/* Description */}
          <div>
            <label className="block text-sm mb-1">Description</label>
            <textarea
              value={formData.description}
              onChange={(e) =>
                setFormData({ ...formData, description: e.target.value })
              }
              className="w-full px-3 py-2 rounded border dark:bg-zinc-900 h-20"
            />
          </div>

          {/* Status & Priority */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm mb-1">Status</label>
              <select
                value={formData.status}
                onChange={(e) =>
                  setFormData({ ...formData, status: e.target.value })
                }
                className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
              >
                <option value="PLANNING">Planning</option>
                <option value="ACTIVE">Active</option>
                <option value="COMPLETED">Completed</option>
                <option value="ON_HOLD">On Hold</option>
                <option value="CANCELLED">Cancelled</option>
              </select>
            </div>

            <div>
              <label className="block text-sm mb-1">Priority</label>
              <select
                value={formData.priority}
                onChange={(e) =>
                  setFormData({ ...formData, priority: e.target.value })
                }
                className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
              >
                <option value="LOW">Low</option>
                <option value="MEDIUM">Medium</option>
                <option value="HIGH">High</option>
              </select>
            </div>
          </div>

          {/* Dates */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm mb-1">Start Date</label>
              <input
                type="date"
                value={formData.start_date}
                onChange={(e) =>
                  setFormData({ ...formData, start_date: e.target.value })
                }
                className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
              />
            </div>

            <div>
              <label className="block text-sm mb-1">End Date</label>
              <input
                type="date"
                value={formData.end_date}
                onChange={(e) =>
                  setFormData({ ...formData, end_date: e.target.value })
                }
                min={formData.start_date || undefined}
                className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
              />
            </div>
          </div>

          {/* Project Lead */}
          <div>
            <label className="block text-sm mb-1">Project Lead (Email)</label>
            <input
              type="email"
              value={formData.team_lead}
              onChange={(e) =>
                setFormData({ ...formData, team_lead: e.target.value })
              }
              placeholder="lead@example.com"
              className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
            />
          </div>

          {/* Team Members */}
          <div>
            <label className="block text-sm mb-1">Team Members</label>
            <input
              type="email"
              placeholder="Press Enter to add email"
              className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
              onKeyDown={(e) => {
                if (e.key === "Enter" && e.target.value) {
                  e.preventDefault();
                  if (!formData.team_members.includes(e.target.value)) {
                    setFormData((prev) => ({
                      ...prev,
                      team_members: [...prev.team_members, e.target.value],
                    }));
                    e.target.value = "";
                  }
                }
              }}
            />

            {formData.team_members.length > 0 && (
              <div className="flex flex-wrap gap-2 mt-2">
                {formData.team_members.map((email) => (
                  <div
                    key={email}
                    className="flex items-center gap-1 bg-blue-200/50 px-2 py-1 rounded text-sm"
                  >
                    {email}
                    <button
                      type="button"
                      onClick={() => removeTeamMember(email)}
                    >
                      <XIcon className="w-3 h-3" />
                    </button>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* Footer */}
          <div className="flex justify-end gap-3 pt-2">
            <button
              type="button"
              onClick={() => setIsDialogOpen(false)}
              className="px-4 py-2 rounded border"
            >
              Cancel
            </button>
            <button
              disabled={isSubmitting}
              className="px-4 py-2 rounded bg-blue-600 text-white"
            >
              {isSubmitting ? "Creating..." : "Create Project"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateProjectDialog;
