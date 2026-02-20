import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "../../styles/Chat/ChatListPage.css";
import ChatListStore from "../../stores/ChatListStore";

function ChatsListPage() {
  const [chats, setChats] = useState([]);
  const [clients, setClients] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

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

        const clientIdsWithChat = new Set(
          chatsWithUsers.map((c) => c.clientId),
        );

        if (currentUserId) {
          const assignRes =
            await ChatListStore.getAssignmentsByCoach(currentUserId);
          const assignments = assignRes.data || [];

          const filteredAssignments = assignments.filter(
            (a) => !clientIdsWithChat.has(a.userId),
          );

          const mappedClients = await Promise.all(
            filteredAssignments.map(async (a) => {
              const clientId = a.userId;
              let fullName = "Client";
              const u = await ChatListStore.getUserById(clientId);
              fullName = u.data.fullName;
              return {
                id: clientId,
                fullName,
              };
            }),
          );

          setClients(mappedClients.filter((c) => c.id));
        }
      } catch (e) {
        console.error(e);
        setError("Failed to load chats");
      } finally {
        setLoading(false);
      }
    })();
  }, [currentUserId]);

  if (loading) return <p className="chats-loading">Loading chats...</p>;
  if (error) return <p className="chats-error">{error}</p>;

  const handleClientClick = async (client) => {
    try {
      const res = await ChatListStore.getOrCreateChat(currentUserId, client.id);
      const chat = res.data;
      navigate(`/chat/${chat.id}`);
    } catch (e) {
      console.error(e);
      alert("Failed to open chat");
    }
  };

  return (
    <div className="chats-list-page">
      <div className="chats-header">
        <h2 className="chats-title">Messages</h2>
        <p className="chats-subtitle">
          {chats.length === 0
            ? "No chats yet. Start a conversation!"
            : `${chats.length} ${chats.length === 1 ? "chat" : "chats"}`}
        </p>
      </div>

      {clients.length > 0 && (
        <div className="chats-clients-row">
          {clients.map((c) => (
            <button
              key={c.id}
              className="chats-client-pill"
              onClick={() => handleClientClick(c)}
            >
              <span className="chats-client-avatar">
                {(c.fullName?.[0] || "C").toUpperCase()}
              </span>
              <span className="chats-client-name">
                {c.fullName || "Client"}
              </span>
            </button>
          ))}
        </div>
      )}

      {chats.length === 0 ? (
        <div className="chats-empty">
          <p>No chats yet.</p>
        </div>
      ) : (
        <div className="chats-list">
          {chats.map((chat) => {
            const otherName =
              currentUserId === chat.coachId ? chat.clientName : chat.coachName;

            return (
              <div
                key={chat.id}
                className="chats-list-item"
                onClick={() => navigate(`/chat/${chat.id}`)}
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
      )}
    </div>
  );
}

export default ChatsListPage;
