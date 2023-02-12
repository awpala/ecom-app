using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
  {
    private readonly StoreContext _context;

    public GenericRepository(StoreContext context)
    {
      _context = context;
    }

    public async Task<T> GetByIdAsync(int id)
    {
      var entity = await _context.Set<T>().FindAsync(id);
      return entity;
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
      var entities = await _context.Set<T>().ToListAsync();
      return entities;
    }

    public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
    {
      var entity = await ApplySpecification(spec).FirstOrDefaultAsync();
      return entity;
    }

    public async  Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
      var entities = await ApplySpecification(spec).ToListAsync();
      return entities;
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
      var count = await ApplySpecification(spec).CountAsync();
      return count;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
      return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
  }
}
