using FirebaseAdmin.Messaging;

namespace ROFit.Service
{
    public class FirebaseNotificationService
    {
        public async Task<bool> SendPushNotificationAsync(string title, string body, string deviceToken)
        {
            try
            {
                var message = new Message
                {
                    Token = deviceToken,

                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },

                    Data = new Dictionary<string, string>
                    {
                        { "title", title },
                        { "body", body },
                        { "click_action", "FLUTTER_NOTIFICATION_CLICK" }
                    },

                    Android = new AndroidConfig
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification
                        {
                            ChannelId = "high_importance_channel",
                            Title = title,
                            Body = body,
                            Sound = "default"
                        }
                    },

                    Apns = new ApnsConfig
                    {
                        Aps = new Aps
                        {
                            Alert = new ApsAlert
                            {
                                Title = title,
                                Body = body
                            },
                            Sound = "default"
                        }
                    }
                };

                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
