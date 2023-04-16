using BB.EventBus.RabbitMQ.Enums;
using BB.EventBus.RabbitMQ.MessageConverter.Interfaces;
using BB.EventBus.RabbitMQ.MessageConverter.Json;

namespace BB.EventBus.RabbitMQ.MessageConverter
{
    class MessageConverterFactory
    {
        public IMessageConverter CreateByMessageContentType(MessageContentTypeEnum contentType)
        {
            if (contentType == MessageContentTypeEnum.Json)
            {
                return new JsonMessageConverter();
            }

            return new JsonMessageConverter();
        }
    }
}
