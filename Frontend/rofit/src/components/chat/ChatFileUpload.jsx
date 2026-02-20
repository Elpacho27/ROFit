import { useState } from "react";
import ChatStore from "../../stores/ChatStore";

function ChatFileUpload({ chatId, connection, onLocalFileMessageCreated }) {
  const [isUploading, setIsUploading] = useState(false);

  const handleChange = async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;

    try {
      setIsUploading(true);
      const dto = await ChatStore.uploadFileMessage(chatId, file);

      if (connection) {
        try {
          await connection.invoke("SendFileMessage", chatId, dto.id);
        } catch (err) {
          console.error("SendFileMessage failed", err);
        }
      }
      if (typeof onLocalFileMessageCreated === "function") {
        onLocalFileMessageCreated(dto);
      }
    } catch (err) {
      console.error(err);
      alert("File upload failed");
    } finally {
      setIsUploading(false);
      e.target.value = "";
    }
  };

  return (
    <label
      style={{ cursor: "pointer", opacity: isUploading ? 0.6 : 1 }}
      title="Attach file"
    >
      📎
      <input
        type="file"
        style={{ display: "none" }}
        onChange={handleChange}
        disabled={isUploading}
      />
    </label>
  );
}

export default ChatFileUpload;
