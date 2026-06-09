using System.Linq.Expressions;

namespace WritingSessionsAspApi.Data.Contracts;

public interface IRecordRepo<TEntity> where TEntity : Model
{
    public Task<List<TEntity>> GetAllRecordsAsync();
    public Task<List<TEntity>> GetSelectRecordsAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] orderBy);
    
    public Task<TEntity?> GetRecordByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);
    
    public Task<TEntity?> GetRecordByCodeAsync(string code, params Expression<Func<TEntity, object>>[] includes);
    
    public Task<int> CreateRecordAsync(TEntity record);
    
    public Task<int> UpdateRecordAsync(TEntity record);
    
    public Task<int?> PutRecordAsync(TEntity recordData, int id);
    
    public Task<int> DeleteRecordAsync(TEntity record);
}