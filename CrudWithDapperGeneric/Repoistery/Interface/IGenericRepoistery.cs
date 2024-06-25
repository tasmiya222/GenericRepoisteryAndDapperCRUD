namespace CrudWithDapperGeneric.Repoistery.Interface
{
    public interface IGenericRepoistery
    {
        public interface IGenericRepository<T> where T : class
        {
            Task<IEnumerable<T>> GetAllAsync();
            Task<T> GetByIdAsync(int id);
            Task<int> AddAsync(T entity);
            Task<int> UpdateAsync(T entity);
            Task<int> DeleteAsync(int id);
            Task<int> ExecuteStoredProcedureAsync(string procedureName, object parameters = null);
            Task<IEnumerable<T>> QueryStoredProcedureAsync(string procedureName, object parameters = null);
        }

    }
}
