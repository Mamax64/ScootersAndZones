namespace ScootAPI.Models.Messaging
{
    public class Message
    {
        public string EventEntity;
        public string EventEntityId;
        public string Action;

        public Message(string eventEntity, string eventEntityId, string action)
        {
            EventEntity = eventEntity;
            EventEntityId = eventEntityId;
            Action = action;
        }
    }
}
