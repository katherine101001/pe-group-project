

import { useEffect, useState, useMemo } from 'react';
import { CheckSquareIcon, ChevronDownIcon, ChevronRightIcon, Bug, Zap, Square, GitCommit, MessageSquare } from 'lucide-react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { getAllTasks } from '../services/ProjectTask/ProjectTaskAPI';
import { getAllUsers } from "../services/Team/team.api";
import { getAllProjects } from '../services/Project/ProjectAPI';

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

function MyTasksSidebar({ searchKeyword, refreshKey }) { // added refreshKey
    const currentUser = useSelector(state => state.user);
    const [showMyTasks, setShowMyTasks] = useState(false);
    const [myTasks, setMyTasks] = useState([]);

    const toggleMyTasks = () => setShowMyTasks(prev => !prev);

    const getTaskStatusColor = (status) => {
        switch (status) {
            case 'DONE': return 'bg-green-500';
            case 'IN_PROGRESS': return 'bg-yellow-500';
            case 'TO_DO': return 'bg-gray-500 dark:bg-zinc-500';
            default: return 'bg-gray-400 dark:bg-zinc-400';
        }
    };

    const sidebarTitle = currentUser?.role === 'ADMIN'
        ? 'All Tasks'
        : currentUser?.role === 'LEADER'
            ? 'Project Tasks'
            : 'My Tasks';

    // Fetch tasks whenever refreshKey changes
    useEffect(() => {
        const fetchData = async () => {
            if (!currentUser?.userId || !currentUser.role) return;

            try {
                const tasks = await getAllTasks();
                const projects = await getAllProjects();

                let visibleTasks = tasks;

                if (currentUser.role === "LEADER") {
                    const usersRes = await getAllUsers();
                    const teamIds = usersRes.data
                        .filter(u => u.email !== currentUser.email)
                        .map(u => u.id);

                    const leaderProjectIds = projects
                        .filter(p => p.leaderId === currentUser.userId || p.memberIds?.includes(currentUser.userId))
                        .map(p => p.id);

                    visibleTasks = tasks.filter(
                        t =>
                            (t.assignToUserId === currentUser.userId || teamIds.includes(t.assignToUserId)) &&
                            leaderProjectIds.includes(t.projectId)
                    );
                } else if (currentUser.role === "MEMBER") {
                    visibleTasks = tasks.filter(t => t.assignToUserId === currentUser.userId);
                }

                setMyTasks(visibleTasks);
            } catch (err) {
                console.error("Failed to fetch tasks:", err);
                setMyTasks([]);
            }
        };

        fetchData();
    }, [currentUser.userId, currentUser.role, currentUser.email, refreshKey]); // added refreshKey here

    // 搜索过滤
    const filteredTasks = useMemo(() => {
        if (!searchKeyword) return myTasks;
        return myTasks.filter(task =>
            task.title.toLowerCase().includes(searchKeyword.toLowerCase())
        );
    }, [myTasks, searchKeyword]);

    const sortedTasks = useMemo(() => {
        return [...filteredTasks].sort((a, b) => new Date(a.dueDate || 0) - new Date(b.dueDate || 0));
    }, [filteredTasks]);

    return (
        <div className="mt-6 px-3">
            <div
                onClick={toggleMyTasks}
                className="flex items-center justify-between px-3 py-2 rounded-lg cursor-pointer hover:bg-gray-100 dark:hover:bg-zinc-800"
            >
                <div className="flex items-center gap-2">
                    <CheckSquareIcon className="w-4 h-4 text-gray-500 dark:text-zinc-400" />
                    <h3 className="text-sm font-medium text-gray-700 dark:text-zinc-300">{sidebarTitle}</h3>
                    <span className="bg-gray-200 dark:bg-zinc-700 text-gray-700 dark:text-zinc-300 text-xs px-2 py-0.5 rounded">
                        {filteredTasks.length}
                    </span>
                </div>
                {showMyTasks ? (
                    <ChevronDownIcon className="w-4 h-4 text-gray-500 dark:text-zinc-400" />
                ) : (
                    <ChevronRightIcon className="w-4 h-4 text-gray-500 dark:text-zinc-400" />
                )}
            </div>

            {showMyTasks && (
                <div className="mt-2 pl-2">
                    <div className="space-y-1">
                        {sortedTasks.length === 0 ? (
                            <div className="px-3 py-2 text-xs text-gray-500 dark:text-zinc-500 text-center">
                                No tasks assigned
                            </div>
                        ) : (
                            sortedTasks.map(task => {
                                const { icon: Icon, color } = typeIcons[task.type] || {};
                                const { background, prioritycolor } = priorityTexts[task.priority] || {};
                                return (
                                    <Link
                                        key={task.id}
                                        to={`/app/taskDetails?projectId=${task.projectId}&taskId=${task.id}`}
                                        className="w-full rounded-lg transition-all duration-200 text-gray-700 dark:text-zinc-300 hover:bg-gray-100 dark:hover:bg-zinc-800 hover:text-black dark:hover:text-white"
                                    >
                                        <div className="flex items-center gap-2 px-3 py-2 w-full min-w-0">
                                            <div className={`w-2 h-2 rounded-full ${getTaskStatusColor(task.status)} flex-shrink-0`} />
                                            <div className="flex-1 min-w-0">
                                                <p className="text-xs font-medium truncate flex items-center gap-1">
                                                    {Icon && <Icon className={`size-3 ${color}`} />}
                                                    {task.title}
                                                </p>
                                                <p className="text-xs text-gray-500 dark:text-zinc-500 lowercase flex gap-1">
                                                    <span className={`px-1 py-0.5 rounded text-[10px] ${background} ${prioritycolor}`}>
                                                        {task.priority}
                                                    </span>
                                                    {task.status.replace('_', ' ')}
                                                </p>
                                            </div>
                                        </div>
                                    </Link>
                                );
                            })
                        )}
                    </div>
                </div>
            )}
        </div>
    );

}


// function MyTasksSidebar({ searchKeyword }) {
//     const currentUser = useSelector(state => state.user);
//     const [showMyTasks, setShowMyTasks] = useState(false);
//     const [myTasks, setMyTasks] = useState([]);

//     const toggleMyTasks = () => setShowMyTasks(prev => !prev);

//     const getTaskStatusColor = (status) => {
//         switch (status) {
//             case 'DONE': return 'bg-green-500';
//             case 'IN_PROGRESS': return 'bg-yellow-500';
//             case 'TO_DO': return 'bg-gray-500 dark:bg-zinc-500';
//             default: return 'bg-gray-400 dark:bg-zinc-400';
//         }
//     };

//     const sidebarTitle = currentUser?.role === 'ADMIN'
//         ? 'All Tasks'
//         : currentUser?.role === 'LEADER'
//             ? 'Project Tasks'
//             : 'My Tasks';

//     // 原有获取任务逻辑
//     useEffect(() => {
//         const fetchData = async () => {
//             if (!currentUser?.userId || !currentUser.role) return;

//             try {
//                 const tasks = await getAllTasks();
//                 const projects = await getAllProjects();

//                 let visibleTasks = tasks;

//                 if (currentUser.role === "LEADER") {
//                     const usersRes = await getAllUsers();
//                     const teamIds = usersRes.data
//                         .filter(u => u.email !== currentUser.email)
//                         .map(u => u.id);

//                     const leaderProjectIds = projects
//                         .filter(p => p.leaderId === currentUser.userId || p.memberIds?.includes(currentUser.userId))
//                         .map(p => p.id);

//                     visibleTasks = tasks.filter(
//                         t =>
//                             (t.assignToUserId === currentUser.userId || teamIds.includes(t.assignToUserId)) &&
//                             leaderProjectIds.includes(t.projectId)
//                     );
//                 } else if (currentUser.role === "MEMBER") {
//                     visibleTasks = tasks.filter(t => t.assignToUserId === currentUser.userId);
//                 }

//                 setMyTasks(visibleTasks);
//             } catch (err) {
//                 console.error("Failed to fetch tasks:", err);
//                 setMyTasks([]);
//             }
//         };

//         fetchData();
//     }, [currentUser.userId, currentUser.role, currentUser.email]);

//     // 搜索过滤
//     const filteredTasks = useMemo(() => {
//         if (!searchKeyword) return myTasks;
//         return myTasks.filter(task =>
//             task.title.toLowerCase().includes(searchKeyword.toLowerCase())
//         );
//     }, [myTasks, searchKeyword]);

//     const sortedTasks = useMemo(() => {
//         return [...filteredTasks].sort((a, b) => new Date(a.dueDate || 0) - new Date(b.dueDate || 0));
//     }, [filteredTasks]);

//     return (
//         <div className="mt-6 px-3">
//             <div
//                 onClick={toggleMyTasks}
//                 className="flex items-center justify-between px-3 py-2 rounded-lg cursor-pointer hover:bg-gray-100 dark:hover:bg-zinc-800"
//             >
//                 <div className="flex items-center gap-2">
//                     <CheckSquareIcon className="w-4 h-4 text-gray-500 dark:text-zinc-400" />
//                     <h3 className="text-sm font-medium text-gray-700 dark:text-zinc-300">{sidebarTitle}</h3>
//                     <span className="bg-gray-200 dark:bg-zinc-700 text-gray-700 dark:text-zinc-300 text-xs px-2 py-0.5 rounded">
//                         {filteredTasks.length}
//                     </span>
//                 </div>
//                 {showMyTasks ? (
//                     <ChevronDownIcon className="w-4 h-4 text-gray-500 dark:text-zinc-400" />
//                 ) : (
//                     <ChevronRightIcon className="w-4 h-4 text-gray-500 dark:text-zinc-400" />
//                 )}
//             </div>

//             {showMyTasks && (
//                 <div className="mt-2 pl-2">
//                     <div className="space-y-1">
//                         {sortedTasks.length === 0 ? (
//                             <div className="px-3 py-2 text-xs text-gray-500 dark:text-zinc-500 text-center">
//                                 No tasks assigned
//                             </div>
//                         ) : (
//                             sortedTasks.map(task => {
//                                 const { icon: Icon, color } = typeIcons[task.type] || {};
//                                 const { background, prioritycolor } = priorityTexts[task.priority] || {};
//                                 return (
//                                     <Link
//                                         key={task.id}
//                                         to={`/app/taskDetails?projectId=${task.projectId}&taskId=${task.id}`}
//                                         className="w-full rounded-lg transition-all duration-200 text-gray-700 dark:text-zinc-300 hover:bg-gray-100 dark:hover:bg-zinc-800 hover:text-black dark:hover:text-white"
//                                     >
//                                         <div className="flex items-center gap-2 px-3 py-2 w-full min-w-0">
//                                             <div className={`w-2 h-2 rounded-full ${getTaskStatusColor(task.status)} flex-shrink-0`} />
//                                             <div className="flex-1 min-w-0">
//                                                 <p className="text-xs font-medium truncate flex items-center gap-1">
//                                                     {Icon && <Icon className={`size-3 ${color}`} />}
//                                                     {task.title}
//                                                 </p>
//                                                 <p className="text-xs text-gray-500 dark:text-zinc-500 lowercase flex gap-1">
//                                                     <span className={`px-1 py-0.5 rounded text-[10px] ${background} ${prioritycolor}`}>
//                                                         {task.priority}
//                                                     </span>
//                                                     {task.status.replace('_', ' ')}
//                                                 </p>
//                                             </div>
//                                         </div>
//                                     </Link>
//                                 );
//                             })
//                         )}
//                     </div>
//                 </div>
//             )}
//         </div>
//     );
// }

export default MyTasksSidebar;
