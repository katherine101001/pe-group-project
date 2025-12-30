import { useEffect, useState } from "react";
import { Plus, Save, X } from "lucide-react";
import { format } from "date-fns";
import AddProjectMember from "./AddProjectMember";
import { getAllUsersComplete } from "../services/Team/team.api";
import { updateProject } from "../services/Project/ProjectAPI";

export default function ProjectSettings({ project }) {
    const [formData, setFormData] = useState({
        name: "",
        description: "",
        status: "PLANNING",
        priority: "MEDIUM",
        start_date: null,
        end_date: null,
        progress: 0,
    });

    const [allUsersMap, setAllUsersMap] = useState({});
    const [teamLeadId, setTeamLeadId] = useState("");
    const [teamMembers, setTeamMembers] = useState([]);
    const [availableLeaders, setAvailableLeaders] = useState([]);
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);

    // Confirmation dialog state
    const [isConfirmOpen, setIsConfirmOpen] = useState(false);
    const [memberToRemove, setMemberToRemove] = useState(null);

    // Initialize project form and users
    useEffect(() => {
        if (!project) return;

        setFormData({
            name: project.title || "",
            description: project.description || "",
            status: project.status || "PLANNING",
            priority: project.priority || "MEDIUM",
            start_date: project.startDate ? new Date(project.startDate) : null,
            end_date: project.endDate ? new Date(project.endDate) : null,
            progress: project.progress || 0,
        });

        const fetchUsers = async () => {
            try {
                const response = await getAllUsersComplete(); // Axios response
                const users = response.data;

                const map = {};
                users.forEach(u => (map[u.id] = u));
                setAllUsersMap(map);

                // Map leader
                const lead = users.find(u => u.email === project.teamLeadEmail);
                setTeamLeadId(lead?.id || "");

                // Map members
                const members = users
                    .filter(u => project.teamMemberEmails.includes(u.email))
                    .map(u => u.id);
                setTeamMembers(members);

                // Available leaders
                setAvailableLeaders(users.filter(u => u.role === "Leader"));
            } catch (err) {
                console.error("Failed to fetch users:", err);
            }
        };

        fetchUsers();
    }, [project]);

    // Remove member with confirmation
    const handleRemoveMember = (id) => {
        setMemberToRemove(id);
        setIsConfirmOpen(true);
    };

    const confirmRemoveMember = () => {
        setTeamMembers(teamMembers.filter(m => m !== memberToRemove));
        if (teamLeadId === memberToRemove) setTeamLeadId("");
        setMemberToRemove(null);
        setIsConfirmOpen(false);
    };

    const handleSubmit = async e => {
        e.preventDefault();
        if (!project) return;

        setIsSubmitting(true);
        try {
            const payload = {
                title: formData.name,
                description: formData.description,
                status: formData.status,
                priority: formData.priority,
                startDate: formData.start_date ? new Date(formData.start_date).toISOString() : null,
                endDate: formData.end_date ? new Date(formData.end_date).toISOString() : null,
                progress: formData.progress,
                teamLeadId: teamLeadId || null,
                teamMemberIds: teamMembers,
            };

            console.log("PUT payload:", payload);
            await updateProject(project.id, payload);
            alert("Project updated successfully!");
        } catch (err) {
            console.error("Failed to update project:", err);
            alert("Failed to update project.");
        } finally {
            setIsSubmitting(false);
        }
    };

    const inputClasses = "w-full px-3 py-2 rounded mt-2 border text-sm dark:bg-zinc-900 border-zinc-300 dark:border-zinc-700 text-zinc-900 dark:text-zinc-300";
    const cardClasses = "rounded-lg border p-6 dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border-zinc-300 dark:border-zinc-800";
    const labelClasses = "text-sm text-zinc-600 dark:text-zinc-400";

    if (!project || Object.keys(allUsersMap).length === 0) return <div>Loading...</div>;

    return (
        <div className="grid lg:grid-cols-2 gap-8">
            {/* Project Details */}
            <div className={cardClasses}>
                <h2 className="text-lg font-medium text-zinc-900 dark:text-zinc-300 mb-4">Project Details</h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    {/* Project Name */}
                    <div className="space-y-2">
                        <label className={labelClasses}>Project Name</label>
                        <input
                            value={formData.name}
                            onChange={e => setFormData({ ...formData, name: e.target.value })}
                            className={inputClasses}
                            required
                        />
                    </div>

                    {/* Description */}
                    <div className="space-y-2">
                        <label className={labelClasses}>Description</label>
                        <textarea
                            value={formData.description}
                            onChange={e => setFormData({ ...formData, description: e.target.value })}
                            className={`${inputClasses} h-24`}
                        />
                    </div>

                    {/* Status & Priority */}
                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className={labelClasses}>Status</label>
                            <select
                                value={formData.status}
                                onChange={e => setFormData({ ...formData, status: e.target.value })}
                                className={inputClasses}
                            >
                                <option value="PLANNING">Planning</option>
                                <option value="ACTIVE">Active</option>
                                <option value="ON_HOLD">On Hold</option>
                                <option value="COMPLETED">Completed</option>
                                <option value="CANCELLED">Cancelled</option>
                            </select>
                        </div>
                        <div className="space-y-2">
                            <label className={labelClasses}>Priority</label>
                            <select
                                value={formData.priority}
                                onChange={e => setFormData({ ...formData, priority: e.target.value })}
                                className={inputClasses}
                            >
                                <option value="LOW">Low</option>
                                <option value="MEDIUM">Medium</option>
                                <option value="HIGH">High</option>
                            </select>
                        </div>
                    </div>

                    {/* Timeline */}
                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className={labelClasses}>Start Date</label>
                            <input
                                type="date"
                                value={formData.start_date ? format(new Date(formData.start_date), "yyyy-MM-dd") : ""}
                                onChange={e =>
                                    setFormData({ ...formData, start_date: e.target.value ? new Date(e.target.value) : null })
                                }
                                className={inputClasses}
                            />
                        </div>
                        <div className="space-y-2">
                            <label className={labelClasses}>End Date</label>
                            <input
                                type="date"
                                value={formData.end_date ? format(new Date(formData.end_date), "yyyy-MM-dd") : ""}
                                onChange={e =>
                                    setFormData({ ...formData, end_date: e.target.value ? new Date(e.target.value) : null })
                                }
                                className={inputClasses}
                            />
                        </div>
                    </div>

                    {/* Progress */}
                    <div className="space-y-2">
                        <label className={labelClasses}>Progress: {formData.progress}%</label>
                        <input
                            type="range"
                            min="0"
                            max="100"
                            step="5"
                            value={formData.progress}
                            onChange={e => setFormData({ ...formData, progress: Number(e.target.value) })}
                            className="w-full accent-blue-500 dark:accent-blue-400"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="ml-auto flex items-center text-sm justify-center gap-2 bg-gradient-to-br from-blue-500 to-blue-600 text-white px-4 py-2 rounded"
                    >
                        <Save className="size-4" /> {isSubmitting ? "Saving..." : "Save Changes"}
                    </button>
                </form>
            </div>

            {/* Team Lead & Members */}
            <div className="space-y-6">
                <div className={cardClasses}>
                    <h2 className="text-lg font-medium text-zinc-900 dark:text-zinc-300 mb-2">Team Lead</h2>
                    {/* <select
                        value={teamLeadId}
                        onChange={e => setTeamLeadId(e.target.value)}
                        className={inputClasses}
                        required
                    >
                        <option value="">Select Team Lead</option>
                        {teamLeadId && allUsersMap[teamLeadId] && (
                            <option value={teamLeadId}>
                                {allUsersMap[teamLeadId].name} ({allUsersMap[teamLeadId].email})
                            </option>
                        )}
                        {availableLeaders.map(l => (
                            <option key={l.id} value={l.id}>
                                {l.name} ({l.email})
                            </option>
                        ))}
                    </select> */}
                    <div className="relative">
                        <select
                            className={`${inputClasses} pr-8 appearance-none`}
                            value={teamLeadId}
                            onChange={e => setTeamLeadId(e.target.value)}
                        >
                            <option value="">Select Team Lead</option>
                            {availableLeaders.map(l => (
                                <option key={l.id} value={l.id}>
                                    {l.name} ({l.email})
                                </option>
                            ))}
                        </select>

                        {/* Custom arrow using Tailwind */}
                        <div className="pointer-events-none absolute inset-y-0 right-2 flex items-center translate-y-1">
                            <svg
                                className="w-4 h-4 text-zinc-500"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 9l-7 7-7-7"></path>
                            </svg>
                        </div>
                    </div>


                    <div className="flex items-center justify-between mt-6 gap-4">
                        <h2 className="text-lg font-medium text-zinc-900 dark:text-zinc-300 mb-4">
                            Team Members <span className="text-sm text-zinc-600 dark:text-zinc-400">({teamMembers.length})</span>
                        </h2>
                        <button
                            type="button"
                            onClick={() => setIsDialogOpen(true)}
                            className="p-2 rounded-lg border border-zinc-300 dark:border-zinc-700 hover:bg-zinc-100 dark:hover:bg-zinc-800"
                        >
                            <Plus className="size-4 text-zinc-900 dark:text-zinc-300" />
                        </button>

                        <AddProjectMember
                            isDialogOpen={isDialogOpen}
                            setIsDialogOpen={setIsDialogOpen}
                            project={project}
                            selectedMemberIds={teamMembers}
                            setSelectedMemberIds={setTeamMembers}
                        />
                    </div>

                    <div className="space-y-2 mt-2 max-h-32 overflow-y-auto">
                        {teamMembers.map(id => {
                            const user = allUsersMap[id];
                            return (
                                <div key={id} className="flex items-center justify-between px-3 py-2 rounded dark:bg-zinc-800 text-sm text-zinc-900 dark:text-zinc-300">
                                    <span>{user ? `${user.name} (${user.email})` : id}</span>
                                    <button onClick={() => handleRemoveMember(id)} className="p-1 rounded hover:bg-zinc-700">
                                        <X className="w-4 h-4 text-red-500" />
                                    </button>
                                </div>
                            );
                        })}
                    </div>
                </div>
            </div>

            {/* Confirmation Dialog */}
            {isConfirmOpen && (
                <div className="fixed inset-0 bg-black/20 dark:bg-black/50 backdrop-blur flex items-center justify-center z-50">
                    <div className="bg-white dark:bg-zinc-950 rounded-xl shadow-lg w-full max-w-md p-6 border border-zinc-300 dark:border-zinc-700 text-zinc-900 dark:text-zinc-200">
                        <div className="flex justify-between items-center mb-4">
                            <h3 className="text-lg font-semibold">Remove Member</h3>
                            <button
                                onClick={() => setIsConfirmOpen(false)}
                                className="p-1 rounded hover:bg-zinc-200 dark:hover:bg-zinc-800"
                            >
                                <X className="w-5 h-5" />
                            </button>
                        </div>
                        <p className="mb-6 text-sm text-zinc-600 dark:text-zinc-400">
                            Are you sure you want to remove this member from the project?
                        </p>
                        <div className="flex justify-end gap-3">
                            <button
                                onClick={() => setIsConfirmOpen(false)}
                                className="px-4 py-2 rounded border border-zinc-300 dark:border-zinc-700 hover:bg-zinc-100 dark:hover:bg-zinc-800"
                            >
                                Cancel
                            </button>
                            <button
                                onClick={confirmRemoveMember}
                                className="px-4 py-2 rounded bg-red-600 text-white hover:bg-red-700"
                            >
                                Confirm
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
