import { useEffect, useState } from "react";
import axios from "axios";
import ProjectAnalytics from "./ProjectAnalytics";

const ProjectAnalyticsWrapper = ({ project }) => {
    const [analytics, setAnalytics] = useState(null);
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        if (!project?.id) return;

        const fetchAnalytics = async () => {
            setLoading(true);
            setError(null);
            try {
                // Fetch project analytics
                const analyticsRes = await axios.get(
                    `/api/projects/${project.id}/analytics/overview`
                );
                setAnalytics(analyticsRes.data);

                // Fetch tasks (if your ProjectAnalytics needs full task data)
                const tasksRes = await axios.get(`/api/projects/${project.id}/tasks`);
                setTasks(tasksRes.data);
            } catch (err) {
                console.error("Failed to fetch project analytics", err);
                setError("Failed to load analytics.");
            } finally {
                setLoading(false);
            }
        };

        fetchAnalytics();
    }, [project?.id]);

    if (loading) return <div>Loading analytics...</div>;
    if (error) return <div>{error}</div>;

    return <ProjectAnalytics project={project} tasks={tasks} />;
};

export default ProjectAnalyticsWrapper;


//replace <ProjectAnalytics project={project} tasks={tasks} />
//with
//<ProjectAnalyticsWrapper project={project} />