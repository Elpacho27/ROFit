import { useEffect, useState, useCallback, useRef, useMemo } from "react";
import { useParams } from "react-router-dom";
import { createChatConnection } from "../../signalr/ChatConnection";
import ChatStore from "../../stores/ChatStore";
import FileMessageBubble from "../../components/chat/FileMessageBubble";
import ChatFileUpload from "../../components/chat/ChatFileUpload";
import "../../styles/chat/ChatPage.css";

function ChatPage({ embedded = false, chatId: propChatId }) {
  const params = useParams();
  const routeChatId = params.chatId;
  const chatId = embedded ? propChatId : routeChatId;

  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([]);
  const [fileMessages, setFileMessages] = useState([]);
  const [text, setText] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [otherUser, setOtherUser] = useState(null);
  const [isOtherTyping, setIsOtherTyping] = useState(false);

  const bottomRef = useRef(null);
  const typingTimeoutRef = useRef(null);

  const storedUser = localStorage.getItem("user");
  const currentUser = storedUser ? JSON.parse(storedUser) : null;
  const currentUserId = currentUser?.id;

  const scrollToBottom = useCallback(() => {
    if (bottomRef.current) {
      bottomRef.current.scrollIntoView({
        behavior: "smooth",
        block: "nearest",
      });
    }
  }, []);

  useEffect(() => {
    if (!chatId) return;

    (async () => {
      try {
        setLoading(true);
        const messagesRes = await ChatStore.getChatMessages(chatId);
        setMessages(messagesRes);

        const fileMessagesRes = await ChatStore.getFileMessagesByChat(chatId);
        setFileMessages(fileMessagesRes);

        const chat = await ChatStore.getChatById(chatId);
        const otherId = chat.data.clientId;
        const user = await ChatStore.getUserById(otherId);
        setOtherUser(user);
        setError(null);
      } catch (e) {
        console.error(e);
        setError("Failed to load messages");
      } finally {
        setLoading(false);
      }
    })();
  }, [chatId]);

  useEffect(() => {
    if (!loading && !error) {
      scrollToBottom();
    }
  }, [messages, fileMessages, loading, error, scrollToBottom]);

  useEffect(() => {
    if (!chatId) return;

    let conn = null;
    let cancelled = false;

    (async () => {
      try {
        const token = localStorage.getItem("token");
        conn = createChatConnection(token);

        conn.on("ReceiveMessage", async (msg) => {
          if (msg.chatId !== chatId) return;
          setMessages((prev) => [...prev, msg]);
          if (msg.senderId !== currentUserId) {
            setOtherUser(
              async (prev) =>
                prev || (await ChatStore.getUserById(msg.senderId)),
            );
          }
        });

        conn.on("ReceiveFileMessage", (fileMsg) => {
          if (fileMsg.chatId !== chatId) return;
          setFileMessages((prev) => [...prev, fileMsg]);
        });

        conn.on("UserTyping", (chatIdFromHub, isTyping) => {
          if (chatIdFromHub === chatId) {
            setIsOtherTyping(isTyping);
          }
        });

        await conn.start();
        if (cancelled) {
          await conn.stop();
          return;
        }

        await conn.invoke("JoinChat", chatId);
        setConnection(conn);
        setError(null);
      } catch (e) {
        console.error(e);
        if (!conn || conn.state === "Disconnected") {
          setError("Failed to connect to chat");
        }
      }
    })();

    return () => {
      cancelled = true;
      if (conn) conn.stop();
      if (typingTimeoutRef.current) {
        clearTimeout(typingTimeoutRef.current);
      }
    };
  }, [chatId, currentUserId]);

  const sendMessage = async () => {
    if (!text.trim() || !connection || !chatId) return;
    try {
      await connection.invoke("SendMessage", chatId, text);
      setText("");
      if (typingTimeoutRef.current) clearTimeout(typingTimeoutRef.current);
      connection.invoke("StopTyping", chatId).catch(console.error);
    } catch (e) {
      console.error(e);
    }
  };

  const handleLocalFileMessageCreated = (dto) => {
    setFileMessages((prev) => [...prev, dto]);
  };

  const handleChange = (e) => {
    const value = e.target.value;
    setText(value);
    if (!connection || !chatId) return;

    connection.invoke("StartTyping", chatId).catch(console.error);
    if (typingTimeoutRef.current) {
      clearTimeout(typingTimeoutRef.current);
    }
    typingTimeoutRef.current = setTimeout(() => {
      connection.invoke("StopTyping", chatId).catch(console.error);
    }, 2000);
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      sendMessage();
    }
  };

  const combinedItems = useMemo(() => {
    const textItems = messages.map((m) => ({ type: "text", ...m }));
    const fileItems = fileMessages.map((f) => ({ type: "file", ...f }));

    return [...textItems, ...fileItems].sort(
      (a, b) => new Date(a.createdAt) - new Date(b.createdAt),
    );
  }, [messages, fileMessages]);

  if (!chatId) return <p>No chat selected.</p>;
  if (loading) return <p>Loading chat...</p>;
  if (error) return <p>{error}</p>;

  const otherUserName =
    otherUser && otherUser.fullName ? `${otherUser.fullName}` : "Client";

  return (
    <div className={`chat-page ${embedded ? "chat-page--embedded" : ""}`}>
      <div className={`chat-card ${embedded ? "chat-card--embedded" : ""}`}>
        <div className="chat-header">
          <div className="chat-header-avatar">
            {otherUserName[0]?.toUpperCase() || "C"}
          </div>
          <div className="chat-header-text">
            <div className="chat-header-name">{otherUserName}</div>
            {isOtherTyping && <div className="chat-header-typing">typing…</div>}
          </div>
        </div>

        <div className="chat-messages">
          {combinedItems.map((item) => {
            const isMine = item.senderId === currentUserId;

            if (item.type === "text") {
              return (
                <div
                  key={item.id}
                  className={`chat-message-row ${
                    isMine
                      ? "chat-message-row--mine"
                      : "chat-message-row--theirs"
                  }`}
                >
                  <div
                    className={`chat-message-bubble ${
                      isMine
                        ? "chat-message-bubble--mine"
                        : "chat-message-bubble--theirs"
                    }`}
                  >
                    <div className="chat-message-text">{item.content}</div>
                  </div>
                </div>
              );
            }

            return (
              <FileMessageBubble
                key={item.id}
                fileMessage={item}
                isMine={isMine}
              />
            );
          })}

          <div ref={bottomRef} />
        </div>

        <div className="chat-input-bar">
          <ChatFileUpload
            chatId={chatId}
            connection={connection}
            onLocalFileMessageCreated={handleLocalFileMessageCreated}
          />
          <input
            value={text}
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            placeholder="Type a message..."
            className="chat-input"
          />
          <button
            onClick={sendMessage}
            disabled={!connection || !text.trim()}
            className={`chat-send-button ${
              !connection || !text.trim() ? "chat-send-button--disabled" : ""
            }`}
          >
            Send
          </button>
        </div>
      </div>
    </div>
  );
}

export default ChatPage;
