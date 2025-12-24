import { API } from "../api"; // 相对路径指向 services/api.js

export const getAllUsersSimple = () => API.get("/user/team");
export const getTeamStats = () => API.get("/User/teamstats");
export const searchUsers = (keyword) => API.get(`/User/search?keyword=${keyword}`);

// 修改这里，将 register 改成 invite
export const registerUser = (dto) => API.post("/User/invite", dto);

export const getAllUsers = () => API.get("/user");
