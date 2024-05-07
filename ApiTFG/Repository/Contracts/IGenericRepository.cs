using System.Linq.Expressions;

namespace ApiTFG.Repository.Contracts
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<TModel> Get(Expression<Func<TModel, bool>> filter);
        Task<TModel> Create(TModel model);
        Task<TModel> Update(TModel model);
        Task<bool> Delete(TModel model);
        Task<IQueryable<TModel>> Query(Expression<Func<TModel,bool>> filter = null);
        Task<List<TModel>> GetAll();
    }
}
