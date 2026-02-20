import { useState, useEffect } from "react";
import "../../styles/Chat/ChatListPage.css";
import ChatListStore from "../../stores/ChatListStore";

function ChatsListEmbedded({ onSelectChat }) {
  const [chats, setChats] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const storedUser = localStorage.getItem("user");
  const currentUser = storedUser ? JSON.parse(storedUser) : null;
  const currentUserId = currentUser?.id;

  useEffect(() => {
    (async () => {
      try {
        setLoading(true);

        const res = await ChatListStore.getMyChats();
        const chatsWithUsers = await Promise.all(
          (res.data || []).map(async (chat) => {
            try {
              const coach = await ChatListStore.getUserById(chat.coachId);
              const client = await ChatListStore.getUserById(chat.clientId);
              return {
                ...chat,
                coachName: coach.data?.fullName || "Unknown Coach",
                clientName: client.data?.fullName || "Unknown Client",
              };
            } catch {
              return {
                ...chat,
                coachName: "Unknown Coach",
                clientName: "Unknown Client",
              };
            }
          }),
        );

        setChats(chatsWithUsers);
        setError(null);
      } catch (e) {
        console.error(e);
        setError("Failed to load chats");
      } finally {
        setLoading(false);
      }
    })();
  }, [currentUserId]);

  if (loading) {
    return <p className="chats-loading">Loading chats...</p>;
  }

  if (error) {
    return <p className="chats-error">{error}</p>;
  }

  if (chats.length === 0) {
    return (
      <div className="chats-empty">
        <p>No chats yet. Start a conversation!</p>
      </div>
    );
  }

  return (
    <div className="chats-list-embedded">
      <div className="chats-header">
        <h2 className="chats-title">Messages</h2>
      </div>

      <div className="chats-list">
        {chats.map((chat) => {
          const otherName =
            currentUserId === chat.coachId ? chat.clientName : chat.coachName;

          return (
            <div
              key={chat.id}
              className="chats-list-item"
              onClick={() => onSelectChat(chat.id)}
            >
              <div className="chats-item-avatar">
                {(otherName || "?").charAt(0).toUpperCase()}
              </div>
              <div className="chats-item-content">
                <h3 className="chats-item-name">{otherName}</h3>
                <p className="chats-item-preview">
                  Chat with{" "}
                  {currentUserId === chat.coachId ? "client" : "coach"}
                </p>
              </div>
              <div className="chats-item-meta">
                <span className="chats-item-arrow">→</span>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}

export default ChatsListEmbedded;
