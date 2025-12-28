import { Plus } from 'lucide-react';
import { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import StatsGrid from '../components/StatsGrid';
import ProjectOverview from '../components/ProjectOverview';
import RecentActivity from '../components/RecentActivity';
import TasksSummary from '../components/TasksSummary';
import CreateProjectDialog from '../components/CreateProjectDialog';
import { getCurrentUserName } from '../services/Dashboard/DashboardAPI';
import { useOutletContext } from 'react-router-dom';


const Dashboard = () => {
    const user = useSelector((state) => state.user);
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [userName, setUserName] = useState('User');
    // const [refreshKey, setRefreshKey] = useState(0);
    const { refreshKey, handleRefresh } = useOutletContext();


    // ✅ 关键修复在这里
    useEffect(() => {
        if (user?.userId) { // ❗ 改这里
            getCurrentUserName(user.userId).then(name => { // ❗ 改这里
                if (name) setUserName(name);
            });
        }
    }, [user.userId]); // ❗ dependency 也顺手修一下

    const handleProjectCreated = () => {
        setRefreshKey(prev => prev + 1);
    };


    const canCreateProject = ["ADMIN", "LEADER"].includes(user.role);

    return (
        <div className='max-w-6xl mx-auto'>
            <div className="flex flex-col lg:flex-row justify-between items-start lg:items-center gap-6 ">
                <div>
                    <h1 className="text-xl sm:text-2xl font-semibold text-gray-900 dark:text-white mb-1">
                        Welcome back, {userName}
                    </h1>
                    <p className="text-gray-500 dark:text-zinc-400 text-sm">
                        Here's what's happening with your projects today
                    </p>
                </div>

                {canCreateProject && (
                    <button
                        onClick={() => setIsDialogOpen(true)}
                        className="flex items-center gap-2 px-5 py-2 text-sm rounded bg-gradient-to-br from-blue-500 to-blue-600 text-white hover:opacity-90 transition"
                    >
                        <Plus size={16} /> New Project
                    </button>
                )}

                <CreateProjectDialog
                    isDialogOpen={isDialogOpen}
                    setIsDialogOpen={setIsDialogOpen}
                    onProjectCreated={handleRefresh}
                />
            </div>

            <StatsGrid refreshKey={refreshKey} />

            <div className="grid lg:grid-cols-3 gap-8">
                <div className="lg:col-span-2 space-y-8">
                    <ProjectOverview userId={user.userId} refreshKey={refreshKey} />
                    <RecentActivity userId={user.userId} refreshKey={refreshKey} />
                </div>
                <div>
                    <TasksSummary />
                    {/* <TasksSummary userId={user.userId} refreshKey={refreshKey} /> */}

                </div>
            </div>
        </div>
    );
};

export default Dashboard;
