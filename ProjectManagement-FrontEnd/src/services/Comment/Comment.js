import { API } from "../api";

// 获取指定任务的评论
export const getCommentsByTaskId = async (taskId) => {
  try {
    const res = await API.get("/comments");
    // 只返回当前任务的评论
    return res.data
      .filter(c => c.projectTaskId === taskId)
      .sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));
  } catch (error) {
    console.error("Failed to fetch comments", error);
    return [];
  }
};

// 添加评论
export const addCommentToTask = async (taskId, userId, content) => {
  try {
    const res = await API.post("/comments", {
      ProjectTaskId: taskId,
      UserId: userId,
      Content: content
    });
    return res.data;
  } catch (error) {
    console.error("Failed to add comment", error);
    return null;
  }
};
