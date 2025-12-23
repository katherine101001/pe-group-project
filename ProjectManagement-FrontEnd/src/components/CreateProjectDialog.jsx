// import { useState, useEffect } from "react";
// import { XIcon } from "lucide-react";
// import { useSelector } from "react-redux";
// import { createProject } from "../services/Project/ProjectAPI";
// import { getAllUsersSimple } from "../services/Team/team.api";

// const CreateProjectDialog = ({ isDialogOpen, setIsDialogOpen }) => {
//   const { role, email } = useSelector((state) => state.user ?? {});

//   const [allUsers, setAllUsers] = useState([]);
//   const [isSubmitting, setIsSubmitting] = useState(false);

//   const [formData, setFormData] = useState({
//     name: "",
//     description: "",
//     status: "PLANNING",
//     priority: "MEDIUM",
//     start_date: "",
//     end_date: "",
//     team_members: [],
//     team_lead: "",
//   });


//   /* ================================
//      Fetch all users (ADMIN / LEADER)
//   ================================= */
//   useEffect(() => {
//     if (!isDialogOpen) return;

//     const fetchUsers = async () => {
//       try {
//         const response = await getAllUsersSimple();
//         setAllUsers(response.data);
//       } catch (err) {
//         console.error("Failed to fetch users:", err);
//       }
//     };

//     fetchUsers();
//   }, [isDialogOpen]);

//   /* ================================
//      LEADER: 自动成为 Project Lead
//   ================================= */
//   useEffect(() => {
//     if (role === "LEADER" && email) {
//       setFormData((prev) => ({
//         ...prev,
//         team_lead: email,
//         team_members: [...new Set([...prev.team_members, email])],
//       }));
//     }
//   }, [role, email]);


//     /* ================================
//      RBAC: MEMBER 不允许创建 Project
//   ================================= */
//   if (!isDialogOpen || role === "MEMBER") return null;
//   /* ================================
//      Submit
//   ================================= */
//   const handleSubmit = async (e) => {
//     e.preventDefault();
//     setIsSubmitting(true);

//     try {
//       const payload = {
//         title: formData.name,
//         description: formData.description,
//         status: formData.status,
//         priority: formData.priority,
//         startDate: formData.start_date || null,
//         endDate: formData.end_date || null,
//         teamLeadEmail:
//           role === "LEADER" ? email : formData.team_lead || null,
//         teamMemberEmails: [...new Set(formData.team_members)],
//       };

//       await createProject(payload);

//       alert("Project created successfully!");
//       setIsDialogOpen(false);

//       setFormData({
//         name: "",
//         description: "",
//         status: "PLANNING",
//         priority: "MEDIUM",
//         start_date: "",
//         end_date: "",
//         team_members: [],
//         team_lead: "",
//       });
//     } catch (err) {
//       console.error("Failed to create project:", err);
//       alert("Failed to create project.");
//     } finally {
//       setIsSubmitting(false);
//     }
//   };

//   const removeTeamMember = (email) => {
//     setFormData((prev) => ({
//       ...prev,
//       team_members: prev.team_members.filter((m) => m !== email),
//     }));
//   };

//   /* ================================
//      UI
//   ================================= */
//   return (
//     <div className="fixed inset-0 bg-black/20 dark:bg-black/60 backdrop-blur flex items-center justify-center z-50">
//       <div className="bg-white dark:bg-zinc-950 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 w-full max-w-lg relative">
//         <button
//           className="absolute top-3 right-3 text-zinc-500 hover:text-zinc-700"
//           onClick={() => setIsDialogOpen(false)}
//         >
//           <XIcon className="size-5" />
//         </button>

//         <h2 className="text-xl font-medium mb-4">Create New Project</h2>

//         <form onSubmit={handleSubmit} className="space-y-4">
//           {/* Project Name */}
//           <div>
//             <label className="text-sm">Project Name</label>
//             <input
//               required
//               value={formData.name}
//               onChange={(e) =>
//                 setFormData({ ...formData, name: e.target.value })
//               }
//               className="w-full mt-1 px-3 py-2 border rounded text-sm dark:bg-zinc-900"
//             />
//           </div>

//           {/* Description */}
//           <div>
//             <label className="text-sm">Description</label>
//             <textarea
//               value={formData.description}
//               onChange={(e) =>
//                 setFormData({ ...formData, description: e.target.value })
//               }
//               className="w-full mt-1 px-3 py-2 border rounded text-sm dark:bg-zinc-900 h-20"
//             />
//           </div>

//           {/* Status & Priority */}
//           <div className="grid grid-cols-2 gap-4">
//             <select
//               value={formData.status}
//               onChange={(e) =>
//                 setFormData({ ...formData, status: e.target.value })
//               }
//               className="px-3 py-2 border rounded text-sm dark:bg-zinc-900"
//             >
//               <option value="PLANNING">Planning</option>
//               <option value="ACTIVE">Active</option>
//               <option value="COMPLETED">Completed</option>
//             </select>

//             <select
//               value={formData.priority}
//               onChange={(e) =>
//                 setFormData({ ...formData, priority: e.target.value })
//               }
//               className="px-3 py-2 border rounded text-sm dark:bg-zinc-900"
//             >
//               <option value="LOW">Low</option>
//               <option value="MEDIUM">Medium</option>
//               <option value="HIGH">High</option>
//             </select>
//           </div>

//           {/* Project Lead */}
//           <div>
//             <label className="text-sm">Project Lead</label>
//             <select
//               disabled={role === "LEADER"}
//               value={formData.team_lead}
//               onChange={(e) =>
//                 setFormData({
//                   ...formData,
//                   team_lead: e.target.value,
//                   team_members: [
//                     ...new Set([...formData.team_members, e.target.value]),
//                   ],
//                 })
//               }
//               className="w-full mt-1 px-3 py-2 border rounded text-sm dark:bg-zinc-900 disabled:opacity-60"
//             >
//               {role === "ADMIN" && <option value="">No Lead</option>}

//               {role === "ADMIN" &&
//                 allUsers.map((u) => (
//                   <option key={u.email} value={u.email}>
//                     {u.email}
//                   </option>
//                 ))}

//               {role === "LEADER" && (
//                 <option value={email}>{email} (You)</option>
//               )}
//             </select>
//           </div>

//           {/* Team Members */}
//           <div>
//             <label className="text-sm">Team Members</label>
//             <select
//               className="w-full mt-1 px-3 py-2 border rounded text-sm dark:bg-zinc-900"
//               onChange={(e) => {
//                 if (
//                   e.target.value &&
//                   !formData.team_members.includes(e.target.value)
//                 ) {
//                   setFormData((prev) => ({
//                     ...prev,
//                     team_members: [...prev.team_members, e.target.value],
//                   }));
//                 }
//               }}
//             >
//               <option value="">Add member</option>
//               {allUsers
//                 .filter((u) => !formData.team_members.includes(u.email))
//                 .map((u) => (
//                   <option key={u.email} value={u.email}>
//                     {u.email}
//                   </option>
//                 ))}
//             </select>

//             <div className="flex flex-wrap gap-2 mt-2">
//               {formData.team_members.map((email) => (
//                 <span
//                   key={email}
//                   className="bg-blue-500/10 text-blue-600 px-2 py-1 rounded text-sm flex items-center gap-1"
//                 >
//                   {email}
//                   <button
//                     type="button"
//                     onClick={() => removeTeamMember(email)}
//                   >
//                     <XIcon className="w-3 h-3" />
//                   </button>
//                 </span>
//               ))}
//             </div>
//           </div>

//           {/* Footer */}
//           <div className="flex justify-end gap-3 pt-2">
//             <button
//               type="button"
//               onClick={() => setIsDialogOpen(false)}
//               className="px-4 py-2 border rounded"
//             >
//               Cancel
//             </button>
//             <button
//               disabled={isSubmitting}
//               className="px-4 py-2 bg-blue-600 text-white rounded"
//             >
//               {isSubmitting ? "Creating..." : "Create Project"}
//             </button>
//           </div>
//         </form>
//       </div>
//     </div>
//   );
// };

// export default CreateProjectDialog;


import { useState, useEffect } from "react";
import { XIcon } from "lucide-react";
import { useSelector } from "react-redux";
import { createProject } from "../services/Project/ProjectAPI";
import { getAllUsersSimple } from "../services/Team/team.api"; // your API call


const CreateProjectDialog = ({ isDialogOpen, setIsDialogOpen }) => {
  const { role, email } = useSelector((state) => state.user ?? {});

  const [allUsers, setAllUsers] = useState([]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    status: "PLANNING",
    priority: "MEDIUM",
    start_date: "",
    end_date: "",
    team_members: [],
    team_lead: "",
  });

  /* ================================
     Fetch all users and normalize roles
  ================================= */
  useEffect(() => {
    if (!isDialogOpen) return;

    const fetchUsers = async () => {
      try {

        const res = await getAllUsersSimple();
        // 🔑 Normalize role to uppercase
        const normalizedUsers = (res.data || []).map((u) => ({
          ...u,
          role: u.role.toUpperCase(),
        }));
        setAllUsers(normalizedUsers);
      } catch (err) {
        console.error("Failed to fetch users:", err);
      }
    };

    fetchUsers();
  }, [isDialogOpen]);

  /* ================================
     LEADER: 自动成为 Project Lead
  ================================= */
  useEffect(() => {
    if (role === "LEADER" && email) {
      setFormData((prev) => ({
        ...prev,
        team_lead: email,
        team_members: [...new Set([...prev.team_members, email])],
      }));
    }
  }, [role, email]);

  /* ================================
     RBAC: MEMBER 无法创建 Project
  ================================= */
  if (!isDialogOpen || role === "MEMBER") return null;

  /* ================================
     Submit
  ================================= */
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
        teamLeadEmail: role === "LEADER" ? email : formData.team_lead || null,
        teamMemberEmails: [...new Set(formData.team_members)],
      };

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
      });
    } catch (err) {
      console.error("Failed to create project:", err);
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

  /* ================================
     UI
  ================================= */
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
            <label className="text-sm">Project Name</label>
            <input
              required
              value={formData.name}
              onChange={(e) =>
                setFormData({ ...formData, name: e.target.value })
              }
              className="w-full mt-1 px-3 py-2 border rounded text-sm dark:bg-zinc-900"
            />
          </div>

          {/* Description */}
          <div>
            <label className="text-sm">Description</label>
            <textarea
              value={formData.description}
              onChange={(e) =>
                setFormData({ ...formData, description: e.target.value })
              }
              className="w-full mt-1 px-3 py-2 border rounded text-sm dark:bg-zinc-900 h-20"
            />
          </div>

          {/* Status & Priority */}
          <div className="grid grid-cols-2 gap-4">
            <select
              value={formData.status}
              onChange={(e) =>
                setFormData({ ...formData, status: e.target.value })
              }
              className="px-3 py-2 border rounded text-sm dark:bg-zinc-900"
            >
              <option value="PLANNING">Planning</option>
              <option value="ACTIVE">Active</option>
              <option value="COMPLETED">Completed</option>
            </select>

            <select
              value={formData.priority}
              onChange={(e) =>
                setFormData({ ...formData, priority: e.target.value })
              }
              className="px-3 py-2 border rounded text-sm dark:bg-zinc-900"
            >
              <option value="LOW">Low</option>
              <option value="MEDIUM">Medium</option>
              <option value="HIGH">High</option>
            </select>
          </div>

          {/* Project Lead */}
          <div>
            <label className="text-sm">Project Lead</label>
            <select
              disabled={role === "LEADER"}
              value={formData.team_lead}
              onChange={(e) =>
                setFormData((prev) => ({
                  ...prev,
                  team_lead: e.target.value,
                  team_members: [
                    ...new Set([...prev.team_members, e.target.value]),
                  ],
                }))
              }
              className="w-full mt-1 px-3 py-2 border rounded text-sm dark:bg-zinc-900 disabled:opacity-60"
            >
              {role === "ADMIN" && <option value="">Select Leader</option>}

              {role === "ADMIN" &&
                allUsers
                  .filter((u) => u.role === "LEADER")
                  .map((u) => (
                    <option key={u.email} value={u.email}>
                      {u.email}
                    </option>
                  ))}

              {role === "LEADER" && email && (
                <option value={email}>{email} (You)</option>
              )}
            </select>
          </div>

          {/* Team Members */}
          <div>
            <label className="text-sm">Team Members</label>
            <select
              className="w-full mt-1 px-3 py-2 border rounded text-sm dark:bg-zinc-900"
              onChange={(e) => {
                if (
                  e.target.value &&
                  !formData.team_members.includes(e.target.value)
                ) {
                  setFormData((prev) => ({
                    ...prev,
                    team_members: [...prev.team_members, e.target.value],
                  }));
                }
              }}
            >
              <option value="">Add member</option>

              {allUsers
                .filter((u) => u.role === "MEMBER")
                .filter(
                  (u) => !formData.team_members.includes(u.email)
                )
                .map((u) => (
                  <option key={u.email} value={u.email}>
                    {u.email}
                  </option>
                ))}
            </select>

            <div className="flex flex-wrap gap-2 mt-2">
              {formData.team_members.map((m) => (
                <span
                  key={m}
                  className="bg-blue-500/10 text-blue-600 px-2 py-1 rounded text-sm flex items-center gap-1"
                >
                  {m}
                  <button
                    type="button"
                    onClick={() => removeTeamMember(m)}
                  >
                    <XIcon className="w-3 h-3" />
                  </button>
                </span>
              ))}
            </div>
          </div>

          {/* Footer */}
          <div className="flex justify-end gap-3 pt-2">
            <button
              type="button"
              onClick={() => setIsDialogOpen(false)}
              className="px-4 py-2 border rounded"
            >
              Cancel
            </button>
            <button
              disabled={isSubmitting}
              className="px-4 py-2 bg-blue-600 text-white rounded"
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
