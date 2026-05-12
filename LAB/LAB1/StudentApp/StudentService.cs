using System.Collections.Generic;

namespace StudentApp
{
    /// <summary>
    /// LOGIC LAYER - Nghiệp vụ, validate, trung gian UI <-> Data
    /// </summary>
    public class StudentService
    {
        private readonly StudentRepository _repo = new();

        // READ
        public List<Student> GetAll() => _repo.GetAll();
        public Student? GetById(int id) => _repo.GetById(id);

        // CREATE
        public (bool success, string message, Student? student) AddStudent(
            string name, string email, string address, int age, double grade)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Tên không được để trống.", null);
            if (age < 1 || age > 120)
                return (false, "Tuổi không hợp lệ (1 - 120).", null);
            if (grade < 0 || grade > 10)
                return (false, "Điểm không hợp lệ (0 - 10).", null);

            var student = _repo.Add(name, email, address, age, grade);
            return (true, "Thêm sinh viên thành công!", student);
        }

        // UPDATE
        public (bool success, string message) EditStudent(
            int id, string name, string email, string address, int age, double grade)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Tên không được để trống.");
            if (age < 1 || age > 120)
                return (false, "Tuổi không hợp lệ (1 - 120).");
            if (grade < 0 || grade > 10)
                return (false, "Điểm không hợp lệ (0 - 10).");

            bool ok = _repo.Update(id, name, email, address, age, grade);
            return ok ? (true, "Cập nhật thành công!") : (false, "Không tìm thấy sinh viên.");
        }

        // DELETE
        public (bool success, string message) RemoveStudent(int id)
        {
            bool ok = _repo.Delete(id);
            return ok ? (true, "Đã xoá thành công!") : (false, "Không tìm thấy sinh viên.");
        }

        // SEARCH
        public List<Student> SearchById(int id) => _repo.SearchById(id);
        public List<Student> SearchByName(string name) => _repo.SearchByName(name);
        public List<Student> SearchByAddress(string address) => _repo.SearchByAddress(address);
        public List<Student> SearchByGrade(string grade) => _repo.SearchByGrade(grade);
    }
}
