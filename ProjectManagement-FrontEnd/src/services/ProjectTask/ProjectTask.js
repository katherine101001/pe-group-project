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
  
