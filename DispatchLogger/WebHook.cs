using MessagingLib;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace DispatchLogger
{
    public static class WebHook
    {
        private const string token = "https://discord.com/api/webhooks/841680482332704848/X0baJKyK65u19DLfvqfF9brwvDfsDLWY9CE-NqwoJl8onMJAjs2I1PgDAJB4WYmqUi4O";

        public static void WriteMessage(MessageEntity msg)
        {
            WebRequest wr = (HttpWebRequest)WebRequest.Create(token);

            wr.ContentType = "application/json";

            wr.Method = "POST";

            string time = DateTime.UtcNow.ToString();
            string trame = $"{time} | id: {msg.EventEntityId}";

            string msgColor = "15844367";
            if (msg.EventEntity == "Scooter") msgColor = "3447003";

            using (StreamWriter sw = new(wr.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    username = "Scooter Logger",
                    embeds = new[]
                    {
                        new
                        {
                            description = trame,
                            title = $"New Event: {msg.EventEntity} {msg.Action}",
                            color = msgColor
                        }
                    }
                });
                sw.Write(json);
            }
            var response = (HttpWebResponse)wr.GetResponse();
        }
    }
}
