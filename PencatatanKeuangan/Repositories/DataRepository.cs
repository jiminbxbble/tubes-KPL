using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PencatatanKeuangan.Repositories
{
    public class DataRepository<T> where T : class
    {
        private List<T> _dataList = new List<T>();
        private readonly string _filePath;

        public DataRepository(string fileName = "transactions.json")
        {
            // Simpan di dalam folder proyek agar rapi
            string projectRoot = @"d:\4. Thoriq_KULIAH\4. Matkul\Semester 4\LKPL\TUBES-Thoriq\tubes-KPL";
            string folder = Path.Combine(projectRoot, "_Output", "Database");
            Directory.CreateDirectory(folder);
            _filePath = Path.Combine(folder, fileName);
            Load();
        }

        public void Add(T item)
        {
            _dataList.Add(item);
            Save();
        }

        public List<T> GetAll()
        {
            return _dataList;
        }

        private void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_dataList, options);
            File.WriteAllText(_filePath, json);
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _dataList = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
        }
    }
}