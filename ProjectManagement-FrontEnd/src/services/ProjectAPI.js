// src/api/projects.js
import { API } from "./axios"; // import your Axios instance

export const createProject = async (data) => {
    return API.post("/projects", data);
};

export const getAllProjects = async () => {
    return API.get("/projects");
};

export const getProjectById = async (id) => {
    return API.get(`/projects/${id}`);
};

export const updateProject = async (id, data) => {
    return API.put(`/projects/${id}`, data);
};

export const deleteProject = async (id) => {
    return API.delete(`/projects/${id}`);
};
