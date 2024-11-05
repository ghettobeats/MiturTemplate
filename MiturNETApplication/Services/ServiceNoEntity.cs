namespace MiturNetApplication.Services;
public class ServiceNoEntity<T> : IServiceNoEntity<T> where T : class
{
    private readonly IRepositoryBase<T> _repoBase;
    public ServiceNoEntity(IRepositoryBase<T> repositoryBase)
    {
        _repoBase = repositoryBase;
    }
    public async Task AddAsync(T entity)
    {
        await _repoBase.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(params object[] keys)
    {
        throw new NotImplementedException();
    }

    public async Task<T> FindAsync(params object[] key)
    {
        return await _repoBase.FindAsync(key);
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>> whereCondition)
    {
        return _repoBase.FindBy(whereCondition);
    }

    public async Task<T> FirstAsync(Expression<Func<T, bool>> whereCondition)
    {
        return await _repoBase.FirstAsync(whereCondition);
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereCondition)
    {
        return await _repoBase.FirstOrDefaultAsync(whereCondition);
    }

    public IQueryable<T> GetAll()
    {
        return _repoBase.GetAll();
    }

    public IEnumerable<T> GetAllByCondiction(Expression<Func<T, bool>> whereCondition)
    {
       return _repoBase.GetAllByCondiction(whereCondition);
    }

    public Task<IEnumerable<T>> GetAllEnumerable()
    {
        return _repoBase.GetAllEnumerable();
    }

    public  Task InsertRange(IEnumerable<T> entities)
    {
        return _repoBase.InsertRange(entities);
    }

    public Task RemoveRange(IEnumerable<T> entities)
    {
        return _repoBase.RemoveRange(entities);
    }

    public async Task<IAsyncEnumerable<T>> ExcuteStoreProcedure(string procedure, params object[] parameters)
    {
        return await _repoBase.ExcuteStoreProcedure(procedure, parameters);
    }

    public async Task<IEnumerable<T>> ExecWithStoreProcedure(string procedure, params object[] parameters)
    {
        return await _repoBase.ExecWithStoreProcedure(procedure, parameters);
    }

    public Task<IEnumerable<T>> ExecWithStoreProcedure(string procedure)
    {
        return _repoBase.ExecWithStoreProcedure(procedure);
    }

    

    public void SaveChanges()
    {
        _repoBase.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _repoBase.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        await _repoBase.UpdateAsync(entity);
    }

    public bool Exists(Expression<Func<T, bool>> whereCondition)
    {
        return _repoBase.Exists(whereCondition);
    }

    public async Task<int> ExecProcedureNoReturn(string procedure, params object[] parameters)
    {
      return await _repoBase.ExecProcedureNoReturn(procedure, parameters); 
    }

    public async Task<T> ExcuteStoreProcedureScalar(string procedure, params object[] parameters)
    {
       return await _repoBase.ExcuteStoreProcedureScalar(procedure, parameters);
    }
}
