
using MessagingLib;

namespace ScootAPI.Services
{
    public interface IAmqpService
    {
        void SendMessage(MessageEntity message);
    }
}
