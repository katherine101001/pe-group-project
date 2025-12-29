// import { useEffect, useState } from "react";
// import { UsersIcon, Search, UserPlus, Shield, Activity } from "lucide-react";
// import { useSelector } from "react-redux";
// import InviteMemberDialog from "../components/InviteMemberDialog";
// import { getAllUsersSimple, getTeamStats, searchUsers, registerUser } from "../services/Team/team.api";
// import { Trash2 } from "lucide-react";


// const Team = () => {
//   // const currentRole = localStorage.getItem("role");
//   const currentRole = useSelector((state) => state.user.role);
//   const [users, setUsers] = useState([]);
//   const [searchTerm, setSearchTerm] = useState("");
//   const [isDialogOpen, setIsDialogOpen] = useState(false);
//   const [stats, setStats] = useState({
//     totalMembers: 0,
//     activeProjects: 0,
//     totalTasks: 0
//   });
//   const [loading, setLoading] = useState(false);

//   const filterRoles = (list) =>
//     list.filter(u => ["MEMBER", "LEADER"].includes(u.role?.toUpperCase()));

//   const fetchUsers = async () => {
//     try {
//       setLoading(true);
//       const res = await getAllUsersSimple();
//       const mappedUsers = filterRoles(res.data).map((u, index) => ({
//         // id: index,
//         id: u.email,
//         role: u.role?.toUpperCase(),
//         user: {
//           name: u.name,
//           email: u.email,
//           image: `https://ui-avatars.com/api/?name=${encodeURIComponent(u.name)}`
//         }
//       }));
//       setUsers(mappedUsers);
//     } catch (err) {
//       console.error("Failed to fetch users:", err);
//     } finally {
//       setLoading(false);
//     }
//   };

//   const fetchStats = async () => {
//     try {
//       const res = await getTeamStats();
//       setStats({
//         totalMembers: res.data.totalUsers,
//         activeProjects: res.data.totalProjects,
//         totalTasks: res.data.totalTasks
//       });
//     } catch (err) {
//       console.error("Failed to fetch stats:", err);
//     }
//   };

//   useEffect(() => {
//     fetchUsers();
//     fetchStats();
//   }, []);

//   const handleSearch = async (keyword) => {
//     setSearchTerm(keyword);
//     if (!keyword) {
//       fetchUsers();
//       return;
//     }
//     try {
//       const res = await searchUsers(keyword);
//       const mappedUsers = filterRoles(res.data).map((u, index) => ({
//         id: index,
//         role: u.role?.toUpperCase(),
//         user: {
//           name: u.name,
//           email: u.email,
//           image: `https://ui-avatars.com/api/?name=${encodeURIComponent(u.name)}`
//         }
//       }));
//       setUsers(mappedUsers);
//     } catch (err) {
//       console.error("Search failed:", err);
//     }
//   };

//   const handleInvite = async (dto) => {
//     try {
//       const newUser = await registerUser(dto);
//       const role = dto.role.toUpperCase();
//       if (role !== "MEMBER" && role !== "LEADER") return;

//       setUsers(prev => {
//         const index = prev.findIndex(u => u.user.email.toLowerCase() === dto.email.toLowerCase());
//         if (index !== -1) {
//           const updated = [...prev];
//           updated[index].role = role;
//           return updated;
//         } else {
//           return [
//             ...prev,
//             {
//               id: prev.length,
//               role,
//               user: {
//                 name: newUser.data.name,
//                 email: newUser.data.email,
//                 image: `https://ui-avatars.com/api/?name=${encodeURIComponent(newUser.data.name)}`
//               }
//             }
//           ];
//         }
//       });

//       setIsDialogOpen(false);
//     } catch (err) {
//       console.error("Invite failed:", err);
//       alert(err.response?.data?.message || err.message);
//     }
//   };

//       const filteredUsers = users.filter(
//         user =>
//           user?.user?.name?.toLowerCase().includes(searchTerm.toLowerCase()) ||
//           user?.user?.email?.toLowerCase().includes(searchTerm.toLowerCase())
//       );

//       const handleDeleteUser = async (id) => {
//       if (!window.confirm("Are you sure you want to delete this user?")) return;

//       try {
//         const response = await fetch(`/api/user/${id}`, { method: "DELETE" });

//         if (!response.ok) {
//           // 如果后端返回 400/404/500 等，显示提示
//           const data = await response.json();
//           alert(data.message || "Failed to delete user");
//           return; // 不更新前端列表
//         }

//         // 删除成功，更新前端列表
//         setUsers(prev => prev.filter(u => u.id !== id));
//         alert("User deleted successfully");

//       } catch (err) {
//         console.error(err);
//         alert("Failed to delete user");
//       }
//     };


//   return (
//     <div className="space-y-6 max-w-6xl mx-auto">
//       {/* Header */}
//       <div className="flex flex-col lg:flex-row justify-between items-start lg:items-center gap-6">
//         <div>
//           <h1 className="text-xl sm:text-2xl font-semibold text-gray-900 dark:text-white mb-1">Team</h1>
//           <p className="text-gray-500 dark:text-zinc-400 text-sm">
//             Manage team members and their contributions
//           </p>
//         </div>
//         {currentRole === "ADMIN" && (
//           <button
//             onClick={() => setIsDialogOpen(true)}
//             className="flex items-center px-5 py-2 rounded text-sm bg-gradient-to-br from-blue-500 to-blue-600 hover:opacity-90 text-white transition"
//           >
//             <UserPlus className="w-4 h-4 mr-2" /> Invite Member
//           </button>
//         )}


//         {/* ✅ 关键修改：传入 users 和 setUsers */}
//         <InviteMemberDialog
//           isDialogOpen={isDialogOpen}
//           setIsDialogOpen={setIsDialogOpen}
//           onInvite={handleInvite}
//           users={users}
//           setUsers={setUsers}
//         />
//       </div>
//       {/* Stats Cards */}
//       <div className="flex flex-wrap gap-4">
//         {/* Total Members */}
//         <div className="max-sm:w-full dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border border-gray-300 dark:border-zinc-800 rounded-lg p-6">
//           <div className="flex items-center justify-between gap-8 md:gap-22">
//             <div>
//               <p className="text-sm text-gray-500 dark:text-zinc-400">Total Members</p>
//               <p className="text-xl font-bold text-gray-900 dark:text-white">{stats.totalMembers}</p>
//             </div>
//             <div className="p-3 rounded-xl bg-blue-100 dark:bg-blue-500/10">
//               <UsersIcon className="size-4 text-blue-500 dark:text-blue-200" />
//             </div>
//           </div>
//         </div>

//         {/* Active Projects */}
//         <div className="max-sm:w-full dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border border-gray-300 dark:border-zinc-800 rounded-lg p-6">
//           <div className="flex items-center justify-between gap-8 md:gap-22">
//             <div>
//               <p className="text-sm text-gray-500 dark:text-zinc-400">Active Projects</p>
//               <p className="text-xl font-bold text-gray-900 dark:text-white">{stats.activeProjects}</p>
//             </div>
//             <div className="p-3 rounded-xl bg-emerald-100 dark:bg-emerald-500/10">
//               <Activity className="size-4 text-emerald-500 dark:text-emerald-200" />
//             </div>
//           </div>
//         </div>

//         {/* Total Tasks */}
//         <div className="max-sm:w-full dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border border-gray-300 dark:border-zinc-800 rounded-lg p-6">
//           <div className="flex items-center justify-between gap-8 md:gap-22">
//             <div>
//               <p className="text-sm text-gray-500 dark:text-zinc-400">Total Tasks</p>
//               <p className="text-xl font-bold text-gray-900 dark:text-white">{stats.totalTasks}</p>
//             </div>
//             <div className="p-3 rounded-xl bg-purple-100 dark:bg-purple-500/10">
//               <Shield className="size-4 text-purple-500 dark:text-purple-200" />
//             </div>
//           </div>
//         </div>
//       </div>

//       {/* Search */}
//       <div className="relative max-w-md">
//         <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 dark:text-zinc-400 size-3" />
//         <input
//           placeholder="Search team members..."
//           value={searchTerm}
//           onChange={(e) => handleSearch(e.target.value)}
//           className="pl-8 w-full text-sm rounded-md border border-gray-300 dark:border-zinc-800 text-gray-900 dark:text-white placeholder-gray-400 dark:placeholder-zinc-400 py-2 focus:outline-none focus:border-blue-500"
//         />
//       </div>

//       {/* Team Members */}
//       <div className="w-full">
//         {loading ? (
//           <p className="text-center py-16">Loading...</p>
//         ) : filteredUsers.length === 0 ? (
//           <div className="col-span-full text-center py-16">
//             <div className="w-24 h-24 mx-auto mb-6 bg-gray-200 dark:bg-zinc-800 rounded-full flex items-center justify-center">
//               <UsersIcon className="w-12 h-12 text-gray-400 dark:text-zinc-500" />
//             </div>
//             <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
//               {users.length === 0 ? "No team members yet" : "No members match your search"}
//             </h3>
//             <p className="text-gray-500 dark:text-zinc-400 mb-6">
//               {users.length === 0 ? "Invite team members to start collaborating" : "Try adjusting your search term"}
//             </p>
//           </div>
//         ) : (
//           <div className="max-w-4xl w-full">
//             {/* Desktop Table */}
//             <div className="hidden sm:block overflow-x-auto rounded-md border border-gray-200 dark:border-zinc-800">
//               <table className="min-w-full divide-y divide-gray-200 dark:divide-zinc-800">
//                 <thead className="bg-gray-50 dark:bg-zinc-900/50">
//                   <tr>
//                     <th className="px-6 py-2.5 text-left font-medium text-sm">Name</th>
//                     <th className="px-6 py-2.5 text-left font-medium text-sm">Email</th>
//                     <th className="px-6 py-2.5 text-left font-medium text-sm">Role</th>
//                   </tr>
//                 </thead>
//                 <tbody className="divide-y divide-gray-200 dark:divide-zinc-800">
//                   {filteredUsers.map((user) => (
//                     <tr key={user.id} className="hover:bg-gray-50 dark:hover:bg-zinc-800/50 transition-colors">
//                       <td className="px-6 py-2.5 whitespace-nowrap flex items-center gap-3">
//                         <img src={user.user.image} alt={user.user.name} className="size-7 rounded-full bg-gray-200 dark:bg-zinc-800" />
//                         <span className="text-sm text-zinc-800 dark:text-white truncate">{user.user?.name || "Unknown User"}</span>
//                       </td>
//                       <td className="px-6 py-2.5 whitespace-nowrap text-sm text-gray-500 dark:text-zinc-400">{user.user.email}</td>
//                       <td className="px-6 py-2.5 whitespace-nowrap flex items-center justify-between">
//                       <span className={`px-2 py-1 text-xs rounded-md ${user.role === "LEADER"
//                         ? "bg-blue-100 dark:bg-blue-500/20 text-blue-500 dark:text-blue-400"
//                         : "bg-gray-200 dark:bg-zinc-700 text-gray-700 dark:text-zinc-300"}`}>
//                         {user.role}
//                       </span>
//                       {currentRole === "ADMIN" && (
//                         <button
//                           onClick={() => handleDeleteUser(user.id)}
//                           className="p-1 rounded hover:bg-red-100 dark:hover:bg-red-600/30"
//                         >
//                           <Trash2 className="w-5 h-5 text-red-500" />
//                         </button>
//                       )}
//                     </td>



//                     </tr>
//                   ))}
//                 </tbody>
//               </table>
//             </div>

//             {/* Mobile Cards */}
           
//             <div className="sm:hidden space-y-3">
//               {filteredUsers.map((user) => (
//                 <div key={user.id} className="p-4 border border-gray-200 dark:border-zinc-800 rounded-md bg-white dark:bg-zinc-900">
//                   <div className="flex items-center gap-3 mb-2">
//                     <img src={user.user.image} alt={user.user.name} className="size-9 rounded-full bg-gray-200 dark:bg-zinc-800" />
//                     <div>
//                       <p className="font-medium text-gray-900 dark:text-white">{user.user?.name || "Unknown User"}</p>
//                       <p className="text-sm text-gray-500 dark:text-zinc-400">{user.user.email}</p>
//                     </div>
//                   </div>
//                   {/* ✅ 这里替换原来的 role 显示 */}
//                   <div className="flex items-center justify-between mt-2">
//                   <span className={`px-2 py-1 text-xs rounded-md ${user.role === "LEADER"
//                     ? "bg-blue-100 dark:bg-blue-500/20 text-blue-500 dark:text-blue-400"
//                     : "bg-gray-200 dark:bg-zinc-700 text-gray-700 dark:text-zinc-300"}`}>
//                     {user.role}
//                   </span>
//                   {currentRole === "ADMIN" && (
//                     <button
//                       onClick={() => handleDeleteUser(user.id)}
//                       className="p-1 rounded hover:bg-red-100 dark:hover:bg-red-600/30"
//                     >
//                       <Trash2 className="w-5 h-5 text-red-500" />
//                     </button>
//                   )}
//                 </div>

//                 </div>
//               ))}
//             </div>

//           </div>
//         )}
//       </div>
//     </div>
//   );
// };

// export default Team;


import { useEffect, useState } from "react";
import { UsersIcon, Search, UserPlus, Shield, Activity } from "lucide-react";
import { useSelector } from "react-redux";
import InviteMemberDialog from "../components/InviteMemberDialog";
import { 
  getAllUsersSimple, 
  getAllUsers, 
  getTeamStats, 
  searchUsers, 
  registerUser,
  deleteUser
} from "../services/Team/team.api";
import { Trash2 } from "lucide-react";

const Team = () => {
  const currentRole = useSelector((state) => state.user.role);
  const [users, setUsers] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [stats, setStats] = useState({
    totalMembers: 0,
    activeProjects: 0,
    totalTasks: 0
  });
  const [loading, setLoading] = useState(false);

  const filterRoles = (list) =>
    list.filter(u => ["MEMBER", "LEADER"].includes(u.role?.toUpperCase()));

  // ✅ 获取用户并合并 GUID 和 role
  const fetchUsers = async () => {
    try {
      setLoading(true);

      const [simpleRes, allRes] = await Promise.all([getAllUsersSimple(), getAllUsers()]);

      const mappedUsers = filterRoles(simpleRes.data).map((su, index) => {
        const full = allRes.data.find(u => u.email === su.email);
        return {
          id: full?.id, // GUID
          role: su.role?.toUpperCase(),
          user: {
            name: su.name,
            email: su.email,
            image: `https://ui-avatars.com/api/?name=${encodeURIComponent(su.name)}`
          }
        };
      });

      setUsers(mappedUsers);
    } catch (err) {
      console.error("Failed to fetch users:", err);
    } finally {
      setLoading(false);
    }
  };

  const fetchStats = async () => {
    try {
      const res = await getTeamStats();
      setStats({
        totalMembers: res.data.totalUsers,
        activeProjects: res.data.totalProjects,
        totalTasks: res.data.totalTasks
      });
    } catch (err) {
      console.error("Failed to fetch stats:", err);
    }
  };

  useEffect(() => {
    fetchUsers();
    fetchStats();
  }, []);

  // 搜索
  const handleSearch = async (keyword) => {
    setSearchTerm(keyword);
    if (!keyword) {
      fetchUsers();
      return;
    }
    try {
      const res = await searchUsers(keyword);
      const mappedUsers = filterRoles(res.data).map((u, index) => ({
        id: u.id || index, // 如果后端没给 id，用 index 临时
        role: u.role?.toUpperCase(),
        user: {
          name: u.name,
          email: u.email,
          image: `https://ui-avatars.com/api/?name=${encodeURIComponent(u.name)}`
        }
      }));
      setUsers(mappedUsers);
    } catch (err) {
      console.error("Search failed:", err);
    }
  };

  // 邀请用户
  const handleInvite = async (dto) => {
    try {
      const newUser = await registerUser(dto);
      const role = dto.role.toUpperCase();
      if (role !== "MEMBER" && role !== "LEADER") return;

      setUsers(prev => {
        const index = prev.findIndex(u => u.user.email.toLowerCase() === dto.email.toLowerCase());
        if (index !== -1) {
          const updated = [...prev];
          updated[index].role = role;
          return updated;
        } else {
          return [
            ...prev,
            {
              id: newUser.data.id || prev.length,
              role,
              user: {
                name: newUser.data.name,
                email: newUser.data.email,
                image: `https://ui-avatars.com/api/?name=${encodeURIComponent(newUser.data.name)}`
              }
            }
          ];
        }
      });

      setIsDialogOpen(false);
    } catch (err) {
      console.error("Invite failed:", err);
      alert(err.response?.data?.message || err.message);
    }
  };

  const handleDeleteUser = async (id) => {
    if (!id) return alert("User ID not found");
    if (!window.confirm("Are you sure you want to delete this user?")) return;

    try {
      const response = await deleteUser(id);
      alert(response.data?.message || "User deleted successfully");
      setUsers(prev => prev.filter(u => u.id !== id));
    } catch (err) {
      console.error(err);
      alert(err.response?.data?.message || "Failed to delete user");
    }
  };

  const filteredUsers = users.filter(
    user =>
      user?.user?.name?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user?.user?.email?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="space-y-6 max-w-6xl mx-auto">
      {/* Header */}
      <div className="flex flex-col lg:flex-row justify-between items-start lg:items-center gap-6">
        <div>
          <h1 className="text-xl sm:text-2xl font-semibold text-gray-900 dark:text-white mb-1">Team</h1>
          <p className="text-gray-500 dark:text-zinc-400 text-sm">
            Manage team members and their contributions
          </p>
        </div>
        {currentRole === "ADMIN" && (
          <button
            onClick={() => setIsDialogOpen(true)}
            className="flex items-center px-5 py-2 rounded text-sm bg-gradient-to-br from-blue-500 to-blue-600 hover:opacity-90 text-white transition"
          >
            <UserPlus className="w-4 h-4 mr-2" /> Invite Member
          </button>
        )}
        <InviteMemberDialog
          isDialogOpen={isDialogOpen}
          setIsDialogOpen={setIsDialogOpen}
          onInvite={handleInvite}
          users={users}
          setUsers={setUsers}
        />
      </div>

      {/* Stats */}
      <div className="flex flex-wrap gap-4">
        <div className="max-sm:w-full dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border border-gray-300 dark:border-zinc-800 rounded-lg p-6">
          <div className="flex items-center justify-between gap-8 md:gap-22">
            <div>
              <p className="text-sm text-gray-500 dark:text-zinc-400">Total Members</p>
              <p className="text-xl font-bold text-gray-900 dark:text-white">{stats.totalMembers}</p>
            </div>
            <div className="p-3 rounded-xl bg-blue-100 dark:bg-blue-500/10">
              <UsersIcon className="size-4 text-blue-500 dark:text-blue-200" />
            </div>
          </div>
        </div>

        <div className="max-sm:w-full dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border border-gray-300 dark:border-zinc-800 rounded-lg p-6">
          <div className="flex items-center justify-between gap-8 md:gap-22">
            <div>
              <p className="text-sm text-gray-500 dark:text-zinc-400">Active Projects</p>
              <p className="text-xl font-bold text-gray-900 dark:text-white">{stats.activeProjects}</p>
            </div>
            <div className="p-3 rounded-xl bg-emerald-100 dark:bg-emerald-500/10">
              <Activity className="size-4 text-emerald-500 dark:text-emerald-200" />
            </div>
          </div>
        </div>

        <div className="max-sm:w-full dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border border-gray-300 dark:border-zinc-800 rounded-lg p-6">
          <div className="flex items-center justify-between gap-8 md:gap-22">
            <div>
              <p className="text-sm text-gray-500 dark:text-zinc-400">Total Tasks</p>
              <p className="text-xl font-bold text-gray-900 dark:text-white">{stats.totalTasks}</p>
            </div>
            <div className="p-3 rounded-xl bg-purple-100 dark:bg-purple-500/10">
              <Shield className="size-4 text-purple-500 dark:text-purple-200" />
            </div>
          </div>
        </div>
      </div>

      {/* Search */}
      <div className="relative max-w-md">
        <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 dark:text-zinc-400 size-3" />
        <input
          placeholder="Search team members..."
          value={searchTerm}
          onChange={(e) => handleSearch(e.target.value)}
          className="pl-8 w-full text-sm rounded-md border border-gray-300 dark:border-zinc-800 text-gray-900 dark:text-white placeholder-gray-400 dark:placeholder-zinc-400 py-2 focus:outline-none focus:border-blue-500"
        />
      </div>

      {/* Team Members */}
      <div className="w-full">
        {loading ? (
          <p className="text-center py-16">Loading...</p>
        ) : filteredUsers.length === 0 ? (
          <div className="col-span-full text-center py-16">
            <div className="w-24 h-24 mx-auto mb-6 bg-gray-200 dark:bg-zinc-800 rounded-full flex items-center justify-center">
              <UsersIcon className="w-12 h-12 text-gray-400 dark:text-zinc-500" />
            </div>
            <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
              {users.length === 0 ? "No team members yet" : "No members match your search"}
            </h3>
            <p className="text-gray-500 dark:text-zinc-400 mb-6">
              {users.length === 0 ? "Invite team members to start collaborating" : "Try adjusting your search term"}
            </p>
          </div>
        ) : (
          <div className="max-w-4xl w-full">
            {/* Desktop Table */}
            <div className="hidden sm:block overflow-x-auto rounded-md border border-gray-200 dark:border-zinc-800">
              <table className="min-w-full divide-y divide-gray-200 dark:divide-zinc-800">
                <thead className="bg-gray-50 dark:bg-zinc-900/50">
                  <tr>
                    <th className="px-6 py-2.5 text-left font-medium text-sm">Name</th>
                    <th className="px-6 py-2.5 text-left font-medium text-sm">Email</th>
                    <th className="px-6 py-2.5 text-left font-medium text-sm">Role</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200 dark:divide-zinc-800">
                  {filteredUsers.map((user) => (
                    <tr key={user.id}>
                      <td className="px-6 py-2.5 whitespace-nowrap flex items-center gap-3">
                        <img src={user.user.image} alt={user.user.name} className="size-7 rounded-full bg-gray-200 dark:bg-zinc-800" />
                        <span className="text-sm text-zinc-800 dark:text-white truncate">{user.user?.name || "Unknown User"}</span>
                      </td>
                      <td className="px-6 py-2.5 whitespace-nowrap text-sm text-gray-500 dark:text-zinc-400">{user.user.email}</td>
                      <td className="px-6 py-2.5 whitespace-nowrap flex items-center justify-between">
                        <span className={`px-2 py-1 text-xs rounded-md ${user.role === "LEADER"
                          ? "bg-blue-100 dark:bg-blue-500/20 text-blue-500 dark:text-blue-400"
                          : "bg-gray-200 dark:bg-zinc-700 text-gray-700 dark:text-zinc-300"}`}>
                          {user.role}
                        </span>
                        {currentRole === "ADMIN" && (
                          <button
                            onClick={() => handleDeleteUser(user.id)}
                            className="p-1 rounded hover:bg-red-100 dark:hover:bg-red-600/30"
                          >
                            <Trash2 className="w-5 h-5 text-red-500" />
                          </button>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {/* Mobile Cards */}
            <div className="sm:hidden space-y-3">
              {filteredUsers.map((user) => (
                <div key={user.id} className="p-4 border border-gray-200 dark:border-zinc-800 rounded-md bg-white dark:bg-zinc-900">
                  <div className="flex items-center gap-3 mb-2">
                    <img src={user.user.image} alt={user.user.name} className="size-9 rounded-full bg-gray-200 dark:bg-zinc-800" />
                    <div>
                      <p className="font-medium text-gray-900 dark:text-white">{user.user?.name || "Unknown User"}</p>
                      <p className="text-sm text-gray-500 dark:text-zinc-400">{user.user.email}</p>
                    </div>
                  </div>
                  <div className="flex items-center justify-between mt-2">
                    <span className={`px-2 py-1 text-xs rounded-md ${user.role === "LEADER"
                      ? "bg-blue-100 dark:bg-blue-500/20 text-blue-500 dark:text-blue-400"
                      : "bg-gray-200 dark:bg-zinc-700 text-gray-700 dark:text-zinc-300"}`}>
                      {user.role}
                    </span>
                    {currentRole === "ADMIN" && (
                      <button
                        onClick={() => handleDeleteUser(user.id)}
                        className="p-1 rounded hover:bg-red-100 dark:hover:bg-red-600/30"
                      >
                        <Trash2 className="w-5 h-5 text-red-500" />
                      </button>
                    )}
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Team;
