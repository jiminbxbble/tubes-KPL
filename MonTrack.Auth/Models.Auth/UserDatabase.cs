using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BCrypt.Net;

namespace MonTrack.Auth.Models
{
    public static class UserDatabase
    {
        private static List<User> _users;
        private static readonly string _filePath;

        static UserDatabase()
        {
            // Simpan di dalam folder proyek agar rapi
            string projectRoot = @"d:\4. Thoriq_KULIAH\4. Matkul\Semester 4\LKPL\TUBES-Thoriq\tubes-KPL";
            string folder = Path.Combine(projectRoot, "_Output", "Database");
            Directory.CreateDirectory(folder);
            _filePath = Path.Combine(folder, "users.json");
            
            _users = Load();
            
            // Seed default users if file is empty
            if (_users.Count == 0)
            {
                _users.Add(new User(1, "rosa@email.com", BCrypt.Net.BCrypt.HashPassword("password123"), true));
                _users.Add(new User(2, "raissha@email.com", BCrypt.Net.BCrypt.HashPassword("mypassword"), false));
                _users.Add(new User(3, "putri@email.com", BCrypt.Net.BCrypt.HashPassword("putri1234"), true));
                Save();
            }
        }

        public static List<User> GetUsers() => _users;

        public static User? FindByEmail(string email) => _users.FirstOrDefault(u => u.Email == email);

        public static void ResetToDefault()
        {
            _users.Clear();
            _users.Add(new User(1, "rosa@email.com", BCrypt.Net.BCrypt.HashPassword("password123"), true));
            _users.Add(new User(2, "raissha@email.com", BCrypt.Net.BCrypt.HashPassword("mypassword"), false));
            _users.Add(new User(3, "putri@email.com", BCrypt.Net.BCrypt.HashPassword("putri1234"), true));
            Save();
        }

        public static void AddUser(User user)
        {
            _users.Add(user);
            Save();
        }

        public static void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_users, options);
            File.WriteAllText(_filePath, json);
        }

        private static List<User> Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            return new List<User>();
        }
    }
}