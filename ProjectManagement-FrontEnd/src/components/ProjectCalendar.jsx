// import { useState, useEffect, useMemo } from "react";
// import { format, isSameDay, eachDayOfInterval, startOfMonth, endOfMonth, addMonths, subMonths } from "date-fns";
// import { useSelector } from "react-redux";
// import { CalendarIcon, Clock, User, ChevronLeft, ChevronRight } from "lucide-react";
// import { getTaskCalendarByMonth, getTasksByProjectId } from "../services/ProjectTask/ProjectTaskAPI";

// const typeColors = {
//   BUG: "bg-red-200 text-red-800 dark:bg-red-500 dark:text-red-900",
//   FEATURE: "bg-blue-200 text-blue-800 dark:bg-blue-500 dark:text-blue-900",
//   TASK: "bg-green-200 text-green-800 dark:bg-green-500 dark:text-green-900",
//   IMPROVEMENT: "bg-purple-200 text-purple-800 dark:bg-purple-500 dark:text-purple-900",
//   OTHER: "bg-amber-200 text-amber-800 dark:bg-amber-500 dark:text-amber-900",
// };

// const ProjectCalendar = ({ project }) => {
//   const projectId = project?.id;

//   if (!projectId) {
//     return <div className="text-red-600 font-bold">ProjectCalendar: projectId missing</div>;
//   }

//   const { role, id: userId, email: userEmail } = useSelector((state) => state.user ?? {});
//   const [selectedDate, setSelectedDate] = useState(new Date());
//   const [currentMonth, setCurrentMonth] = useState(new Date());
//   const [tasks, setTasks] = useState([]);
//   const [overdueTasks, setOverdueTasks] = useState([]);
//   const [loading, setLoading] = useState(false);

//   useEffect(() => {
//     const fetchData = async () => {
//       setLoading(true);
//       try {
//         const year = currentMonth.getFullYear();
//         const month = currentMonth.getMonth() + 1;

//         // 日历任务
//         const calRes = await getTaskCalendarByMonth(projectId, year, month);
//         setTasks(calRes);

//         // 全量任务
//         const allTasks = await getTasksByProjectId(projectId);
//         const today = new Date();
//         const filteredOverdue = allTasks.filter(t => t.due_date < today);
//         setOverdueTasks(filteredOverdue);
//       } catch (err) {
//         console.error("Failed to fetch tasks:", err);
//       } finally {
//         setLoading(false);
//       }
//     };

//     fetchData();
//   }, [projectId, currentMonth]);

//   // 过滤日历任务，根据角色和分配
//   const filteredTasks = useMemo(() => {
//     return tasks.map(day => {
//       const filteredDayTasks = day.tasks.filter(task => 
//         role === "LEADER" || role === "ADMIN" || task.assignToUserId === userId || task.assigneeEmail === userEmail
//       );
//       return {
//         ...day,
//         tasks: filteredDayTasks,
//         taskCount: filteredDayTasks.length,
//       };
//     });
//   }, [tasks, role, userId, userEmail]);

//   const filteredOverdueTasks = useMemo(() => {
//     return overdueTasks.filter(task =>
//       role === "LEADER" || role === "ADMIN" || task.assignToUserId === userId || task.assigneeEmail === userEmail
//     );
//   }, [overdueTasks, role, userId, userEmail]);

//   const getTasksForDate = (date) => {
//     const day = filteredTasks.find(d => isSameDay(d.due_date, date));
//     return day ? day.tasks : [];
//   };

//   const daysInMonth = eachDayOfInterval({
//     start: startOfMonth(currentMonth),
//     end: endOfMonth(currentMonth),
//   });

//   const handleMonthChange = (dir) => {
//     setCurrentMonth(prev => (dir === "next" ? addMonths(prev, 1) : subMonths(prev, 1)));
//   };

//   return (
//     <div className="grid lg:grid-cols-3 gap-6">
//       {/* Calendar */}
//       <div className="lg:col-span-2">
//         <div className="bg-white dark:bg-zinc-900 border rounded-lg p-4">
//           <div className="flex justify-between items-center mb-4">
//             <h2 className="text-lg text-zinc-900 dark:text-white flex items-center gap-2">
//               <CalendarIcon /> Task Calendar
//             </h2>
//             <div className="flex items-center gap-2">
//               <button onClick={() => handleMonthChange("prev")}><ChevronLeft /></button>
//               <span>{format(currentMonth, "MMMM yyyy")}</span>
//               <button onClick={() => handleMonthChange("next")}><ChevronRight /></button>
//             </div>
//           </div>

//           {/* Week Days */}
//           <div className="grid grid-cols-7 text-xs text-center text-zinc-500 dark:text-zinc-400 mb-2">
//             {["Sun","Mon","Tue","Wed","Thu","Fri","Sat"].map(d => <div key={d}>{d}</div>)}
//           </div>

//           {/* Days */}
//           <div className="grid grid-cols-7 gap-2">
//             {daysInMonth.map(day => {
//               const dayTasks = getTasksForDate(day);
//               const isSelected = isSameDay(day, selectedDate);
//               return (
//                 <button
//                   key={day.toISOString()}
//                   onClick={() => setSelectedDate(day)}
//                   className={`h-14 rounded-md flex flex-col items-center justify-center text-sm
//                     ${isSelected ? "bg-blue-200 dark:bg-blue-600 text-blue-900 dark:text-white" : "bg-zinc-50 dark:bg-zinc-800/40 text-zinc-900 dark:text-zinc-300 hover:bg-zinc-100 dark:hover:bg-zinc-700"}`}>
//                   <span>{format(day,"d")}</span>
//                   {dayTasks.length > 0 && <span className="text-xs text-blue-700 dark:text-blue-400">{dayTasks.length} tasks</span>}
//                 </button>
//               );
//             })}
//           </div>
//         </div>

//         {/* Tasks for Selected Day */}
//         {getTasksForDate(selectedDate).length > 0 && (
//           <div className="bg-white dark:bg-zinc-900 mt-4 p-4 rounded-lg border">
//             <h3 className="text-lg text-zinc-900 dark:text-white mb-2">
//               Tasks for {format(selectedDate,"MMM d, yyyy")}
//             </h3>
//             <div className="space-y-2">
//               {getTasksForDate(selectedDate).map(task => (
//                 <div key={task.id} className="p-3 rounded border-l-4 border-zinc-300 bg-zinc-50 dark:bg-zinc-800/40">
//                   <div className="flex justify-between">
//                     <span className="font-medium text-zinc-900 dark:text-white">{task.title}</span>
//                     <span className={`text-xs px-2 py-0.5 rounded ${typeColors[task.type]}`}>{task.type}</span>
//                   </div>
//                   <div className="text-xs text-zinc-500 dark:text-zinc-400 flex justify-between mt-1">
//                     <span>{task.status}</span>
//                     {task.assigneeName && <span className="flex items-center gap-1"><User className="w-3 h-3"/>{task.assigneeName}</span>}
//                   </div>
//                 </div>
//               ))}
//             </div>
//           </div>
//         )}
//       </div>

//       {/* Sidebar: Overdue */}
//       <div className="space-y-4">
//         <div className="bg-white dark:bg-zinc-950 p-4 rounded-lg border border-red-300 dark:border-red-500">
//           <h3 className="text-sm text-red-700 dark:text-red-400 flex items-center gap-2 mb-2">
//             <Clock className="w-4 h-4"/> Overdue Tasks ({filteredOverdueTasks.length})
//           </h3>
//           {filteredOverdueTasks.length === 0 ? (
//             <p className="text-xs text-zinc-500 dark:text-zinc-400 text-center">No overdue tasks</p>
//           ) : (
//             <div className="space-y-2">
//               {filteredOverdueTasks.map(task => (
//                 <div key={task.id} className="p-2 bg-red-50 dark:bg-red-900/20 rounded transition">
//                   <div className="flex justify-between text-sm text-zinc-900 dark:text-white">
//                     <span>{task.title}</span>
//                     <span className={`text-xs px-1 py-0.5 rounded ${typeColors[task.type]}`}>{task.type}</span>
//                   </div>
//                   <p className="text-xs text-red-600 dark:text-red-300">Due {format(task.due_date,"MMM d")}</p>
//                 </div>
//               ))}
//             </div>
//           )}
//         </div>
//       </div>
//     </div>
//   );
// };

// export default ProjectCalendar;


import { useState, useEffect, useMemo } from "react";
import { format, isSameDay, eachDayOfInterval, startOfMonth, endOfMonth, addMonths, subMonths } from "date-fns";
import { useSelector } from "react-redux";
import { CalendarIcon, Clock, User, ChevronLeft, ChevronRight } from "lucide-react";
import { getTaskCalendarByMonth, getTasksByProjectId } from "../services/ProjectTask/ProjectTaskAPI";

const typeColors = {
  BUG: "bg-red-200 text-red-800 dark:bg-red-500 dark:text-red-900",
  FEATURE: "bg-blue-200 text-blue-800 dark:bg-blue-500 dark:text-blue-900",
  TASK: "bg-green-200 text-green-800 dark:bg-green-500 dark:text-green-900",
  IMPROVEMENT: "bg-purple-200 text-purple-800 dark:bg-purple-500 dark:text-purple-900",
  OTHER: "bg-amber-200 text-amber-800 dark:bg-amber-500 dark:text-amber-900",
};

const ProjectCalendar = ({ project }) => {
  const projectId = project?.id;

  if (!projectId) {
    return <div className="text-red-600 font-bold">ProjectCalendar: projectId missing</div>;
  }

  const { role, id: userId, email: userEmail } = useSelector((state) => state.user ?? {});
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const [tasks, setTasks] = useState([]);
  const [overdueTasks, setOverdueTasks] = useState([]);
  const [upcomingTasks, setUpcomingTasks] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const year = currentMonth.getFullYear();
        const month = currentMonth.getMonth() + 1;

        // Calendar tasks
        const calRes = await getTaskCalendarByMonth(projectId, year, month);
        setTasks(calRes);

        // All tasks
        const allTasksRaw = await getTasksByProjectId(projectId);
        const allTasks = allTasksRaw.map(t => ({ ...t, due_date: new Date(t.dueDate || t.due_date) }));
        const today = new Date();

        // Overdue
        const filteredOverdue = allTasks.filter(t => t.due_date < today && t.status !== "DONE");
        setOverdueTasks(filteredOverdue);

        // Upcoming (today or later, not DONE)
        const filteredUpcoming = allTasks.filter(t => t.due_date >= today && t.status !== "DONE");
        setUpcomingTasks(filteredUpcoming);

      } catch (err) {
        console.error("Failed to fetch tasks:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [projectId, currentMonth]);

  // Filtered tasks for calendar display
  const filteredTasks = useMemo(() => {
    return tasks.map(day => {
      const filteredDayTasks = day.tasks.filter(task =>
        role === "LEADER" || role === "ADMIN" || task.assignToUserId === userId || task.assigneeEmail === userEmail
      );
      return { ...day, tasks: filteredDayTasks, taskCount: filteredDayTasks.length };
    });
  }, [tasks, role, userId, userEmail]);

  const filteredOverdueTasks = useMemo(() => {
    return overdueTasks.filter(task =>
      role === "LEADER" || role === "ADMIN" || task.assignToUserId === userId || task.assigneeEmail === userEmail
    );
  }, [overdueTasks, role, userId, userEmail]);

  const filteredUpcomingTasks = useMemo(() => {
    return upcomingTasks.filter(task =>
      role === "LEADER" || role === "ADMIN" || task.assignToUserId === userId || task.assigneeEmail === userEmail
    );
  }, [upcomingTasks, role, userId, userEmail]);

  const getTasksForDate = (date) => {
    const day = filteredTasks.find(d => isSameDay(d.due_date, date));
    return day ? day.tasks : [];
  };

  const daysInMonth = eachDayOfInterval({
    start: startOfMonth(currentMonth),
    end: endOfMonth(currentMonth),
  });

  const handleMonthChange = (dir) => {
    setCurrentMonth(prev => (dir === "next" ? addMonths(prev, 1) : subMonths(prev, 1)));
  };

  return (
    <div className="grid lg:grid-cols-3 gap-6">
      {/* Calendar */}
      <div className="lg:col-span-2">
        <div className="bg-white dark:bg-zinc-900 border rounded-lg p-4">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg text-zinc-900 dark:text-white flex items-center gap-2">
              <CalendarIcon /> Task Calendar
            </h2>
            <div className="flex items-center gap-2">
              <button onClick={() => handleMonthChange("prev")}><ChevronLeft /></button>
              <span>{format(currentMonth, "MMMM yyyy")}</span>
              <button onClick={() => handleMonthChange("next")}><ChevronRight /></button>
            </div>
          </div>

          {/* Week Days */}
          <div className="grid grid-cols-7 text-xs text-center text-zinc-500 dark:text-zinc-400 mb-2">
            {["Sun","Mon","Tue","Wed","Thu","Fri","Sat"].map(d => <div key={d}>{d}</div>)}
          </div>

          {/* Days */}
          <div className="grid grid-cols-7 gap-2">
            {daysInMonth.map(day => {
              const dayTasks = getTasksForDate(day);
              const isSelected = isSameDay(day, selectedDate);
              return (
                <button
                  key={day.toISOString()}
                  onClick={() => setSelectedDate(day)}
                  className={`h-14 rounded-md flex flex-col items-center justify-center text-sm
                    ${isSelected ? "bg-blue-200 dark:bg-blue-600 text-blue-900 dark:text-white" : "bg-zinc-50 dark:bg-zinc-800/40 text-zinc-900 dark:text-zinc-300 hover:bg-zinc-100 dark:hover:bg-zinc-700"}`}>
                  <span>{format(day,"d")}</span>
                  {dayTasks.length > 0 && <span className="text-xs text-blue-700 dark:text-blue-400">{dayTasks.length} tasks</span>}
                </button>
              );
            })}
          </div>
        </div>

        {/* Tasks for Selected Day */}
        {getTasksForDate(selectedDate).length > 0 && (
          <div className="bg-white dark:bg-zinc-900 mt-4 p-4 rounded-lg border">
            <h3 className="text-lg text-zinc-900 dark:text-white mb-2">
              Tasks for {format(selectedDate,"MMM d, yyyy")}
            </h3>
            <div className="space-y-2">
              {getTasksForDate(selectedDate).map(task => (
                <div key={task.id} className="p-3 rounded border-l-4 border-zinc-300 bg-zinc-50 dark:bg-zinc-800/40">
                  <div className="flex justify-between">
                    <span className="font-medium text-zinc-900 dark:text-white">{task.title}</span>
                    <span className={`text-xs px-2 py-0.5 rounded ${typeColors[task.type]}`}>{task.type}</span>
                  </div>
                  <div className="text-xs text-zinc-500 dark:text-zinc-400 flex justify-between mt-1">
                    <span>{task.status}</span>
                    {task.assigneeName && <span className="flex items-center gap-1"><User className="w-3 h-3"/>{task.assigneeName}</span>}
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>

      {/* Sidebar: Overdue + Upcoming */}
      <div className="space-y-4">
        {/* Overdue */}
        <div className="bg-white dark:bg-zinc-950 p-4 rounded-lg border border-red-300 dark:border-red-500">
          <h3 className="text-sm text-red-700 dark:text-red-400 flex items-center gap-2 mb-2">
            <Clock className="w-4 h-4"/> Overdue Tasks ({filteredOverdueTasks.length})
          </h3>
          {filteredOverdueTasks.length === 0 ? (
            <p className="text-xs text-zinc-500 dark:text-zinc-400 text-center">No overdue tasks</p>
          ) : (
            <div className="space-y-2">
              {filteredOverdueTasks.map(task => (
                <div key={task.id} className="p-2 bg-red-50 dark:bg-red-900/20 rounded transition">
                  <div className="flex justify-between text-sm text-zinc-900 dark:text-white">
                    <span>{task.title}</span>
                    <span className={`text-xs px-1 py-0.5 rounded ${typeColors[task.type]}`}>{task.type}</span>
                  </div>
                  <p className="text-xs text-red-600 dark:text-red-300">Due {format(task.due_date,"MMM d")}</p>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Upcoming */}
        <div className="bg-white dark:bg-zinc-950 p-4 rounded-lg border border-blue-300 dark:border-blue-500">
          <h3 className="text-sm text-blue-700 dark:text-blue-400 flex items-center gap-2 mb-2">
            <Clock className="w-4 h-4"/> Upcoming Tasks ({filteredUpcomingTasks.length})
          </h3>
          {filteredUpcomingTasks.length === 0 ? (
            <p className="text-xs text-zinc-500 dark:text-zinc-400 text-center">No upcoming tasks</p>
          ) : (
            <div className="space-y-2">
              {filteredUpcomingTasks.map(task => (
                <div key={task.id} className="p-2 bg-blue-50 dark:bg-blue-900/20 rounded transition">
                  <div className="flex justify-between text-sm text-zinc-900 dark:text-white">
                    <span>{task.title}</span>
                    <span className={`text-xs px-1 py-0.5 rounded ${typeColors[task.type]}`}>{task.type}</span>
                  </div>
                  <p className="text-xs text-blue-600 dark:text-blue-300">Due {format(task.due_date,"MMM d")}</p>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProjectCalendar;
