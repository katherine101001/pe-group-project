import { configureStore } from '@reduxjs/toolkit'
import themeReducer from '../features/themeSlice'
import userReducer from "../features/userSlice";

export const store = configureStore({
    reducer: {
        theme: themeReducer,
        user: userReducer,
    },
})