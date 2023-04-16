using API.Catalog.Domain.Entities;
using Infrastructure.Base.EventLog;
using Infrastructure.Base.RequestManager;
using Microsoft.EntityFrameworkCore;

namespace API.Catalog.Infrastructure
{
    public class CatalogContext : DbContext, IIntegrationEventDbContext, IRequestManagerDbContext
    {
        public const string Schema = "ms_catalog";

        public CatalogContext(DbContextOptions<CatalogContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockItemRecord> StockItemRecords { get; set; }
        public virtual DbSet<IntegrationEventLogEntry> EventLogEntries { get; set; }
        public virtual DbSet<RequestEntry> RequestEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var productTableBuider = modelBuilder.Entity<Product>().ToTable("Products", Schema);

            productTableBuider
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            productTableBuider
                .Property(p => p.Price)
                .HasColumnType("decimal(20,5)");

            var stockTableBuider = modelBuilder.Entity<Stock>().ToTable("Stocks", Schema);

            stockTableBuider
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            var stockItemRecordTableBuider = modelBuilder.Entity<StockItemRecord>().ToTable("StockItemRecords", Schema);

            stockItemRecordTableBuider
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            var eventLogTableBuilder = modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog", Schema);

            eventLogTableBuilder.HasKey(x => x.EventId);

            var requestEntryTableBuilder = modelBuilder.Entity<RequestEntry>().ToTable("RequestEntry", Schema);

            requestEntryTableBuilder.HasKey(x => x.Id);
        }
    }
}
