import ChatService from "../api/services/ChatService";
import UserService from "../api/services/UserService";
import FileMessageService from "../api/services/FileMessageService";
class ChatStore {
  async getChatMessages(chatId) {
    const response = await ChatService.getChatMessages(chatId);
    const sorted = [...response.data.messages].sort(
      (a, b) => new Date(a.createdAt) - new Date(b.createdAt),
    );
    return sorted || [];
  }

  async getUserById(userId) {
    const response = await UserService.getById(userId);
    return response.data;
  }

  async uploadFileMessage(chatId, file) {
    const dto = await FileMessageService.upload(chatId, file);
    return dto;
  }

  async downloadFileMessage(fileId) {
    await FileMessageService.download(fileId);
  }
  async getFileMessagesByChat(chatId) {
    const list = await FileMessageService.getByChat(chatId);
    const sortedFiles = [...list].sort(
      (a, b) => new Date(a.createdAt) - new Date(b.createdAt),
    );
    console.log(sortedFiles);
    return sortedFiles || [];
  }
  async getChatById(chatId) {
    const chat = await ChatService.getChatById(chatId);
    return chat;
  }
}

export default new ChatStore();
