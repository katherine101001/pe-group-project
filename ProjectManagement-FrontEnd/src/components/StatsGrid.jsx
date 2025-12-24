import { FolderOpen, CheckCircle, Users, AlertTriangle } from "lucide-react";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { getDashboardStats } from "../services/Dashboard/DashboardAPI";

export default function StatsGrid() {
  const { role, userId } = useSelector((state) => state.user ?? {});
  const [stats, setStats] = useState({
    totalProjects: 0,
    activeProjects: 0,
    completedProjects: 0,
    myTasks: 0,
    overdueIssues: 0,
  });

  useEffect(() => {
    if (!role) return;

    const fetchStats = async () => {
      try {
        const data = await getDashboardStats.getStats(userId, role);
        setStats(data);
      } catch (err) {
        console.error("Failed to fetch stats:", err);
      }
    };

    fetchStats();
  }, [role, userId]);

  // Build stat cards
  const statCards = [
    // Projects stats always shown
    {
      icon: FolderOpen,
      title: "Total Projects",
      value: stats.activeProjects,
      subtitle: role === "ADMIN" ? "projects in portfolio" : "projects you are part of",
      bgColor: "bg-blue-500/10",
      textColor: "text-blue-500",
    },
    {
      icon: CheckCircle,
      title: "Completed Projects",
      value: stats.completedProjects,
      subtitle: role === "ADMIN" ? `of ${stats.totalProjects} total` : `of your ${stats.totalProjects} projects`,
      bgColor: "bg-emerald-500/10",
      textColor: "text-emerald-500",
    },
    // Task stats
    {
      icon: Users,
      title: role === "ADMIN" ? "Total Tasks" : "My Tasks",
      value: stats.myTasks,
      subtitle: role === "ADMIN" ? "all tasks" : "assigned to you",
      bgColor: "bg-purple-500/10",
      textColor: "text-purple-500",
    },
    {
      icon: AlertTriangle,
      title: "Overdue Soon",
      value: stats.overdueIssues,
      subtitle: "need attention",
      bgColor: "bg-amber-500/10",
      textColor: "text-amber-500",
    },
  ];

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 my-9">
      {statCards.map(({ icon: Icon, title, value, subtitle, bgColor, textColor }, i) => (
        <div
          key={i}
          className={`bg-white dark:bg-zinc-950 dark:bg-gradient-to-br dark:from-zinc-800/70 dark:to-zinc-900/50 border border-zinc-200 dark:border-zinc-800 hover:border-zinc-300 dark:hover:border-zinc-700 transition duration-200 rounded-md`}
        >
          <div className="p-6 py-4">
            <div className="flex items-start justify-between">
              <div>
                <p className="text-sm text-zinc-500 dark:text-zinc-400 mb-1">{title}</p>
                <p className="text-3xl font-bold text-zinc-800 dark:text-white">{value}</p>
                {subtitle && <p className="text-xs text-zinc-400 dark:text-zinc-500 mt-1">{subtitle}</p>}
              </div>
              <div className={`p-3 rounded-xl ${bgColor} bg-opacity-20`}>
                <Icon size={20} className={textColor} />
              </div>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}
