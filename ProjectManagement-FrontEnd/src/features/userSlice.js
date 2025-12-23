import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    userId: null,   // store the unique ID from backend
    role: null,     // e.g., "Admin", "User"
    email: null,    // optional, for display
    name: null      // optional, for display
};

export const userSlice = createSlice({
    name: "user",
    initialState,
    reducers: {
        // set user info after login
        setUser: (state, action) => {
            const { userId, role, email, name } = action.payload;
            state.userId = userId;
            state.role = role;
            state.email = email;
            state.name = name;
        },
        // logout user
        logout: (state) => {
            state.userId = null;
            state.role = null;
            state.email = null;
            state.name = null;
        },
    },
});

export const { setUser, logout } = userSlice.actions;
export default userSlice.reducer;
