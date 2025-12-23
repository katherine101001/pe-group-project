// import { format } from "date-fns";
// import toast from "react-hot-toast";
// import { useEffect, useState } from "react";
// import { useSearchParams } from "react-router-dom";
// import { CalendarIcon, MessageCircle, PenIcon } from "lucide-react";
// import { getTaskDetailsById } from "../services/ProjectTask/ProjectTaskAPI";
// import { getProjectById } from "../services/Project/ProjectAPI";
// import { getCommentsByTaskId, addCommentToTask } from "../services/Comment/CommentAPI";

// const TaskDetails = () => {
//     const [searchParams] = useSearchParams();
//     const taskId = searchParams.get("taskId");
//     const projectId = searchParams.get("projectId");

//     const user = { id: "72da7c5e-a07c-4e30-b59c-a6a4fc164f58", name: "Orange", image: assets.profile_img_a };
//     const [task, setTask] = useState(null);
//     const [project, setProject] = useState(null);
//     const [comments, setComments] = useState([]);
//     const [newComment, setNewComment] = useState("");
//     const [loading, setLoading] = useState(true);

//     const fetchAllData = async () => {
//         setLoading(true);
//         try {
//             const t = await getTaskDetailsById(taskId);
//             setTask(t);

//             const p = await getProjectById(projectId || t.projectId);
//             setProject(p);

//             const c = await getCommentsByTaskId(taskId);
//             setComments(c);
//         } catch (err) {
//             console.error("Failed to fetch task/project/comments", err);
//         } finally {
//             setLoading(false);
//         }
//     };

//     useEffect(() => {
//         if (taskId) fetchAllData();
//     }, [taskId]);

//     const handleAddComment = async () => {
//         if (!newComment.trim()) return;
//         const added = await addCommentToTask(taskId, user.id, newComment.trim());
//         if (added) {
//             setComments(prev => [...prev, added]);
//             setNewComment("");
//             toast.success("Comment added");
//         }
//     };

//     if (loading) return <div className="text-gray-500 dark:text-zinc-400 px-4 py-6">Loading task details...</div>;
//     if (!task) return <div className="text-red-500 px-4 py-6">Task not found.</div>;

//     return (
//         <div className="flex flex-col-reverse lg:flex-row gap-6 sm:p-4 text-gray-900 dark:text-zinc-100 max-w-6xl mx-auto">
//             {/* Left: Comments / Chatbox */}
//             <div className="w-full lg:w-2/3">
//                 <div className="p-5 rounded-md border border-gray-300 dark:border-zinc-800 flex flex-col lg:h-[80vh]">
//                     <h2 className="text-base font-semibold flex items-center gap-2 mb-4 text-gray-900 dark:text-white">
//                         <MessageCircle className="size-5" /> Task Discussion ({comments.length})
//                     </h2>

//                     <div className="flex-1 md:overflow-y-scroll no-scrollbar">
//                         {comments.length > 0 ? (
//                             <div className="flex flex-col gap-4 mb-6 mr-2">
//                                 {comments.map(comment => (
//                                     <div key={comment.id} className="sm:max-w-4/5 dark:bg-gradient-to-br dark:from-zinc-800 dark:to-zinc-900 border border-gray-300 dark:border-zinc-700 p-3 rounded-md">

//                                         <div className="flex items-center gap-2 mb-1 text-sm text-gray-500 dark:text-zinc-400">
//                                             <img src={user.image} alt="avatar" className="size-5 rounded-full" />
//                                             <span className="font-medium text-gray-900 dark:text-white">{comment.userName}</span>
//                                             <span className="text-xs text-gray-400 dark:text-zinc-600">
//                                                 • {format(new Date(comment.createdAt), "dd MMM yyyy, HH:mm")}
//                                             </span>
//                                         </div>
//                                         <p className="text-sm text-gray-900 dark:text-zinc-200">{comment.content}</p>
//                                     </div>
//                                 ))}
//                             </div>
//                         ) : (
//                             <p className="text-gray-600 dark:text-zinc-500 mb-4 text-sm">No comments yet. Be the first!</p>
//                         )}
//                     </div>

//                     {/* Add Comment */}
//                     <div className="flex flex-col sm:flex-row items-start sm:items-end gap-3">
//                         <textarea
//                             value={newComment}
//                             onChange={(e) => setNewComment(e.target.value)}
//                             placeholder="Write a comment..."
//                             className="w-full dark:bg-zinc-800 border border-gray-300 dark:border-zinc-700 rounded-md p-2 text-sm text-gray-900 dark:text-zinc-200 resize-none focus:outline-none focus:ring-1 focus:ring-blue-600"
//                             rows={3}
//                         />
//                         <button onClick={handleAddComment} className="bg-gradient-to-l from-blue-500 to-blue-600 transition-colors text-white text-sm px-5 py-2 rounded" >
//                             Post
//                         </button>
//                     </div>
//                 </div>
//             </div>

//             {/* Right: Task + Project Info */}
//             <div className="w-full lg:w-1/2 flex flex-col gap-6">
//                 {/* Task Info */}
//                 <div className="p-5 rounded-md bg-white dark:bg-zinc-900 border border-gray-300 dark:border-zinc-800">
//                     <div className="mb-3">
//                         <h1 className="text-lg font-medium text-gray-900 dark:text-zinc-100">{task.title}</h1>
//                         <div className="flex flex-wrap gap-2 mt-2">
//                             <span className="px-2 py-0.5 rounded bg-zinc-200 dark:bg-zinc-700 text-zinc-900 dark:text-zinc-300 text-xs">{task.status}</span>
//                             <span className="px-2 py-0.5 rounded bg-blue-200 dark:bg-blue-900 text-blue-900 dark:text-blue-300 text-xs">{task.type}</span>
//                             <span className="px-2 py-0.5 rounded bg-green-200 dark:bg-emerald-900 text-green-900 dark:text-emerald-300 text-xs">{task.priority}</span>
//                         </div>
//                     </div>

//                     {task.description && (
//                         <p className="text-sm text-gray-600 dark:text-zinc-400 leading-relaxed mb-4">{task.description}</p>
//                     )}

//                     <hr className="border-zinc-200 dark:border-zinc-700 my-3" />

//                     <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 text-sm text-gray-700 dark:text-zinc-300">
//                         <div className="flex items-center gap-2">
//                             <img src={task.assignee?.image} className="size-5 rounded-full" alt="avatar" />
//                             {task.assignee?.name || "Unassigned"}
//                         </div>
//                         <div className="flex items-center gap-2">
//                             <CalendarIcon className="size-4 text-gray-500 dark:text-zinc-500" />
//                             Due : {task.dueDate ? format(new Date(task.dueDate), "dd MMM yyyy") : "-"}
//                         </div>
//                     </div>
//                 </div>

//                 {/* Project Info */}
//                 {project && (
//                     <div className="p-4 rounded-md bg-white dark:bg-zinc-900 text-zinc-700 dark:text-zinc-200 border border-gray-300 dark:border-zinc-800">
//                         <p className="text-xl font-medium mb-4">Project Details</p>
//                         <h2 className="text-gray-900 dark:text-zinc-100 flex items-center gap-2"><PenIcon className="size-4" /> {project.title}</h2>
//                         <p className="text-xs mt-3">Project Start Date: {project.startDate ? format(new Date(project.startDate), "dd MMM yyyy") : "-"}</p>
//                         <div className="flex flex-wrap gap-4 text-sm text-gray-500 dark:text-zinc-400 mt-3">
//                             <span>Status: {project.status}</span>
//                             <span>Priority: {project.priority || "N/A"}</span>
//                             <span>Progress: {project.totalTasks > 0 ? Math.floor((project.completedTasks / project.totalTasks) * 100) : 0}%</span>
//                         </div>
//                     </div>
//                 )}
//             </div>
//         </div>
//     );
// };

// export default TaskDetails;

import { format } from "date-fns";
import toast from "react-hot-toast";
import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { CalendarIcon, MessageCircle, PenIcon } from "lucide-react";
import { getTaskDetailsById } from "../services/ProjectTask/ProjectTaskAPI";
import { getProjectById } from "../services/Project/ProjectAPI";
import { getCommentsByTaskId, addCommentToTask } from "../services/Comment/CommentAPI";

const TaskDetails = () => {
  const [searchParams] = useSearchParams();
  const taskId = searchParams.get("taskId");
  const projectId = searchParams.get("projectId");

  const [task, setTask] = useState(null);
  const [project, setProject] = useState(null);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState("");
  const [loading, setLoading] = useState(true);

  const user = {
    id: "72da7c5e-a07c-4e30-b59c-a6a4fc164f58",
    name: "Orange",
  };

  const fetchAllData = async () => {
    setLoading(true);
    try {
      const t = await getTaskDetailsById(taskId);
      if (!t) throw new Error("Task not found");

      const mappedTask = {
        ...t,
        title: t.title || "Untitled Task",
        description: t.description || "",
        dueDate: t.dueDate || t.due_date || null,
        status: t.status || "TO_DO",
        type: t.type || "TASK",
        priority: t.priority || "MEDIUM",
        assignee: {
          name: t.assigneeName || "Unassigned",
        },
      };
      setTask(mappedTask);

      const p = await getProjectById(projectId || t.projectId);
      setProject({
        ...p,
        title: p.title || "Untitled Project",
        status: p.status || "PLANNING",
        priority: p.priority || "MEDIUM",
        startDate: p.startDate || null,
        endDate: p.endDate || null,
        totalTasks: p.totalTasks || 0,
        completedTasks: p.completedTasks || 0,
      });

      const c = await getCommentsByTaskId(taskId);
      setComments(
        c?.map(comment => ({
          ...comment,
          userName: comment.userName || "Unknown",
          content: comment.content || "",
          createdAt: comment.createdAt || new Date().toISOString(),
        })) || []
      );
    } catch (err) {
      console.error("Failed to fetch task/project/comments", err);
      toast.error("Failed to load task details");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (taskId) fetchAllData();
  }, [taskId]);

  const handleAddComment = async () => {
    if (!newComment.trim()) return;
    try {
      const added = await addCommentToTask(taskId, user.id, newComment.trim());
      if (added) {
        setComments(prev => [
          ...prev,
          {
            id: added.id || Date.now(),
            userName: user.name,
            content: added.content,
            createdAt: added.createdAt || new Date().toISOString(),
          },
        ]);
        setNewComment("");
        toast.success("Comment added");
      }
    } catch (err) {
      console.error("Failed to add comment", err);
      toast.error("Failed to add comment");
    }
  };

  if (loading)
    return (
      <div className="text-gray-500 dark:text-zinc-400 px-4 py-6">
        Loading task details...
      </div>
    );

  if (!task)
    return (
      <div className="text-red-500 px-4 py-6">
        Task not found.
      </div>
    );

  return (
    <div className="flex flex-col-reverse lg:flex-row gap-6 sm:p-4 text-gray-900 dark:text-zinc-100 max-w-6xl mx-auto">
      {/* Left: Comments */}
      <div className="w-full lg:w-2/3">
        <div className="p-5 rounded-md border border-gray-300 dark:border-zinc-800 flex flex-col lg:h-[80vh]">
          <h2 className="text-base font-semibold flex items-center gap-2 mb-4 text-gray-900 dark:text-white">
            <MessageCircle className="size-5" /> Task Discussion ({comments.length})
          </h2>

          <div className="flex-1 md:overflow-y-scroll no-scrollbar">
            {comments.length > 0 ? (
              <div className="flex flex-col gap-4 mb-6 mr-2">
                {comments.map(comment => (
                  <div
                    key={comment.id}
                    className="sm:max-w-4/5 dark:bg-gradient-to-br dark:from-zinc-800 dark:to-zinc-900 border border-gray-300 dark:border-zinc-700 p-3 rounded-md"
                  >
                    <div className="flex items-center gap-2 mb-1 text-sm text-gray-500 dark:text-zinc-400">
                      <span className="font-medium text-gray-900 dark:text-white">
                        {comment.userName}
                      </span>
                      <span className="text-xs text-gray-400 dark:text-zinc-600">
                        • {format(new Date(comment.createdAt), "dd MMM yyyy, HH:mm")}
                      </span>
                    </div>
                    <p className="text-sm text-gray-900 dark:text-zinc-200">{comment.content}</p>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-gray-600 dark:text-zinc-500 mb-4 text-sm">
                No comments yet. Be the first!
              </p>
            )}
          </div>

          {/* Add Comment */}
          <div className="flex flex-col sm:flex-row items-start sm:items-end gap-3">
            <textarea
              value={newComment}
              onChange={(e) => setNewComment(e.target.value)}
              placeholder="Write a comment..."
              className="w-full dark:bg-zinc-800 border border-gray-300 dark:border-zinc-700 rounded-md p-2 text-sm text-gray-900 dark:text-zinc-200 resize-none focus:outline-none focus:ring-1 focus:ring-blue-600"
              rows={3}
            />
            <button
              onClick={handleAddComment}
              className="bg-gradient-to-l from-blue-500 to-blue-600 transition-colors text-white text-sm px-5 py-2 rounded"
            >
              Post
            </button>
          </div>
        </div>
      </div>

      {/* Right: Task + Project Info */}
      <div className="w-full lg:w-1/2 flex flex-col gap-6">
        {/* Task Info */}
        <div className="p-5 rounded-md bg-white dark:bg-zinc-900 border border-gray-300 dark:border-zinc-800">
          <div className="mb-3">
            <h1 className="text-lg font-medium text-gray-900 dark:text-zinc-100">
              {task.title}
            </h1>
            <div className="flex flex-wrap gap-2 mt-2">
              <span className="px-2 py-0.5 rounded bg-zinc-200 dark:bg-zinc-700 text-zinc-900 dark:text-zinc-300 text-xs">
                {task.status}
              </span>
              <span className="px-2 py-0.5 rounded bg-blue-200 dark:bg-blue-900 text-blue-900 dark:text-blue-300 text-xs">
                {task.type}
              </span>
              <span className="px-2 py-0.5 rounded bg-green-200 dark:bg-emerald-900 text-green-900 dark:text-emerald-300 text-xs">
                {task.priority}
              </span>
            </div>
          </div>

          {task.description && (
            <p className="text-sm text-gray-600 dark:text-zinc-400 leading-relaxed mb-4">
              {task.description}
            </p>
          )}

          <hr className="border-zinc-200 dark:border-zinc-700 my-3" />

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 text-sm text-gray-700 dark:text-zinc-300">
            <div className="flex items-center gap-2">
              {task.assignee?.name || "Unassigned"}
            </div>
            <div className="flex items-center gap-2">
              <CalendarIcon className="size-4 text-gray-500 dark:text-zinc-500" />
              Due : {task.dueDate ? format(new Date(task.dueDate), "dd MMM yyyy") : "-"}
            </div>
          </div>
        </div>

        {/* Project Info */}
        {project && (
          <div className="p-4 rounded-md bg-white dark:bg-zinc-900 text-zinc-700 dark:text-zinc-200 border border-gray-300 dark:border-zinc-800">
            <p className="text-xl font-medium mb-4">Project Details</p>
            <h2 className="text-gray-900 dark:text-zinc-100 flex items-center gap-2">
              <PenIcon className="size-4" /> {project.title}
            </h2>
            <p className="text-xs mt-3">
              Project Start Date: {project.startDate ? format(new Date(project.startDate), "dd MMM yyyy") : "-"}
            </p>
            <div className="flex flex-wrap gap-4 text-sm text-gray-500 dark:text-zinc-400 mt-3">
              <span>Status: {project.status}</span>
              <span>Priority: {project.priority}</span>
              <span>
                Progress: {project.totalTasks > 0 ? Math.floor((project.completedTasks / project.totalTasks) * 100) : 0}%
              </span>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default TaskDetails;
