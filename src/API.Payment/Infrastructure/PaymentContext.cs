using API.Payment.Domain.Entities;
using API.Payment.Infrastructure.StripeIdempotency;
using Infrastructure.Base.EventLog;
using Infrastructure.Base.RequestManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Payment.Infrastructure
{
    public class PaymentContext : DbContext, IIntegrationEventDbContext, IRequestManagerDbContext, IStripeEventManagerDbContext
    {
        public const string Schema = "ms_payment";

        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {

        }

        public virtual DbSet<PaymentOperation> PaymentOperations { get; set; }
        public virtual DbSet<IntegrationEventLogEntry> EventLogEntries { get; set; }
        public virtual DbSet<RequestEntry> RequestEntries { get; set; }
        public virtual DbSet<StripeEvent> StripeEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var paymentTableBuilder = modelBuilder.Entity<PaymentOperation>().ToTable("PaymentOperations", Schema);

            paymentTableBuilder
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            paymentTableBuilder
                .Property(p => p.Amount)
                .HasColumnType("decimal(20,5)");

            paymentTableBuilder
                .Property(p => p.AmountRefunded)
                .HasColumnType("decimal(20,5)");

            var stripePaymentRefTable = modelBuilder.Entity<PaymentStripeSession>().ToTable("PaymentStripeSessions", Schema);

            var eventLogTableBuilder = modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog", Schema);
            eventLogTableBuilder.HasKey(x => x.EventId);

            var requestEntryTableBuilder = modelBuilder.Entity<RequestEntry>().ToTable("RequestEntry", Schema);
            requestEntryTableBuilder.HasKey(x => x.Id);

            var stripeEventTableBuilder = modelBuilder.Entity<StripeEvent>().ToTable("StripeEvents", Schema);
            stripeEventTableBuilder.HasKey(x => x.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
        }
    }
}
