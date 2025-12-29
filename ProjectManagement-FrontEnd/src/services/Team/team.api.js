import { API } from "../api"; // 相对路径指向 services/api.js

export const getAllUsersSimple = () => API.get("/user/team");
export const getTeamStats = () => API.get("/User/teamstats");
export const searchUsers = (keyword) => API.get(`/User/search?keyword=${keyword}`);

// 修改这里，将 register 改成 invite
export const registerUser = (dto) => API.post("/User/invite", dto);

export const getAllUsers = () => API.get("/user");

export const getAllUsersWithRole = async () => {
    try {
      const [allUsersRes, teamRes] = await Promise.all([
        API.get("/user"),
        API.get("/user/team")
      ]);
  
      const allUsers = allUsersRes.data; 
      const teamUsers = teamRes.data;    
  

      const merged = allUsers.map(user => {
        const teamInfo = teamUsers.find(t => t.email === user.email);
        return {
          id: user.id,
          name: user.name,
          email: user.email,
          role: teamInfo?.role || "Member" 
        };
      });
  
      return merged;
    } catch (err) {
      console.error("Failed to fetch users with role", err);
      return [];
    }
  };

  export const deleteUser = (id) => API.delete(`/user/${id}`);