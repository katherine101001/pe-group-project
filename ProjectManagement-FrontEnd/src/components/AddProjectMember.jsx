import { useState, useEffect } from "react";
import { Mail, UserPlus, X } from "lucide-react";
import { getAllUsersComplete } from "../services/Team/team.api";

const AddProjectMember = ({ isDialogOpen, setIsDialogOpen, project, selectedMemberIds, setSelectedMemberIds }) => {
    const [selectedMemberId, setSelectedMemberId] = useState("");
    const [availableMembers, setAvailableMembers] = useState([]);

    // Fetch all users and compute available members
    const fetchAvailableMembers = async () => {
        try {
            const response = await getAllUsersComplete();
            const users = response.data; // Axios response

            if (!Array.isArray(users)) {
                console.error("Expected users array but got:", users);
                return;
            }

            const excludedIds = new Set(selectedMemberIds);

            const members = users.filter(
                u =>
                    u.role === "Member" &&
                    !excludedIds.has(u.id) &&
                    u.email !== project.teamLeadEmail
            );

            setAvailableMembers(members);
        } catch (err) {
            console.error("Failed to fetch members:", err);
        }
    };

    // Refresh available members whenever dialog opens or selected members change
    useEffect(() => {
        if (!isDialogOpen || !project?.id) return;
        fetchAvailableMembers();
    }, [isDialogOpen, project, selectedMemberIds]);

    const handleSubmit = e => {
        e.preventDefault();
        if (!selectedMemberId) return;

        if (!selectedMemberIds.includes(selectedMemberId)) {
            setSelectedMemberIds([...selectedMemberIds, selectedMemberId]);
        }

        setSelectedMemberId("");
        setIsDialogOpen(false);
    };

    if (!isDialogOpen || !project) return null;

    return (
        <div className="fixed inset-0 bg-black/20 backdrop-blur flex items-center justify-center z-50">
            <div className="bg-white dark:bg-zinc-950 border border-zinc-300 dark:border-zinc-800 rounded-xl p-6 w-full max-w-md text-zinc-900 dark:text-zinc-200">
                <h2 className="text-xl font-bold flex items-center gap-2 mb-2">
                    <UserPlus className="size-5" /> Add Member
                </h2>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div className="relative">
                        <Mail className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-500" />
                        <select
                            value={selectedMemberId}
                            onChange={e => setSelectedMemberId(e.target.value)}
                            className="pl-10 w-full rounded border border-zinc-300 dark:border-zinc-700 dark:bg-zinc-900 text-zinc-900 dark:text-zinc-200 py-2 text-sm"
                            required
                        >
                            <option value="">Select a member</option>
                            {availableMembers.map(u => (
                                <option key={u.id} value={u.id}>
                                    {u.name} ({u.email})
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="flex justify-end gap-3 pt-2">
                        <button
                            type="button"
                            onClick={() => setIsDialogOpen(false)}
                            className="px-5 py-2 border rounded"
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            className="px-5 py-2 rounded bg-blue-600 text-white"
                        >
                            Add Member
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default AddProjectMember;
