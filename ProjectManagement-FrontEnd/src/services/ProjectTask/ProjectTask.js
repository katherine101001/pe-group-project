import { API } from '../api'; 




export const getAllTasks = async () => {
    try {
      const res = await API.get("/tasks");
      return res.data; // 返回所有任务数组
    } catch (error) {
      console.error("Failed to fetch tasks", error);
      return [];
    }
  };

// 简略信息 API
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
      // 确保 due_date 是 Date 对象
      return res.data.map(task => ({
        ...task,
        due_date: new Date(task.dueDate || task.due_date),
        assignee: { name: task.assigneeName || "Unassigned" }, // 根据你的后端字段映射
      }));
    } catch (error) {
      console.error("Failed to fetch overdue tasks", error);
      return [];
    }
  };

  export const getTaskCalendarByMonth = async (year, month) => {
    try {
      const res = await API.get(`/tasks/calendar?year=${year}&month=${month}`);
      return res.data.map(task => ({
        ...task,
        due_date: new Date(task.dueDate || task.due_date),
        assignee: { name: task.assigneeName || "Unassigned" },
      }));
    } catch (error) {
      console.error("Failed to fetch calendar tasks", error);
      return [];
    }
  };