﻿using Microsoft.EntityFrameworkCore;
using Ordering.API.Domain.Entities;
using Ordering.API.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ordering.API.Infrastructure
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingContext _context;

        public OrderRepository(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Order Add(Order order)
        {
            _context.Orders.Add(order);
            return order;
        }

        public async Task<Order> GetAsync(int orderId)
        {
            var order = await _context
                .Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return order;
        }

        public void Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }
    }
}
