import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import ProjectAnalytics from "./ProjectAnalytics";
import { getTasksByProjectId } from "../services/ProjectTask/ProjectTaskAPI";

const ProjectAnalyticsWrapper = ({ project }) => {
    const { userId, role } = useSelector(state => state.user);
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        if (!project?.id) return;

        const fetchTasks = async () => {
            setLoading(true);
            setError(null);

            try {
                const tasksData = await getTasksByProjectId(project.id);
                console.log("All tasks fetched:", tasksData);

                let filteredTasks = tasksData;

                if (role === "MEMBER") {
                    filteredTasks = tasksData.filter(t => t.assignToUserId === userId);
                }

                setTasks(filteredTasks);
            } catch (err) {
                console.error("Failed to fetch tasks", err);
                setError("Failed to load tasks.");
            } finally {
                setLoading(false);
            }
        };

        fetchTasks();
    }, [project?.id, userId, role]);

    if (loading) return <div>Loading analytics...</div>;
    if (error) return <div>{error}</div>;

    return <ProjectAnalytics project={project} tasks={tasks} />;
};

export default ProjectAnalyticsWrapper;
