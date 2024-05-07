using ApiTFG.Data;
using ApiTFG.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiTFG.Repository
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TModel> Create(TModel model)
        {
            try
            {
                _context.Set<TModel>().Add(model);
                await _context.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(TModel model)
        {
            try
            {
                _context.Set<TModel>().Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }


        public async Task<TModel> Get(Expression<Func<TModel, bool>> filter)
        {
            try
            {
                TModel model = await _context.Set<TModel>().Where(filter).FirstOrDefaultAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TModel>> Query(Expression<Func<TModel, bool>> filter = null)
        {
            try
            {
                IQueryable<TModel> queryModel = filter == null ? _context.Set<TModel>() : _context.Set<TModel>().Where(filter);
                return queryModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TModel> Update(TModel model)
        {
            try
            {
                _context.Set<TModel>().Update(model);
                await _context.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TModel>> GetAll()
        {
            try
            {
                return await _context.Set<TModel>().ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
