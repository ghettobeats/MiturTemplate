namespace MiturNetApplication.Services;
public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected MiturNetContext _MiturNetDBContext { get; set; } = null;
    private readonly DbSet<T> _entities;

    public RepositoryBase(MiturNetContext MiturNetDBContext)
    {
        _MiturNetDBContext = MiturNetDBContext;
        _entities = _MiturNetDBContext.Set<T>();
    }
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _entities.AddAsync(entity);
        await SaveChangesAsync();
    }

    //Delete // doing soft
    public void Delete(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentException("Entity");
        }
        _entities.Remove(entity);
        SaveChanges();
    }

    public void Delete(params object[] keys)
    {
        throw new NotImplementedException();
    }


    //Query //
    public async Task<T> FindAsync(params object[] key)
    {
        return await _entities.FindAsync(key);
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>> whereCondition)
    {
        return _entities.Where(whereCondition).AsNoTracking();
    }

    public async Task<T> FirstAsync(Expression<Func<T, bool>> whereCondition)
    {
        return await _entities.FirstAsync(whereCondition);
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereCondition)
    {
        return await _entities.FirstOrDefaultAsync(whereCondition);
    }

    public IQueryable<T> GetAll()
    {       
        return _entities.AsNoTracking();
    }

    public IEnumerable<T> GetAllByCondiction(Expression<Func<T, bool>> whereCondition)
    {
        return _entities.Where(whereCondition).AsNoTracking();
        // return _entities.Where(whereCondition).AsEnumerable();
    }

    public async Task<IEnumerable<T>> GetAllEnumerable()
    {
        return await _entities.AsNoTracking().ToListAsync();
    }

    public async Task InsertRange(IEnumerable<T> entities)
    {
        await _entities.AddRangeAsync(entities);
        await SaveChangesAsync();
    }

    public async Task RemoveRange(IEnumerable<T> entities)
    {
        _entities.RemoveRange(entities);
        await SaveChangesAsync();
    }

    //Save Changes //
    public void SaveChanges()
    {
        _MiturNetDBContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _MiturNetDBContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        
        _entities.Update(entity);
        await SaveChangesAsync();
    }

    //Implemplements SP
    public async Task<IAsyncEnumerable<T>> ExcuteStoreProcedure(string procedure, params object[] parameters)
    {
        return _entities.FromSqlRaw<T>(procedure, parameters).AsNoTracking().AsAsyncEnumerable();
    }

    public async Task<IEnumerable<T>> ExecWithStoreProcedure(string procedure, params object[] parameters)
    {
        return await _entities.FromSqlRaw<T>(procedure, parameters).ToListAsync();
    }

    public async Task<IEnumerable<T>> ExecWithStoreProcedure(string procedure)
    {
        return await _entities.FromSqlRaw<T>(procedure).AsNoTracking().ToListAsync();
    }

    public bool Exists(Expression<Func<T, bool>> whereCondition) 
    {
        return _entities.Any(whereCondition);
    }


    public async Task<int> ExecProcedureNoReturn(string procedure, params object[] parameters)
    {
        return await _MiturNetDBContext.Database.ExecuteSqlRawAsync(procedure, parameters).ConfigureAwait(false);
    }        

    public async Task<T> ExcuteStoreProcedureScalar(string procedure, params object[] parameters)
    {
       return await _entities.FromSqlRaw<T>(procedure, parameters).FirstOrDefaultAsync();
    }
}
