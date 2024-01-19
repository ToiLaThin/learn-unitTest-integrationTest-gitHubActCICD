namespace ApiUnitTesting.Api.Repo
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(object id);

        IEnumerable<T> GetAll();

        IQueryable<T> GetAsQueryable();

        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}
