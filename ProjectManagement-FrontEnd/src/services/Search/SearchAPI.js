// src/services/Search/search.api.js
import { API } from '../api';

export const searchAll = async (keyword) => {
  try {
    const res = await API.get(`/search/all?keyword=${encodeURIComponent(keyword)}`);
    return res.data; // { Projects: [], Tasks: [] }
  } catch (error) {
    console.error("Failed to search projects and tasks", error);
    return { Projects: [], Tasks: [] };
  }
};
