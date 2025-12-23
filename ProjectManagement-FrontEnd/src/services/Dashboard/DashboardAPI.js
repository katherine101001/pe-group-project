// src/services/projectService.js
import { API } from '../api';

export const getRecentTasks = async (limit = 5) => {
  try {
    const response = await API.get(`/tasks/recent?limit=${limit}`);
    return response.data; // 返回最近任务数组
  } catch (error) {
    console.error("Failed to fetch recent tasks", error);
    throw error;
  }
};

export const getOverdueTasks = async () => {
  try {
    const response = await API.get('/tasks/overdue');
    return response.data; // 返回所有超期任务数组
  } catch (error) {
    console.error("Failed to fetch overdue tasks", error);
    throw error;
  }
};

export const getMyTasks = async (userId) => {
  try {
    const response = await API.get(`/tasks?assigneeId=${userId}`);
    return response.data;
  } catch (error) {
    console.error("Failed to fetch my tasks", error);
    throw error;
  }
};

export const getOverdueCount = async () => {
  try {
    const response = await API.get('/tasks/overdue/count');
    return response.data.count; // 返回 { count }
  } catch (error) {
    console.error("Failed to fetch overdue count", error);
    return 0;
  }
};

export const getDashboardStats = {
  getStats: async (userId, role) => {
    try {
      const res = await API.get("/dashboard/stats", {
        params: { userId, role }
      });

      return {
        totalProjects: res.data.totalProjects,
        activeProjects: res.data.activeProjects,
        completedProjects: res.data.completedProjects,
        myTasks: res.data.myTasks,
        overdueIssues: res.data.overdueTasks,
      };
    } catch (err) {
      console.error("Failed to fetch dashboard stats", err);
      return {
        totalProjects: 0,
        activeProjects: 0,
        completedProjects: 0,
        myTasks: 0,
        overdueIssues: 0,
      };
    }
  },
};


