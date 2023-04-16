using BB.EventBus.RabbitMQ.Models;
using BB.EventBus.RabbitMQ.Publishers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BB.EventBus.RabbitMQ
{
    class PublisherManager
    {
        private readonly PublisherFactory _publisherFactory;
        private readonly List<Publisher> _publishers;

        public PublisherManager(
            IEnumerable<PublisherSpecification> publisherSpecifications,
            IServiceProvider serviceProvider)
        {
            _publisherFactory = serviceProvider.GetRequiredService<PublisherFactory>();
            _publishers = new List<Publisher>();
            InitList(publisherSpecifications);
        }

        private void InitList(IEnumerable<PublisherSpecification> publisherSpecifications)
        {
            try
            {
                foreach (var spec in publisherSpecifications)
                {
                    var publisher = GetPublisherForEventType(spec.EventType);
                    if (publisher == null)
                    {
                        publisher = Create(spec);
                    }
                }
            }
            catch
            {
                throw new Exception("EventBus RabbitMQ: Invalid passed publisher specification");
            }
        }

        public Publisher Create(PublisherSpecification specification)
        {
            var publisher = GetPublisherForEventType(specification.EventType);
            if (publisher != null)
            {
                throw new Exception($"Publisher for {specification.EventType.Name} already exists");
            }

            publisher = DoCreate(specification);
            _publishers.Add(publisher);
            return publisher;
        }

        private Publisher DoCreate(PublisherSpecification specification)
        {
            var publisher = _publisherFactory.Create(specification);
            return publisher;
        }

        public Publisher GetPublisherForEventType(Type eventType)
        {
            return _publishers.FirstOrDefault(x => x.EventType.Equals(eventType));
        }
    }
}
