import * as signalR from "@microsoft/signalr";

export function createChatConnection(token) {
  return new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5293/hubs/chat", {
      accessTokenFactory: () => token,
    })
    .withAutomaticReconnect()
    .build();
}
