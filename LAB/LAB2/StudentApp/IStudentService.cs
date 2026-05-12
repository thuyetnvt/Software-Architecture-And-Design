namespace StudentApp
{
    /// <summary>
    /// Interface định nghĩa contract cho service layer (dùng string Id cho MongoDB ObjectId)
    /// </summary>
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(string id);
        Task<(bool Success, string Message, Student? Student)> AddAsync(
            string name, string email, string address, int age, double grade);
        Task<(bool Success, string Message)> UpdateAsync(
            string id, string name, string email, string address, int age, double grade);
        Task<(bool Success, string Message)> DeleteAsync(string id);

        // Search
        Task<List<Student>> SearchByIdAsync(string id);
        Task<List<Student>> SearchByNameAsync(string keyword);
        Task<List<Student>> SearchByAddressAsync(string keyword);
        Task<List<Student>> SearchByGradeRankAsync(string rank);
    }
}
