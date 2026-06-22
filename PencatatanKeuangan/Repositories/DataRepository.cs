using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PencatatanKeuangan.Repositories
{
    public class DataRepository<T> where T : class
    {
        private List<T> _dataList = new List<T>();
        private readonly string _filePath;

        private string GetProjectRoot()
        {
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            System.IO.DirectoryInfo? dir = new System.IO.DirectoryInfo(baseDir);
            while (dir != null)
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(dir.FullName, "MonTrack.sln")) ||
                    dir.Name.Equals("tubes-KPL", System.StringComparison.OrdinalIgnoreCase))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;
            }
            return baseDir;
        }

        public DataRepository(string fileName = "transactions.json")
        {
            // Simpan di dalam folder proyek agar rapi
            string projectRoot = GetProjectRoot();
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

        public void Save()
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