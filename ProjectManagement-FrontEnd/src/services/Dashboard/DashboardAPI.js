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

export const getCurrentUserName = async (userId) => {
  try {
    const res = await API.get(`/user/me?userId=${userId}`); // 动态传入 userId
    return res.data.name; // 注意小写 name
  } catch (err) {
    console.error("Failed to fetch current user name", err);
    return null;
  }
};