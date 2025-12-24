// import { format } from "date-fns";
// import { Plus, Save } from "lucide-react";
// import { useEffect, useState } from "react";
// import AddProjectMember from "./AddProjectMember";
// import { useSelector } from "react-redux";

// export default function ProjectSettings({ project }) {
//     const [formData, setFormData] = useState({
//         name: "",
//         description: "",
//         status: "PLANNING",
//         priority: "MEDIUM",
//         start_date: null,
//         end_date: null,
//         progress: 0,
//     });

//     const [isDialogOpen, setIsDialogOpen] = useState(false);
//     const [isSubmitting, setIsSubmitting] = useState(false);
//     const { role, userId } = useSelector((state) => state.user);
//     const [selectedMemberIds, setSelectedMemberIds] = useState([]);
//     const [selectedTeamLeadId, setSelectedTeamLeadId] = useState(null);


//     // Update formData when project changes
//     useEffect(() => {
//         if (!project) return;

//         setFormData({
//             name: project.title || "",
//             description: project.description || "",
//             status: project.status || "PLANNING",
//             priority: project.priority || "MEDIUM",
//             start_date: project.startDate ? new Date(project.startDate) : null,
//             end_date: project.endDate ? new Date(project.endDate) : null,
//             progress: project.progress || 0,
//         });
//     }, [project]);

//     const handleSubmit = async (e) => {
//         e.preventDefault();
//         if (!project) return;

//         setIsSubmitting(true);

//         try {
//             const payload = {
//                 title: formData.name || null,
//                 description: formData.description || null,
//                 status: formData.status,
//                 priority: formData.priority,
//                 startDate: formData.start_date ? new Date(formData.start_date).toISOString() : null,
//                 endDate: formData.end_date ? new Date(formData.end_date).toISOString() : null,
//                 progress: formData.progress,
//                 teamMemberIds: selectedMemberIds,
//                 teamLeadId: selectedTeamLeadId,
//             };

//             console.log("PUT payload:", payload);
//             await updateProject(project.id, payload);

//             alert("Project updated successfully!");
//         } catch (error) {
//             console.error("Failed to update project:", error);
//             alert("Failed to update project.");
//         } finally {
//             setIsSubmitting(false);
//         }
//     };

//     const inputClasses = "w-full px-3 py-2 rounded mt-2 border text-sm dark:bg-zinc-900 border-zinc-300 dark:border-zinc-700 text-zinc-900 dark:text-zinc-300";
//     const cardClasses = "rounded-lg border p-6 not-dark:bg-white dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border-zinc-300 dark:border-zinc-800";
//     const labelClasses = "text-sm text-zinc-600 dark:text-zinc-400";

//     return (
//         <div className="grid lg:grid-cols-2 gap-8">
//             {/* Project Details */}
//             <div className={cardClasses}>
//                 <h2 className="text-lg font-medium text-zinc-900 dark:text-zinc-300 mb-4">Project Details</h2>
//                 <form onSubmit={handleSubmit} className="space-y-4">
//                     {/* Name */}
//                     <div className="space-y-2">
//                         <label className={labelClasses}>Project Name</label>
//                         <input
//                             value={formData.name}
//                             onChange={(e) => setFormData({ ...formData, name: e.target.value })}
//                             className={inputClasses}
//                             required
//                         />
//                     </div>

//                     {/* Description */}
//                     <div className="space-y-2">
//                         <label className={labelClasses}>Description</label>
//                         <textarea
//                             value={formData.description}
//                             onChange={(e) => setFormData({ ...formData, description: e.target.value })}
//                             className={`${inputClasses} h-24`}
//                         />
//                     </div>

//                     {/* Status & Priority */}
//                     <div className="grid grid-cols-2 gap-4">
//                         <div className="space-y-2">
//                             <label className={labelClasses}>Status</label>
//                             <select
//                                 value={formData.status}
//                                 onChange={(e) => setFormData({ ...formData, status: e.target.value })}
//                                 className={inputClasses}
//                             >
//                                 <option value="PLANNING">Planning</option>
//                                 <option value="ACTIVE">Active</option>
//                                 <option value="ON_HOLD">On Hold</option>
//                                 <option value="COMPLETED">Completed</option>
//                                 <option value="CANCELLED">Cancelled</option>
//                             </select>
//                         </div>

//                         <div className="space-y-2">
//                             <label className={labelClasses}>Priority</label>
//                             <select
//                                 value={formData.priority}
//                                 onChange={(e) => setFormData({ ...formData, priority: e.target.value })}
//                                 className={inputClasses}
//                             >
//                                 <option value="LOW">Low</option>
//                                 <option value="MEDIUM">Medium</option>
//                                 <option value="HIGH">High</option>
//                             </select>
//                         </div>
//                     </div>

//                     {/* Timeline */}
//                     <div className="space-y-4 grid grid-cols-2 gap-4">
//                         <div className="space-y-2">
//                             <label className={labelClasses}>Start Date</label>
//                             <input
//                                 type="date"
//                                 value={formData.start_date ? format(new Date(formData.start_date), "yyyy-MM-dd") : ""}
//                                 onChange={(e) => setFormData({ ...formData, start_date: e.target.value ? new Date(e.target.value) : null })}
//                                 className={inputClasses}
//                             />
//                         </div>
//                         <div className="space-y-2">
//                             <label className={labelClasses}>End Date</label>
//                             <input
//                                 type="date"
//                                 value={formData.end_date ? format(new Date(formData.end_date), "yyyy-MM-dd") : ""}
//                                 onChange={(e) => setFormData({ ...formData, end_date: e.target.value ? new Date(e.target.value) : null })}
//                                 className={inputClasses}
//                             />
//                         </div>
//                     </div>

//                     {/* Progress */}
//                     <div className="space-y-2">
//                         <label className={labelClasses}>Progress: {formData.progress}%</label>
//                         <input
//                             type="range"
//                             min="0"
//                             max="100"
//                             step="5"
//                             value={formData.progress}
//                             onChange={(e) => setFormData({ ...formData, progress: Number(e.target.value) })}
//                             className="w-full accent-blue-500 dark:accent-blue-400"
//                         />
//                     </div>

//                     {/* Save Button */}
//                     <button
//                         type="submit"
//                         disabled={isSubmitting}
//                         className="ml-auto flex items-center text-sm justify-center gap-2 bg-gradient-to-br from-blue-500 to-blue-600 text-white px-4 py-2 rounded"
//                     >
//                         <Save className="size-4" /> {isSubmitting ? "Saving..." : "Save Changes"}
//                     </button>
//                 </form>
//             </div>

//             {/* Team Members */}
//             <div className="space-y-6">
//                 <div className={cardClasses}>
//                     <div className="flex items-center justify-between gap-4">
//                         <h2 className="text-lg font-medium text-zinc-900 dark:text-zinc-300 mb-4">
//                             Team Members <span className="text-sm text-zinc-600 dark:text-zinc-400">({project?.teamMemberEmails?.length ?? 0})
//                             </span>
//                         </h2>
//                         <button
//                             type="button"
//                             onClick={() => setIsDialogOpen(true)}
//                             className="p-2 rounded-lg border border-zinc-300 dark:border-zinc-700 hover:bg-zinc-100 dark:hover:bg-zinc-800"
//                         >
//                             <Plus className="size-4 text-zinc-900 dark:text-zinc-300" />
//                         </button>
//                         <AddProjectMember
//                             isDialogOpen={isDialogOpen}
//                             setIsDialogOpen={setIsDialogOpen}
//                             project={project}
//                             selectedMemberIds={selectedMemberIds}
//                             setSelectedMemberIds={setSelectedMemberIds}
//                         />
//                     </div>

//                     {/* Member List */}
//                     {project?.teamMemberEmails?.length > 0 && (
//                         <div className="space-y-2 mt-2 max-h-32 overflow-y-auto">
//                             {project?.teamMemberEmails?.length > 0 && (
//                                 <div className="space-y-2 mt-2 max-h-32 overflow-y-auto">
//                                     {project.teamMemberEmails.map((email, index) => (
//                                         <div
//                                             key={index}
//                                             className="flex items-center justify-between px-3 py-2 rounded dark:bg-zinc-800 text-sm text-zinc-900 dark:text-zinc-300"
//                                         >
//                                             <span>{email}</span>
//                                         </div>
//                                     ))}
//                                 </div>
//                             )}

//                         </div>
//                     )}
//                 </div>
//             </div>
//         </div>
//     );
// }

import { format } from "date-fns";
import { Plus, Save } from "lucide-react";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import AddProjectMember from "./AddProjectMember";
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

    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const { role, userId } = useSelector((state) => state.user);

    const [selectedMemberIds, setSelectedMemberIds] = useState([]);
    const [selectedTeamLeadId, setSelectedTeamLeadId] = useState(null);

    // Update form and members when project changes
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

        const initialMemberIds = project.teamMembers?.map(member => member.id) || [];
        setSelectedMemberIds(initialMemberIds);
    }, [project]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!project) return;

        setIsSubmitting(true);

        try {
            const payload = {
                title: formData.name || null,
                description: formData.description || null,
                status: formData.status,
                priority: formData.priority,
                startDate: formData.start_date ? new Date(formData.start_date).toISOString() : null,
                endDate: formData.end_date ? new Date(formData.end_date).toISOString() : null,
                progress: formData.progress,
                teamMemberIds: selectedMemberIds,
                teamLeadId: selectedTeamLeadId,
            };

            console.log("PUT payload:", payload);
            await updateProject(project.id, payload);

            alert("Project updated successfully!");
        } catch (error) {
            console.error("Failed to update project:", error);
            alert("Failed to update project.");
        } finally {
            setIsSubmitting(false);
        }
    };

    const inputClasses = "w-full px-3 py-2 rounded mt-2 border text-sm dark:bg-zinc-900 border-zinc-300 dark:border-zinc-700 text-zinc-900 dark:text-zinc-300";
    const cardClasses = "rounded-lg border p-6 not-dark:bg-white dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border-zinc-300 dark:border-zinc-800";
    const labelClasses = "text-sm text-zinc-600 dark:text-zinc-400";

    return (
        <div className="grid lg:grid-cols-2 gap-8">
            {/* Project Details */}
            <div className={cardClasses}>
                <h2 className="text-lg font-medium text-zinc-900 dark:text-zinc-300 mb-4">Project Details</h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    {/* Name */}
                    <div className="space-y-2">
                        <label className={labelClasses}>Project Name</label>
                        <input
                            value={formData.name}
                            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                            className={inputClasses}
                            required
                        />
                    </div>

                    {/* Description */}
                    <div className="space-y-2">
                        <label className={labelClasses}>Description</label>
                        <textarea
                            value={formData.description}
                            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                            className={`${inputClasses} h-24`}
                        />
                    </div>

                    {/* Status & Priority */}
                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className={labelClasses}>Status</label>
                            <select
                                value={formData.status}
                                onChange={(e) => setFormData({ ...formData, status: e.target.value })}
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
                                onChange={(e) => setFormData({ ...formData, priority: e.target.value })}
                                className={inputClasses}
                            >
                                <option value="LOW">Low</option>
                                <option value="MEDIUM">Medium</option>
                                <option value="HIGH">High</option>
                            </select>
                        </div>
                    </div>

                    {/* Timeline */}
                    <div className="space-y-4 grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className={labelClasses}>Start Date</label>
                            <input
                                type="date"
                                value={formData.start_date ? format(new Date(formData.start_date), "yyyy-MM-dd") : ""}
                                onChange={(e) => setFormData({ ...formData, start_date: e.target.value ? new Date(e.target.value) : null })}
                                className={inputClasses}
                            />
                        </div>
                        <div className="space-y-2">
                            <label className={labelClasses}>End Date</label>
                            <input
                                type="date"
                                value={formData.end_date ? format(new Date(formData.end_date), "yyyy-MM-dd") : ""}
                                onChange={(e) => setFormData({ ...formData, end_date: e.target.value ? new Date(e.target.value) : null })}
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
                            onChange={(e) => setFormData({ ...formData, progress: Number(e.target.value) })}
                            className="w-full accent-blue-500 dark:accent-blue-400"
                        />
                    </div>

                    {/* Save Button */}
                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="ml-auto flex items-center text-sm justify-center gap-2 bg-gradient-to-br from-blue-500 to-blue-600 text-white px-4 py-2 rounded"
                    >
                        <Save className="size-4" /> {isSubmitting ? "Saving..." : "Save Changes"}
                    </button>
                </form>
            </div>

            {/* Team Members */}
            <div className="space-y-6">
                <div className={cardClasses}>
                    <div className="flex items-center justify-between gap-4">
                        <h2 className="text-lg font-medium text-zinc-900 dark:text-zinc-300 mb-4">
                            Team Members <span className="text-sm text-zinc-600 dark:text-zinc-400">({selectedMemberIds.length})</span>
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
                            selectedMemberIds={selectedMemberIds}
                            setSelectedMemberIds={setSelectedMemberIds}
                        />
                    </div>

                    {/* Member List */}
                    <div className="space-y-2 mt-2 max-h-32 overflow-y-auto">
                        {selectedMemberIds.map(memberId => {
                            const member = project.teamMembers.find(m => m.id === memberId);
                            return member ? (
                                <div key={member.id} className="flex items-center justify-between px-3 py-2 rounded dark:bg-zinc-800 text-sm text-zinc-900 dark:text-zinc-300">
                                    <span>{member.email}</span>
                                </div>
                            ) : null;
                        })}
                    </div>
                </div>
            </div>
        </div>
    );
}
