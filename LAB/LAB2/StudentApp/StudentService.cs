namespace StudentApp
{
    /// <summary>
    /// LOGIC LAYER - Validation và nghiệp vụ trước khi gọi Repository
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly StudentRepository _repository;

        public StudentService(StudentRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Student>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Student?> GetByIdAsync(string id) => _repository.GetByIdAsync(id);

        public async Task<(bool Success, string Message, Student? Student)> AddAsync(
            string name, string email, string address, int age, double grade)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Tên sinh viên không được để trống.", null);
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return (false, "Email không hợp lệ.", null);
            if (age < 16 || age > 60)
                return (false, "Tuổi phải từ 16 đến 60.", null);
            if (grade < 0 || grade > 10)
                return (false, "Điểm phải từ 0 đến 10.", null);

            var student = new Student
            {
                Name    = name.Trim(),
                Email   = email.Trim(),
                Address = address.Trim(),
                Age     = age,
                Grade   = grade
            };

            await _repository.AddAsync(student);  // MongoDB tự gán Id (ObjectId)
            return (true, "Thêm sinh viên thành công!", student);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            string id, string name, string email, string address, int age, double grade)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return (false, "Không tìm thấy sinh viên.");

            if (string.IsNullOrWhiteSpace(name))
                return (false, "Tên sinh viên không được để trống.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return (false, "Email không hợp lệ.");
            if (age < 16 || age > 60)
                return (false, "Tuổi phải từ 16 đến 60.");
            if (grade < 0 || grade > 10)
                return (false, "Điểm phải từ 0 đến 10.");

            existing.Name    = name.Trim();
            existing.Email   = email.Trim();
            existing.Address = address.Trim();
            existing.Age     = age;
            existing.Grade   = grade;

            await _repository.UpdateAsync(existing);
            return (true, "Cập nhật thông tin thành công!");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return (false, "Không tìm thấy sinh viên.");

            await _repository.DeleteAsync(id);
            return (true, $"Đã xoá sinh viên [{existing.Name}].");
        }

        public Task<List<Student>> SearchByIdAsync(string id)           => _repository.SearchByIdAsync(id);
        public Task<List<Student>> SearchByNameAsync(string keyword)    => _repository.SearchByNameAsync(keyword);
        public Task<List<Student>> SearchByAddressAsync(string keyword) => _repository.SearchByAddressAsync(keyword);
        public Task<List<Student>> SearchByGradeRankAsync(string rank)  => _repository.SearchByGradeRankAsync(rank);
    }
}
