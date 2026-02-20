import baseApi from "../BaseApi";

const ChatService = {
  getMyChats: () => baseApi.get("/Chat"),
  getChatMessages: (chatId, take = 50, skip = 0) =>
    baseApi.get(`/Chat/${chatId}/messages`, {
      params: { take, skip },
    }),
  getOrCreateChat: (coachId, clientId) =>
    baseApi.post(`/Chat/coach/${coachId}/client/${clientId}`),
  getChatById: (chatId) => baseApi.get(`/Chat/${chatId}`),
};

export default ChatService;
