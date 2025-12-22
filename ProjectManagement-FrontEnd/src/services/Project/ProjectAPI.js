import { API } from '../api';
//import { updateProject } from "../services/projectAPI";

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

export const getProjectById = async (id) => {
  try {
    const res = await API.get(`/projects/${id}`);
    return res.data;
  } catch (error) {
    console.error("Failed to fetch project", error);
    return null;
  }
};

//import { API } from "../api";

export const updateProject = async (projectId, payload) => {
  try {
    const res = await API.put(`/projects/${projectId}`, payload);
    return res.data;
  } catch (error) {
    console.error("Failed to update project", error);
    throw error;
  }
};

