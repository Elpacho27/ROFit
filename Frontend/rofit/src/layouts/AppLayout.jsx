import { useState } from "react";
import { Outlet, NavLink } from "react-router-dom";
import RoleWidget from "../components/auth/RoleWidget";
import logoTransparent from "../assets/images/logo_transparent_bg.png";
import chatIcon from "../assets/images/chat.png";
import "../styles/Layouts/AppLayout.css";
import ChatWidgetModal from "../components/chat/ChatWidgetModal";

function AppLayout() {
  const [isChatOpen, setIsChatOpen] = useState(false);
  const [activeChatId, setActiveChatId] = useState(null);

  const handleSelectChat = (chatId) => {
    setActiveChatId(chatId);
  };

  const handleBackToList = () => {
    setActiveChatId(null);
  };

  const handleCloseChat = () => {
    setIsChatOpen(false);
    setActiveChatId(null);
  };

  return (
    <div className="app-layout-root">
      <header className="app-layout-header">
        <nav className="app-layout-nav">
          <div className="app-layout-logo-wrapper">
            <NavLink to="/" className="app-layout-logo-link">
              <img
                src={logoTransparent}
                alt="ROFit logo"
                className="app-layout-logo"
              />
            </NavLink>
          </div>
          <div className="app-layout-links">
            <RoleWidget allowedRoles={["Coach"]}>
              <NavLink to="/coach-assignments" className="app-layout-link">
                MY CLIENTS
              </NavLink>
            </RoleWidget>
            <NavLink to="/user" className="app-layout-link">
              PROFILE
            </NavLink>
          </div>
        </nav>
      </header>

      <main className="app-layout-main">
        <Outlet />
      </main>

      {!isChatOpen && (
        <button
          type="button"
          className="chat-fab"
          onClick={() => setIsChatOpen(true)}
        >
          <img src={chatIcon} alt="chat" className="chat-file-icon" />
          <span className="chat-fab-label">Chat</span>
        </button>
      )}

      <ChatWidgetModal
        isOpen={isChatOpen}
        activeChatId={activeChatId}
        onSelectChat={handleSelectChat}
        onBackToList={handleBackToList}
        onClose={handleCloseChat}
      />
    </div>
  );
}

export default AppLayout;
