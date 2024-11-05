namespace MiturNetApplication.Interfaces;
public interface IRepositoryBase<T> where T : class
{
    //Queries
    IQueryable<T> GetAll();
    IQueryable<T> FindBy(Expression<Func<T, bool>> whereCondition);

    //Enumerables  
    IEnumerable<T> GetAllByCondiction(Expression<Func<T, bool>> whereCondition);
    Task<IEnumerable<T>> GetAllEnumerable();
    bool Exists(Expression<Func<T, bool>> whereCondition);

    //Async Method  
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> FindAsync(params Object[] key);
    Task<T> FirstAsync(Expression<Func<T, bool>> whereCondition);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereCondition);
    Task UpdateAsync(T entity);

    void Delete(T entity);
    void Delete(params object[] keys);

    //Range Method
    Task InsertRange(IEnumerable<T> entities);
    Task RemoveRange(IEnumerable<T> entities);


    //Store Procedure Method
    Task<IEnumerable<T>> ExecWithStoreProcedure(string procedure, params object[] parameters);
    Task<IEnumerable<T>> ExecWithStoreProcedure(string procedure);
    Task<IAsyncEnumerable<T>> ExcuteStoreProcedure(string procedure, params object[] parameters);
    Task<T> ExcuteStoreProcedureScalar(string procedure, params object[] parameters);
    Task<int> ExecProcedureNoReturn(string procedure, params object[] parameters);

    //Save Method
    void SaveChanges();
    Task SaveChangesAsync();
}
