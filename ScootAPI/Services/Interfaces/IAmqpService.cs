using ScootAPI.Models.Messaging;

namespace ScootAPI.Services
{
    public interface IAmqpService
    {
        void SendMessage(Message message);
    }
}
