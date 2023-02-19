using Core.Entities;

namespace Core.Interfaces
{
  /*
    N.B. A separate interface is required for the basket, since it is
    using Redis than Entity Framework (cf. `./IGenericRepository`)
  */
  public interface IBasketRepository
  {
    Task<CustomerBasket> GetBasketAsync(string basketId);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task<bool> DeleteBasketAsync(string basketId);
  }
}
