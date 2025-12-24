import { API } from "../api";


export const getCommentsByTaskId = async (taskId) => {
  try {
    const res = await API.get("/comments");
    return res.data
      .filter(c => c.projectTaskId === taskId)
      .sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));
  } catch (error) {
    console.error("Failed to fetch comments", error);
    return [];
  }
};


export const addCommentToTask = async (taskId, userId, content, mentionedUserIds = []) => {
  try {
    const res = await API.post("/comments", {
      projectTaskId: taskId,
      userId: userId,
      content: content,
      mentionedUserIds: mentionedUserIds, // ✅ 注意这里
    });
    return res.data;
  } catch (error) {
    console.error("Failed to add comment", error);
    return null;
  }
};




