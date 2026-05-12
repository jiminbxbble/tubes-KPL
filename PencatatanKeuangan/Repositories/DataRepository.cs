using System.Collections.Generic;

namespace PencatatanKeuangan.Repositories
{
    // Teknik: Parameterization / Generics
    public class DataRepository<T> where T : class
    {
        private readonly List<T> _dataList = new List<T>();

        public void Add(T item)
        {
            _dataList.Add(item);
        }

        public IEnumerable<T> GetAll()
        {
            return _dataList;
        }
    }
}