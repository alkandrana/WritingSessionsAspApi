using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace WritingSessionsAspApi.Data.Contracts;

public class RecordRepo<TEntity> : IRecordRepo<TEntity> where TEntity : Model
{
    private readonly AppDbContext _ctx;
    private readonly DbSet<TEntity> _dbSet;

    public RecordRepo(AppDbContext ctx)
    {
        _ctx = ctx;
        _dbSet = _ctx.Set<TEntity>();
    }
    
    public async Task<List<TEntity>> GetAllRecordsAsync()
    {
        List<TEntity> records = await _dbSet.ToListAsync();
        return records;
    }

    public async Task<List<TEntity>> GetSelectRecordsAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }
        List<TEntity> records = await query.ToListAsync();
        return records;
    }

    public async Task<TEntity?> GetRecordByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        TEntity? record = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        return record;
    }

    public async Task<TEntity?> GetRecordByCodeAsync(string code, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        TEntity? record = await query.FirstOrDefaultAsync(e => EF.Property<string>(e, "Code") == code);
        return record;
    }

    public async Task<int> CreateRecordAsync(TEntity record)
    {
        _dbSet.Add(record);
        return await _ctx.SaveChangesAsync();
    }

    public async Task<int?> PutRecordAsync(TEntity recordData, int id)
    {
        TEntity? recordToUpdate = await _dbSet.FindAsync(id);
        if (recordToUpdate == null)
        {
            return null;
        }
        recordData.ApplyTo(recordToUpdate);
        _dbSet.Update(recordToUpdate);
        return await _ctx.SaveChangesAsync();
    }

    public async Task<int> UpdateRecordAsync(TEntity record)
    {
        _dbSet.Update(record);
        return await _ctx.SaveChangesAsync();
    }
    public async Task<int> DeleteRecordAsync(TEntity record)
    {
        _dbSet.Remove(record);
        return await _ctx.SaveChangesAsync();
    }
}