using API.Carts.Domain.Entities;
using API.Carts.Domain.Interfaces;
using API.Carts.Infrastructure.MessagePack;
using MessagePack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Carts.Infrastructure
{
    //public class RedisCartsRepository : ICartsRepository
    //{
    //    private readonly ILogger<RedisCartsRepository> _logger;
    //    private readonly ConnectionMultiplexer _redis;
    //    private readonly IDatabase _database;

    //    public RedisCartsRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
    //    {
    //        _logger = loggerFactory.CreateLogger<RedisCartsRepository>();
    //        _redis = redis;
    //        _database = redis.GetDatabase();
    //    }

    //    public async Task<bool> DeleteAsync(string id)
    //    {
    //        return await _database.KeyDeleteAsync(id);
    //    }

    //    public IEnumerable<string> GetUsers()
    //    {
    //        var server = GetServer();
    //        var data = server.Keys();

    //        return data?.Select(k => k.ToString());
    //    }

    //    public async Task<Cart> GetAsync(int customerId)
    //    {
    //        var data = await _database.StringGetAsync(customerId.ToString());

    //        if (data.IsNullOrEmpty)
    //        {
    //            return null;
    //        }

    //        var mspCart = MessagePackSerializer.Deserialize<CartMessagePackModel>(data);
    //        var cart = ConvertToEntity(mspCart);

    //        return cart;
    //    }

    //    public async Task<Cart> UpdateAsync(Cart cart)
    //    {
    //        var mspCart = ConvertToMspModel(cart);

    //        byte[] bytes = MessagePackSerializer.Serialize(mspCart);
    //        var created = await _database.StringSetAsync(mspCart.CustomerId.ToString(), bytes);

    //        if (!created)
    //        {
    //            _logger.LogInformation("Problem occur persisting the item.");
    //            return null;
    //        }

    //        _logger.LogInformation("Cart item persisted succesfully.");

    //        return await GetAsync(cart.CustomerId);
    //    }

    //    private IServer GetServer()
    //    {
    //        var endpoint = _redis.GetEndPoints();
    //        return _redis.GetServer(endpoint.First());
    //    }

    //    private CartMessagePackModel ConvertToMspModel(Cart cart)
    //    {
    //        var mspCart = new CartMessagePackModel
    //        {
    //            CustomerId = cart.CustomerId,
    //            TotalPrice = cart.TotalPrice,
    //            Items = cart.CartItems.Select(x => new CartItemMessagePackModel
    //            {
    //                ProductId = x.ProductId,
    //                ProductName = x.ProductName,
    //                UnitPrice = x.UnitPrice,
    //                Quantity = x.Quantity,
    //                PictureUrl = x.PictureUrl
    //            }).ToList()
    //        };

    //        return mspCart;
    //    }

    //    private Cart ConvertToEntity(CartMessagePackModel model)
    //    {
    //        var cartItems = model.Items.Select(x => ConvertToEntity(x)).ToList();
    //        var cart = new Cart(model.CustomerId, cartItems);

    //        return cart;
    //    }

    //    private CartItem ConvertToEntity(CartItemMessagePackModel model)
    //    {
    //        var cartItem = new CartItem(model.ProductId, model.ProductName, model.UnitPrice, model.Quantity, model.PictureUrl);
    //        return cartItem;
    //    }

    //    public Cart Add(Cart entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Update(Cart entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<List<Cart>> GetAllCartsHavingProduct(int productId)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
