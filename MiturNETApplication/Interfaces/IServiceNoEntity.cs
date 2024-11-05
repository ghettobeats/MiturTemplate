namespace MiturNetApplication.Interfaces;
public interface IServiceNoEntity<T>
{
    //Queries
    IQueryable<T> GetAll();
    IQueryable<T> FindBy(Expression<Func<T, bool>> whereCondition);

    //Enumerables  
    IEnumerable<T> GetAllByCondiction(Expression<Func<T, bool>> whereCondition);
    Task<IEnumerable<T>> GetAllEnumerable();
    bool Exists(Expression<Func<T, bool>> whereCondition);

    //Async Method  
    Task AddAsync(T entity);
    Task<T> FindAsync(params Object[] key);
    Task<T> FirstAsync(Expression<Func<T, bool>> whereCondition);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereCondition);
    Task UpdateAsync(T entity);

    //Range Method
    Task InsertRange(IEnumerable<T> entities);
    Task RemoveRange(IEnumerable<T> entities);
    void Delete(T entity);
    void Delete(params object[] keys);

    //Store Procedure Method
    Task<IEnumerable<T>> ExecWithStoreProcedure(string procedure, params object[] parameters);
    Task<IEnumerable<T>> ExecWithStoreProcedure(string procedure);
    Task<IAsyncEnumerable<T>> ExcuteStoreProcedure(string procedure, params object[] parameters);
    Task<int> ExecProcedureNoReturn(string procedure, params object[] parameters);

    Task<T> ExcuteStoreProcedureScalar(string procedure, params object[] parameters);


    //Save Method
    void SaveChanges();
    Task SaveChangesAsync();

}
