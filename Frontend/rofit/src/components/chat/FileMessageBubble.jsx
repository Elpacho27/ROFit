import ChatStore from "../../stores/ChatStore";
import { FiDownload, FiFile } from "react-icons/fi";
import docIcon from "../../assets/images/doc.png";
import xlsIcon from "../../assets/images/xls.png";
const API_BASE = "http://localhost:5293";

function FileMessageBubble({ fileMessage, isMine, onPreview }) {
  const url = `${API_BASE}${fileMessage.filePath}`;
  const ext = (fileMessage.fileType || "").toLowerCase();
  const isImage = ["png", "jpg", "jpeg", "gif", "webp"].includes(ext);
  const isDoc = ["doc", "docx"].includes(ext);
  const isXls = ["xls", "xlsx"].includes(ext);

  const handleDownload = async () => {
    try {
      await ChatStore.downloadFileMessage(fileMessage.id);
    } catch (e) {
      console.error(e);
      alert("Download failed");
    }
  };

  const handleClickImage = () => {
    if (!isImage) return;
    if (onPreview) onPreview(url);
    else window.open(url, "_blank", "noopener,noreferrer");
  };

  const formatSize = (bytes) => {
    const kb = bytes / 1024;
    if (kb >= 1024) {
      const mb = kb / 1024;
      return `${mb.toFixed(1)} MB`;
    }
    return `${Math.round(kb)} KB`;
  };

  const renderThumb = () => {
    if (isImage) {
      return (
        <div className="chat-image-thumb-wrapper" onClick={handleClickImage}>
          <img
            src={url}
            className="chat-image-thumb"
            alt={fileMessage.fileName}
          />
        </div>
      );
    }

    if (isDoc) {
      return (
        <div className="chat-file-generic">
          <img src={docIcon} alt="doc" className="chat-file-icon" />
          <span className="chat-file-name">{fileMessage.fileName}</span>
        </div>
      );
    }

    if (isXls) {
      return (
        <div className="chat-file-generic">
          <img src={xlsIcon} alt="xls" className="chat-file-icon" />
          <span className="chat-file-name">{fileMessage.fileName}</span>
        </div>
      );
    }

    return (
      <div className="chat-file-generic">
        <FiFile size={18} style={{ marginRight: 6 }} />
        <span className="chat-file-name">{fileMessage.fileName}</span>
      </div>
    );
  };

  return (
    <div
      className={`chat-message-row ${
        isMine ? "chat-message-row--mine" : "chat-message-row--theirs"
      }`}
    >
      <div
        className={`chat-message-bubble ${
          isMine ? "chat-message-bubble--mine" : "chat-message-bubble--theirs"
        }`}
        style={{ display: "flex", alignItems: "center", gap: 8 }}
      >
        <div style={{ flex: 1 }}>
          {renderThumb()}

          <div style={{ fontSize: 12, color: "#fff", marginTop: 4 }}>
            {formatSize(fileMessage.fileSize)}
          </div>
        </div>

        <button
          onClick={handleDownload}
          style={{
            border: "none",
            background: "transparent",
            color: "#fff",
            cursor: "pointer",
            padding: 4,
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
          aria-label="Download file"
        >
          <FiDownload size={18} />
        </button>
      </div>
    </div>
  );
}

export default FileMessageBubble;
