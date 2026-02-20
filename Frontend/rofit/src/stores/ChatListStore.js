import ChatService from "../api/services/ChatService";
import UserService from "../api/services/UserService";
import CoachAssignmentsService from "../api/services/CoachAssignmentsService";

class ChatListStore {
  constructor() {
    this.getMyChats = this.getMyChats.bind(this);
  }
  async getMyChats() {
    const response = await ChatService.getMyChats();
    return response;
  }
  async getOrCreateChat(currentUserId, clientId) {
    const response = await ChatService.getOrCreateChat(
      currentUserId,
      client.id,
    );
    return response;
  }
  async getUserById(userId) {
    return await UserService.getById(userId);
  }

  async getAssignmentsByCoach(coachId) {
    return await CoachAssignmentsService.getByCoach(coachId);
  }
}

export default new ChatListStore();
