using ApiTFG.Data;
using ApiTFG.Repository.Contracts;
using ApiTFG.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
            catch (Exception ex)
            {
                throw new GenericException("Error occurred while creating the entity", ex);
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
            catch (Exception ex)
            {
                throw new GenericException("Error occurred while deleting the entity", ex);
            }
        }

        public async Task<TModel> Get(Expression<Func<TModel, bool>> filter)
        {
            try
            {
                TModel model = await _context.Set<TModel>().Where(filter).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw new GenericException("Error occurred while retrieving the entity", ex);
            }
        }

        public async Task<IQueryable<TModel>> Query(Expression<Func<TModel, bool>> filter = null)
        {
            try
            {
                IQueryable<TModel> queryModel = filter == null ? _context.Set<TModel>() : _context.Set<TModel>().Where(filter);
                return queryModel;
            }
            catch (Exception ex)
            {
                throw new GenericException("Error occurred while querying the entity", ex);
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
            catch (Exception ex)
            {
                throw new GenericException("Error occurred while updating the entity", ex);
            }
        }

        public async Task<List<TModel>> GetAll()
        {
            try
            {
                return await _context.Set<TModel>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new GenericException("Error occurred while retrieving all entities", ex);
            }
        }
    }
}