// src/redux/userSlice.js
import { createSlice } from "@reduxjs/toolkit";

// 初始状态，存储当前用户信息
const initialState = {
    userId: null,   // 后端返回的唯一用户 ID
    role: null,     // 用户角色，例如 "Admin", "User"
    email: null,    // 可选，用于显示
    name: null      // 可选，用于显示
};

// 创建 user slice
export const userSlice = createSlice({
    name: "user",
    initialState,
    reducers: {
        // 登录成功后设置用户信息
        setUser: (state, action) => {
            const { userId, role, email, name } = action.payload;
            state.userId = userId;
            state.role = role;
            state.email = email;
            state.name = name;
        },
        // 登出时清空用户信息
        logout: (state) => {
            state.userId = null;
            state.role = null;
            state.email = null;
            state.name = null;
        },
    },
});

// 导出 action，用于组件中 dispatch
export const { setUser, logout } = userSlice.actions;

// 导出 reducer，用于配置 store
export default userSlice.reducer;
