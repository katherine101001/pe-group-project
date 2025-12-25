// features/projects/projectsSlice.js
import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { getAllProjects } from "../services/Project/ProjectAPI";

// ===================== Thunks =====================

// 拉取所有项目
export const fetchProjects = createAsyncThunk(
  "projects/fetchProjects",
  async () => {
    const data = await getAllProjects();
    return data;
  }
);

// 归档项目
export const archiveProject = createAsyncThunk(
  "projects/archiveProject",
  async (projectId) => {
    const response = await fetch(`/api/projects/${projectId}/archive`, { method: "POST" });
    if (!response.ok) throw new Error("Failed to archive project");
    const project = await response.json(); // 返回单个 ProjectDto
    return project;
  }
);

// 恢复项目
export const unarchiveProject = createAsyncThunk(
  "projects/unarchiveProject",
  async (projectId) => {
    const response = await fetch(`/api/projects/${projectId}/unarchive`, { method: "POST" });
    if (!response.ok) throw new Error("Failed to unarchive project");
    const project = await response.json();
    return project;
  }
);

// ===================== Slice =====================
const projectsSlice = createSlice({
  name: "projects",
  initialState: { list: [], status: "idle" },
  reducers: {},
  extraReducers: (builder) => {
    builder
      // 获取项目列表
      .addCase(fetchProjects.fulfilled, (state, action) => {
        state.list = action.payload;
      })

      // 归档项目：immutable 更新，ID 转 string 保证匹配
      .addCase(archiveProject.fulfilled, (state, action) => {
        const updatedProject = action.payload;
        state.list = state.list.map(p =>
          String(p.id) === String(updatedProject.id) ? { ...updatedProject } : p
        );
      })

      // 恢复项目：immutable 更新，ID 转 string 保证匹配
      .addCase(unarchiveProject.fulfilled, (state, action) => {
        const updatedProject = action.payload;
        state.list = state.list.map(p =>
          String(p.id) === String(updatedProject.id) ? { ...updatedProject } : p
        );
      });
  },
});

export default projectsSlice.reducer;
