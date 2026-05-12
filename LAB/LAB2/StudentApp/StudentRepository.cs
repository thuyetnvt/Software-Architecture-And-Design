using MongoDB.Driver;

namespace StudentApp
{
    /// <summary>
    /// DATA LAYER - Thao tác CRUD với MongoDB thông qua MongoDB.Driver
    /// </summary>
    public class StudentRepository
    {
        private readonly IMongoCollection<Student> _collection;

        public StudentRepository(string connectionString,
                                 string databaseName   = "StudentDB",
                                 string collectionName = "Students")
        {
            var client   = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection  = database.GetCollection<Student>(collectionName);
        }

        // ── Lấy toàn bộ danh sách ──
        public async Task<List<Student>> GetAllAsync()
        {
            var result = await _collection.FindAsync(FilterDefinition<Student>.Empty);
            return await result.ToListAsync();
        }

        // ── Lấy theo Id (ObjectId string) ──
        public async Task<Student?> GetByIdAsync(string id)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Id, id);
            var result = await _collection.FindAsync(filter);
            return await result.FirstOrDefaultAsync();
        }

        // ── Thêm sinh viên ──
        public async Task AddAsync(Student student)
        {
            await _collection.InsertOneAsync(student);
        }

        // ── Cập nhật sinh viên ──
        public async Task UpdateAsync(Student student)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Id, student.Id);
            await _collection.ReplaceOneAsync(filter, student);
        }

        // ── Xoá sinh viên ──
        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        // ────────────── SEARCH ──────────────

        public async Task<List<Student>> SearchByIdAsync(string id)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Id, id);
            var result = await _collection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async Task<List<Student>> SearchByNameAsync(string keyword)
        {
            var filter = Builders<Student>.Filter.Regex(s => s.Name,
                new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            var result = await _collection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async Task<List<Student>> SearchByAddressAsync(string keyword)
        {
            var filter = Builders<Student>.Filter.Regex(s => s.Address,
                new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            var result = await _collection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async Task<List<Student>> SearchByGradeRankAsync(string rank)
        {
            FilterDefinition<Student> filter = rank.ToUpper() switch
            {
                "A" => Builders<Student>.Filter.Gte(s => s.Grade, 9.0),
                "B" => Builders<Student>.Filter.And(
                           Builders<Student>.Filter.Gte(s => s.Grade, 8.0),
                           Builders<Student>.Filter.Lt(s => s.Grade, 9.0)),
                "C" => Builders<Student>.Filter.And(
                           Builders<Student>.Filter.Gte(s => s.Grade, 6.5),
                           Builders<Student>.Filter.Lt(s => s.Grade, 8.0)),
                "D" => Builders<Student>.Filter.And(
                           Builders<Student>.Filter.Gte(s => s.Grade, 5.0),
                           Builders<Student>.Filter.Lt(s => s.Grade, 6.5)),
                "F" => Builders<Student>.Filter.Lt(s => s.Grade, 5.0),
                _   => Builders<Student>.Filter.Eq(s => s.Id, "invalid")
            };

            var sort   = Builders<Student>.Sort.Descending(s => s.Grade);
            var result = await _collection.FindAsync(filter,
                             new FindOptions<Student> { Sort = sort });
            return await result.ToListAsync();
        }
    }
}
