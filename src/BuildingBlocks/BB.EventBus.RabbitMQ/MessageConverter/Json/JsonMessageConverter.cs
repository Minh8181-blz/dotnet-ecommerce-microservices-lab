using BB.EventBus.Events;
using BB.EventBus.RabbitMQ.MessageConverter.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BB.EventBus.RabbitMQ.MessageConverter.Json
{
    class JsonMessageConverter : IMessageConverter
    {
        public T Deserialize<T>(ReadOnlyMemory<byte> body) where T : IntegrationEvent
        {
            var raw = Encoding.UTF8.GetString(body.ToArray());
            var message = JsonConvert.DeserializeObject<T>(raw, new JsonSerializerSettings()
            {
                ContractResolver = new IntegrationEventResolverContract()
            });

            return message;
        }

        public byte[] Serialize(IntegrationEvent message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            return body;
        }
    }
}
