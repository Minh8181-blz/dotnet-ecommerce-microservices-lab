using BB.EventBus.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BB.EventBus.RabbitMQ.MessageConverter.Json
{
    public class IntegrationEventResolverContract : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            if (typeof(IntegrationEvent).IsAssignableFrom(member.DeclaringType))
            {
                prop.Writable = true;
            }

            return prop;
        }
    }
}
