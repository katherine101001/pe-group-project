import { API } from '../api';

export const createTask = async (task) => {
    try {
        const res = await API.post("/tasks", task);
        return res.data;
    } catch (error) {
        console.error("Failed to create task", error);
        throw error;
    }
};


export const getAllTasks = async () => {
    try {
      const res = await API.get("/tasks");
      return res.data; 
    } catch (error) {
      console.error("Failed to fetch tasks", error);
      return [];
    }
  };

export const getTaskById = async (taskId) => {
    try {
      const res = await API.get(`/tasks/${taskId}`);
      return res.data;
    } catch (error) {
      console.error("Failed to fetch task", error);
      return null;
    }
  };
  
  // 详细信息 API（包含 description）
  export const getTaskDetailsById = async (taskId) => {
    try {
      const res = await API.get(`/tasks/details/${taskId}`);
      return res.data;
    } catch (error) {
      console.error("Failed to fetch task details", error);
      return null;
    }
  };
  
  export const getOverdueTasksByProject = async (projectId) => {
    try {
      const res = await API.get(`/tasks/overdue/${projectId}`);

      return res.data.map(task => ({
        ...task,
        due_date: new Date(task.dueDate || task.due_date),
        assignee: { name: task.assigneeName || "Unassigned" }, 
      }));
    } catch (error) {
      console.error("Failed to fetch overdue tasks", error);
      return [];
    }
  };
    export const getTaskCalendarByMonth = async (projectId, year, month) => {
      try {
        const res = await API.get(
          `/tasks/calendar?projectId=${projectId}&year=${year}&month=${month}`
        );
        return res.data.map(day => ({
          ...day,
          due_date: new Date(day.date), // make sure it's a proper Date object
          tasks: day.tasks || [],
        }));
      } catch (error) {
        console.error("Failed to fetch calendar tasks", error);
        return [];
      }
    };
    

  export const getTasksByProjectId = async (projectId) => {
    try {
        const res = await API.get(`/tasks/project/${projectId}`); 
        return res.data.map(task => ({
            ...task,
            due_date: new Date(task.dueDate || task.due_date),
            assignee: { name: task.assigneeName || "Unassigned" },
        }));
    } catch (error) {
        console.error("Failed to fetch tasks by project", error);
        return [];
    }
};

export const updateTaskStatus = async (taskId, newStatus) => {
  const response = await API.patch(`/tasks/${taskId}/status`, { status: newStatus });
  return response.data;
};



export const deleteTasks = async (taskIds) => {
    const promises = taskIds.map(id => axios.delete(`/api/tasks/${id}`));
    await Promise.all(promises);
};