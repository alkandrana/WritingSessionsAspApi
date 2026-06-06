using System.Linq.Expressions;

namespace WritingSessionsAspApi.Data.Contracts;

public interface IRecordRepo<TEntity> where TEntity : Model
{
    public Task<List<TEntity>> GetAllRecordsAsync();
    
    public Task<TEntity?> GetOneRecordAsync(int id, params Expression<Func<TEntity, object>>[] includes);
    
    public Task<int> CreateRecordAsync(TEntity record);
    
    public Task<int?> UpdateRecordAsync(TEntity recordData, int id);
    
    public Task<int?> DeleteRecordAsync(int id);
}