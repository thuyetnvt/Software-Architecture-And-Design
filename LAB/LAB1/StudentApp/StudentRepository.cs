using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentApp
{
    /// <summary>
    /// DATA LAYER - Đọc/ghi dữ liệu ra file, CRUD cơ bản
    /// </summary>
    public class StudentRepository
    {
        private readonly List<Student> _students = new();
        private int _nextId = 1;
        private readonly string _filePath = "students.txt";

        public StudentRepository()
        {
            LoadFromFile();
        }

        // ──────────────────── READ ────────────────────
        public List<Student> GetAll() => _students;

        public Student? GetById(int id) =>
            _students.FirstOrDefault(s => s.Id == id);

        // ──────────────────── CREATE ────────────────────
        public Student Add(string name, string email, string address, int age, double grade)
        {
            var student = new Student
            {
                Id      = _nextId++,
                Name    = name,
                Email   = email,
                Address = address,
                Age     = age,
                Grade   = grade
            };
            _students.Add(student);
            SaveToFile();
            return student;
        }

        // ──────────────────── UPDATE ────────────────────
        public bool Update(int id, string name, string email, string address, int age, double grade)
        {
            var student = GetById(id);
            if (student == null) return false;

            student.Name    = name;
            student.Email   = email;
            student.Address = address;
            student.Age     = age;
            student.Grade   = grade;
            SaveToFile();
            return true;
        }

        // ──────────────────── DELETE ────────────────────
        public bool Delete(int id)
        {
            var student = GetById(id);
            if (student == null) return false;
            _students.Remove(student);
            SaveToFile();
            return true;
        }

        // ──────────────────── SEARCH ────────────────────
        public List<Student> SearchById(int id) =>
            _students.Where(s => s.Id == id).ToList();

        public List<Student> SearchByName(string name) =>
            _students.Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

        public List<Student> SearchByAddress(string address) =>
            _students.Where(s => s.Address.Contains(address, StringComparison.OrdinalIgnoreCase)).ToList();

        public List<Student> SearchByGrade(string gradeLetter) =>
            _students.Where(s => s.GetGradeLetter().Equals(gradeLetter, StringComparison.OrdinalIgnoreCase)).ToList();

        // ──────────────────── FILE I/O ────────────────────
        private void LoadFromFile()
        {
            if (!File.Exists(_filePath)) return;
            foreach (var line in File.ReadAllLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                try
                {
                    var student = Student.FromFileString(line);
                    _students.Add(student);
                    if (student.Id >= _nextId)
                        _nextId = student.Id + 1;
                }
                catch { /* bỏ qua dòng lỗi */ }
            }
        }

        private void SaveToFile()
        {
            File.WriteAllLines(_filePath, _students.Select(s => s.ToFileString()));
        }
    }
}
