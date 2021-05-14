namespace MessagingLib
{
    public class MessageEntity
    {
        public string EventEntity { get; set; }
        public string EventEntityId { get; set; }
        public string Action { get; set; }

        public MessageEntity(string eventEntity, string eventEntityId, string action)
        {
            EventEntity = eventEntity;
            EventEntityId = eventEntityId;
            Action = action;
        }
    }
}
