﻿using Infrastructure.Base.EventLog;
using Infrastructure.Base.RequestManager;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Domain.Entities;

namespace Ordering.API.Infrastructure
{
    public class OrderingContext : DbContext, IIntegrationEventDbContext, IRequestManagerDbContext
    {
        public const string Schema = "ms_ordering";

        public OrderingContext(DbContextOptions<OrderingContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<IntegrationEventLogEntry> EventLogEntries { get; set; }
        public virtual DbSet<RequestEntry> RequestEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var orderTableBuider = modelBuilder.Entity<Order>().ToTable("Orders", Schema);

            orderTableBuider
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            orderTableBuider
                .Property(p => p.Amount)
                .HasColumnType("decimal(20,5)");

            var orderItemTableBuilder = modelBuilder.Entity<OrderItem>().ToTable("OrderItems", Schema);

            orderItemTableBuilder
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(20,5)");

            orderItemTableBuilder
                .Property(p => p.SubTotal)
                .HasColumnType("decimal(20,5)");

            var productTableBuilder = modelBuilder.Entity<Product>().ToTable("Products", Schema);

            productTableBuilder
                .Property(p => p.Price)
                .HasColumnType("decimal(20,5)");

            productTableBuilder
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            productTableBuilder
                .Property(p => p.Id)
                .ValueGeneratedNever();

            var eventLogTableBuilder = modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog", Schema);

            eventLogTableBuilder.HasKey(x => x.EventId);

            var requestEntryTableBuilder = modelBuilder.Entity<RequestEntry>().ToTable("RequestEntry", Schema);

            requestEntryTableBuilder.HasKey(x => x.Id);
        }
    }
}
