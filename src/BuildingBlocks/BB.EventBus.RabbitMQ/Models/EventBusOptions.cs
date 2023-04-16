using System;
using System.Collections.Generic;
using System.Text;

namespace BB.EventBus.RabbitMQ.Models
{
    public class EventBusOptions
    {
        public string ConnectionString { get; set; }
        public IEnumerable<ConsumerSpecification> ConsumerSpecifications { get; set; }
        public IEnumerable<PublisherSpecification> PublisherSpecifications { get; set; }
    }
}
