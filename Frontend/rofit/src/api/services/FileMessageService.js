import baseApi from "../BaseApi";

const FileMessageService = {
  upload: async (chatId, file) => {
    const formData = new FormData();
    formData.append("file", file);

    const response = await baseApi.post(
      `/FileMessages/upload?chatId=${chatId}`,
      formData,
    );
    return response.data;
  },

  download: async (fileId) => {
    const response = await baseApi.get(`/FileMessages/download/${fileId}`, {
      responseType: "blob",
    });
    const file = await baseApi.get(`/FileMessages/${fileId}`);
    console.log(file);
    const fileName = file.data.fileName;
    let mimeType =
      response.headers["content-type"] || "application/octet-stream";

    if (mimeType === "application/octet-stream") {
      const lower = fileName.toLowerCase();
      if (lower.endsWith(".pdf")) mimeType = "application/pdf";
      else if (lower.endsWith(".txt")) mimeType = "text/plain";
      else if (lower.endsWith(".png")) mimeType = "image/png";
      else if (lower.endsWith(".jpg") || lower.endsWith(".jpeg"))
        mimeType = "image/jpeg";
    }

    const blob = new Blob([response.data], { type: mimeType });
    const url = window.URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(url);
  },
  getByChat: async (chatId) => {
    const response = await baseApi.get(`/FileMessages/chat/${chatId}`);
    return response.data.messages || [];
  },
};

export default FileMessageService;
