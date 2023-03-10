using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
  public class BasketRepository : IBasketRepository
  {
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer redis)
    {
      _database = redis.GetDatabase();
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
    {
      var isDeleted = await _database.KeyDeleteAsync(basketId);
      return isDeleted;
    }

    public async Task<CustomerBasket> GetBasketAsync(string basketId)
    {
      var data = await _database.StringGetAsync(basketId);
      var basket = data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
      return basket;
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
      var created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

      if (!created) return null;

      var updatedBasket = await GetBasketAsync(basket.Id);
      return updatedBasket;
    }
  }
}
