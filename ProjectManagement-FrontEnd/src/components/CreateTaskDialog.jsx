// import { useState, useEffect } from "react";
// import { Calendar as CalendarIcon } from "lucide-react";
// import { format } from "date-fns";
// import { useSelector } from "react-redux";
// import { createTask } from "../services/ProjectTask/ProjectTaskAPI";
// import { getAllUsersSimple, getAllUsers } from "../services/Team/team.api";
// import { getProjectUpdateForm } from "../services/Project/ProjectAPI";

// export default function CreateTaskDialog({ showCreateTask, setShowCreateTask, projectId }) {
//   const { role, email, id: userId } = useSelector((state) => state.user ?? {});

//   const [isSubmitting, setIsSubmitting] = useState(false);
//   const [members, setMembers] = useState([]);
//   const [loadingMembers, setLoadingMembers] = useState(true);
//   const [allUsersMap, setAllUsersMap] = useState({}); 

//   const [formData, setFormData] = useState({
//     title: "",
//     description: "",
//     type: "TASK",
//     status: "TO_DO",
//     priority: "MEDIUM",
//     assignToUserEmail: "",
//     dueDate: "",
//   });

//   if (!showCreateTask || role === "MEMBER") return null;

//   useEffect(() => {
//     if (!showCreateTask || !projectId) return;

//     const fetchMembers = async () => {
//       setLoadingMembers(true);
//       try {
//         const allUsersRes = await getAllUsers();
//         const allUsers = allUsersRes.data || [];
//         const userMap = {};
//         allUsers.forEach((u) => {
//           userMap[u.email] = u.id;
//         });
//         setAllUsersMap(userMap);
//         console.log("All users:", allUsers);


//         const projectRes = await getProjectUpdateForm(projectId);
//         const project = projectRes;
//         console.log("Project API response:", project);

//         const projectMembers = allUsers.filter(
//           (u) =>
//             project.teamMemberIds?.includes(u.id) ||
//             u.id === project.teamLeadId
//         );

//         console.log("Project members after filter:", projectMembers);
//         setMembers(projectMembers);

//         if (role === "LEADER" && projectMembers.some((m) => m.email === email)) {
//           setFormData((prev) => ({
//             ...prev,
//             assignToUserEmail: email,
//           }));
//         }
//       } catch (err) {
//         console.error("Failed to fetch project members:", err);
//         setMembers([]);
//       } finally {
//         setLoadingMembers(false);
//       }
//     };

//     fetchMembers();
//   }, [showCreateTask, projectId, role, email]);

//   /* ================================
//      Submit
//   ================================= */
//   const handleSubmit = async (e) => {
//     e.preventDefault();
//     setIsSubmitting(true);

//     try {
//       const assignToUserId = formData.assignToUserEmail
//         ? allUsersMap[formData.assignToUserEmail]
//         : null;

//       const payload = {
//         projectId,
//         assignToUserId,
//         title: formData.title,
//         description: formData.description,
//         type: formData.type,
//         status: formData.status,
//         priority: formData.priority,
//         dueDate: formData.dueDate || null,
//       };

//       console.log("Create task payload:", payload);

//       await createTask(payload);

//       setShowCreateTask(false);
//       setFormData({
//         title: "",
//         description: "",
//         type: "TASK",
//         status: "TO_DO",
//         priority: "MEDIUM",
//         assignToUserEmail: "",
//         dueDate: "",
//       });
//     } catch (err) {
//       console.error("Failed to create task:", err);
//       alert("Failed to create task");
//     } finally {
//       setIsSubmitting(false);
//     }
//   };

//   return (
//     <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/20 dark:bg-black/60 backdrop-blur">
//       <div className="bg-white dark:bg-zinc-950 border border-zinc-300 dark:border-zinc-800 rounded-lg shadow-lg w-full max-w-md p-6">
//         <h2 className="text-xl font-bold mb-4 text-zinc-900 dark:text-white">
//           Create New Task
//         </h2>

//         <form onSubmit={handleSubmit} className="space-y-4">
//           {/* Title */}
//           <div>
//             <label className="text-sm font-medium">Title</label>
//             <input
//               value={formData.title}
//               onChange={(e) => setFormData({ ...formData, title: e.target.value })}
//               required
//               className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
//             />
//           </div>

//           {/* Description */}
//           <div>
//             <label className="text-sm font-medium">Description</label>
//             <textarea
//               value={formData.description}
//               onChange={(e) => setFormData({ ...formData, description: e.target.value })}
//               className="w-full mt-1 px-3 py-2 rounded border h-24 dark:bg-zinc-900"
//             />
//           </div>

//           {/* Type & Priority */}
//           <div className="grid grid-cols-2 gap-4">
//             <div>
//               <label className="text-sm font-medium">Type</label>
//               <select
//                 value={formData.type}
//                 onChange={(e) => setFormData({ ...formData, type: e.target.value })}
//                 className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
//               >
//                 <option value="TASK">Task</option>
//                 <option value="BUG">Bug</option>
//                 <option value="FEATURE">Feature</option>
//                 <option value="IMPROVEMENT">Improvement</option>
//                 <option value="OTHER">Other</option>
//               </select>
//             </div>

//             <div>
//               <label className="text-sm font-medium">Priority</label>
//               <select
//                 value={formData.priority}
//                 onChange={(e) => setFormData({ ...formData, priority: e.target.value })}
//                 className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
//               >
//                 <option value="LOW">Low</option>
//                 <option value="MEDIUM">Medium</option>
//                 <option value="HIGH">High</option>
//               </select>
//             </div>
//           </div>

//           {/* Assignee & Status */}
//           <div className="grid grid-cols-2 gap-4">
//             <div>
//               <label className="text-sm font-medium">Assignee</label>
//               <select
//                 value={formData.assignToUserEmail}
//                 onChange={(e) => setFormData({ ...formData, assignToUserEmail: e.target.value })}
//                 className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
//                 disabled={loadingMembers}
//               >
//                 <option value="">Unassigned</option>
//                 {loadingMembers ? (
//                   <option disabled>Loading...</option>
//                 ) : (
//                   members.map((m) => (
//                     <option key={m.id} value={m.email}>
//                       {m.email}
//                     </option>
//                   ))
//                 )}
//               </select>
//             </div>

//             <div>
//               <label className="text-sm font-medium">Status</label>
//               <select
//                 value={formData.status}
//                 onChange={(e) => setFormData({ ...formData, status: e.target.value })}
//                 className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
//               >
//                 <option value="TO_DO">To Do</option>
//                 <option value="IN_PROGRESS">In Progress</option>
//                 <option value="DONE">Done</option>
//               </select>
//             </div>
//           </div>

//           {/* Due Date */}
//           <div>
//             <label className="text-sm font-medium">Due Date</label>
//             <div className="flex items-center gap-2">
//               <CalendarIcon className="w-5 h-5 text-zinc-400" />
//               <input
//                 type="date"
//                 value={formData.dueDate}
//                 onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
//                 className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
//               />
//             </div>
//             {formData.dueDate && (
//               <p className="text-xs text-zinc-500 mt-1">{format(new Date(formData.dueDate), "PPP")}</p>
//             )}
//           </div>

//           {/* Footer */}
//           <div className="flex justify-end gap-2 pt-4">
//             <button
//               type="button"
//               onClick={() => setShowCreateTask(false)}
//               className="px-4 py-2 border rounded"
//             >
//               Cancel
//             </button>
//             <button
//               type="submit"
//               disabled={isSubmitting || loadingMembers}
//               className="px-4 py-2 bg-blue-600 text-white rounded"
//             >
//               {isSubmitting ? "Creating..." : "Create Task"}
//             </button>
//           </div>
//         </form>
//       </div>
//     </div>
//   );
// }


import { useState, useEffect } from "react";
import { Calendar as CalendarIcon } from "lucide-react";
import { format } from "date-fns";
import { useSelector } from "react-redux";
import { createTask } from "../services/ProjectTask/ProjectTaskAPI";
import { getAllUsers } from "../services/Team/team.api";
import { getProjectUpdateForm } from "../services/Project/ProjectAPI";

export default function CreateTaskDialog({ showCreateTask, setShowCreateTask, projectId, onTaskCreated }) {
  const { role, email, id: userId } = useSelector((state) => state.user ?? {});

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [members, setMembers] = useState([]);
  const [loadingMembers, setLoadingMembers] = useState(true);
  const [allUsersMap, setAllUsersMap] = useState({}); // email -> id 映射

  const [formData, setFormData] = useState({
    title: "",
    description: "",
    type: "TASK",
    status: "TO_DO",
    priority: "MEDIUM",
    assignToUserEmail: "",
    dueDate: "",
  });

  if (!showCreateTask || role === "MEMBER") return null;

  useEffect(() => {
    if (!showCreateTask || !projectId) return;

    const fetchMembers = async () => {
      setLoadingMembers(true);
      try {
        const allUsersRes = await getAllUsers();
        const allUsers = allUsersRes.data || [];

        // 构建 email -> id 映射
        const userMap = {};
        allUsers.forEach((u) => {
          userMap[u.email] = u.id;
        });
        setAllUsersMap(userMap);

        const projectRes = await getProjectUpdateForm(projectId);
        const project = projectRes;

        const projectMembers = allUsers.filter(
          (u) =>
            project.teamMemberIds?.includes(u.id) ||
            u.id === project.teamLeadId
        );

        setMembers(projectMembers);

        if (role === "LEADER" && projectMembers.some((m) => m.email === email)) {
          setFormData((prev) => ({
            ...prev,
            assignToUserEmail: email,
          }));
        }
      } catch (err) {
        console.error("Failed to fetch project members:", err);
        setMembers([]);
      } finally {
        setLoadingMembers(false);
      }
    };

    fetchMembers();
  }, [showCreateTask, projectId, role, email]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);

    try {
      const assignToUserId = formData.assignToUserEmail
        ? allUsersMap[formData.assignToUserEmail]
        : null;

      const payload = {
        projectId,
        assignToUserId,
        title: formData.title,
        description: formData.description,
        type: formData.type,
        status: formData.status,
        priority: formData.priority,
        dueDate: formData.dueDate || null,
      };

      const createdTask = await createTask(payload);

      // 通知父组件刷新任务列表
      onTaskCreated?.(createdTask);

      setShowCreateTask(false);
      setFormData({
        title: "",
        description: "",
        type: "TASK",
        status: "TO_DO",
        priority: "MEDIUM",
        assignToUserEmail: "",
        dueDate: "",
      });
    } catch (err) {
      console.error("Failed to create task:", err);
      alert("Failed to create task");
    } finally {
      setIsSubmitting(false);
    }
  };

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
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
              required
              className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
            />
          </div>

          {/* Description */}
          <div>
            <label className="text-sm font-medium">Description</label>
            <textarea
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              className="w-full mt-1 px-3 py-2 rounded border h-24 dark:bg-zinc-900"
            />
          </div>

          {/* Type & Priority */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="text-sm font-medium">Type</label>
              <select
                value={formData.type}
                onChange={(e) => setFormData({ ...formData, type: e.target.value })}
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
                onChange={(e) => setFormData({ ...formData, priority: e.target.value })}
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
                value={formData.assignToUserEmail}
                onChange={(e) => setFormData({ ...formData, assignToUserEmail: e.target.value })}
                className="w-full mt-1 px-3 py-2 rounded border dark:bg-zinc-900"
                disabled={loadingMembers}
              >
                <option value="">Unassigned</option>
                {loadingMembers ? (
                  <option disabled>Loading...</option>
                ) : (
                  members.map((m) => (
                    <option key={m.id} value={m.email}>
                      {m.email}
                    </option>
                  ))
                )}
              </select>
            </div>

            <div>
              <label className="text-sm font-medium">Status</label>
              <select
                value={formData.status}
                onChange={(e) => setFormData({ ...formData, status: e.target.value })}
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
                onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
                className="w-full px-3 py-2 rounded border dark:bg-zinc-900"
              />
            </div>
            {formData.dueDate && (
              <p className="text-xs text-zinc-500 mt-1">{format(new Date(formData.dueDate), "PPP")}</p>
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
              disabled={isSubmitting || loadingMembers}
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
