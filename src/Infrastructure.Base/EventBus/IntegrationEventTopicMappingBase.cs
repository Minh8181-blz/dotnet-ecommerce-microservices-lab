using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Base.EventBus
{
    public class IntegrationEventTopicMappingBase : IIntegrationEventTopicMapping
    {
        protected readonly Dictionary<string, RabbitMqTopicModel> PubMaps;
        protected readonly Dictionary<string, RabbitMqQueueModel> SubMaps;

        public IntegrationEventTopicMappingBase()
        {
            PubMaps = new Dictionary<string, RabbitMqTopicModel>();
            SubMaps = new Dictionary<string, RabbitMqQueueModel>();
        }

        public virtual RabbitMqTopicModel GetPublishedTopic(string eventTypeName)
        {
            if (PubMaps.ContainsKey(eventTypeName))
            {
                return PubMaps[eventTypeName];
            }

            return null;
        }

        public virtual RabbitMqQueueModel GetSubscribedQueue(string eventTypeName)
        {
            if (SubMaps.ContainsKey(eventTypeName))
            {
                return SubMaps[eventTypeName];
            }

            return null;
        }
    }
}
