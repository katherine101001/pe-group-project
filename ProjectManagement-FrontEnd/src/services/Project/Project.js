import { API } from '../api';

export const createProject = async (payload) => {
    try {
      const res = await API.post("/projects", payload);
      return res.data; 
    } catch (error) {
      console.error("Failed to create project", error);
      throw error;
    }
  };

export const getAllProjects = async () => {
  try {
    const res = await API.get("/projects");
    return res.data;
  } catch (error) {
    console.error("Failed to fetch projects", error);
    return [];
  }
};

export const getProjectById = async (projectId) => {
  try {
    const res = await API.get(`/projects/${projectId}`);
    return res.data;
  } catch (error) {
    console.error("Failed to fetch project", error);
    return null;
  }
};
