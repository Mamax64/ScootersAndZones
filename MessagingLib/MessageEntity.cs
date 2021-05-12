namespace MessagingLib
{
    public class MessageEntity
    {
        public string EventEntity;
        public string EventEntityId;
        public string Action;

        public MessageEntity(string eventEntity, string eventEntityId, string action)
        {
            EventEntity = eventEntity;
            EventEntityId = eventEntityId;
            Action = action;
        }
    }
}
