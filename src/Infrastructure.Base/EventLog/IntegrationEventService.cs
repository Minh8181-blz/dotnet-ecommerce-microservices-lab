using Application.Base.SeedWork;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Base.EventLog
{
    public class IntegrationEventService<T> : IIntegrationEventService where T : IIntegrationEventDbContext
    {
        protected readonly IIntegrationEventDbContext _context;
        protected readonly IIntegrationEventBus _eventBus;
        protected readonly ILogger<T> _logger;

        public IntegrationEventService(T context, IIntegrationEventBus eventBus, ILogger<T> logger)
        {
            _context = context;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task SaveEventAsync(IntegrationEvent @event)
        {
            var eventLogEntry = new IntegrationEventLogEntry(@event);
            await _context.EventLogEntries.AddAsync(eventLogEntry);
        }

        public async Task PublishAsync(IntegrationEvent @event)
        {
            try
            {
                await MarkEventAsInProgressAsync(@event.Id);
                _eventBus.PublishEvent(@event);
                await MarkEventAsPublishedAsync(@event.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await MarkEventAsFailedAsync(@event.Id);
            }
        }

        protected Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        protected Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        protected Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        protected Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventLogEntry = _context.EventLogEntries.SingleOrDefault(ie => ie.EventId == eventId);
            eventLogEntry.UpdateState(status);
            _context.EventLogEntries.Update(eventLogEntry);

            return _context.SaveChangesAsync();
        }
    }
}
