import ChatsListEmbedded from "../../pages/chat/ChatListEmbedded";
import EmbeddedChat from "./EmbeddedChat";
import "../../styles/chat/ChatWidgetModal.css";

function ChatWidgetModal({
  isOpen,
  activeChatId,
  onSelectChat,
  onBackToList,
  onClose,
}) {
  if (!isOpen) return null;

  return (
    <div className="chat-widget">
      <div className="chat-widget-header">
        <span className="chat-widget-title">
          {activeChatId ? "Chat" : "Messages"}
        </span>
        <div className="chat-widget-header-actions">
          {activeChatId && (
            <button
              type="button"
              className="chat-widget-back"
              onClick={onBackToList}
            >
              ←
            </button>
          )}
          <button type="button" className="chat-widget-close" onClick={onClose}>
            ✕
          </button>
        </div>
      </div>

      <div className="chat-widget-body">
        {!activeChatId ? (
          <ChatsListEmbedded onSelectChat={onSelectChat} />
        ) : (
          <EmbeddedChat chatId={activeChatId} />
        )}
      </div>
    </div>
  );
}

export default ChatWidgetModal;
