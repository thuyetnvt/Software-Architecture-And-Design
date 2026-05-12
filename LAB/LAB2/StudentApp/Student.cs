using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentApp
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name    { get; set; } = "";
        public string Email   { get; set; } = "";
        public string Address { get; set; } = "";
        public int    Age     { get; set; }
        public double Grade   { get; set; }

        public string GetRank() => Grade switch
        {
            >= 9.0 => "A",
            >= 8.0 => "B",
            >= 6.5 => "C",
            >= 5.0 => "D",
            _      => "F"
        };

        // Hiển thị Id rút gọn (8 ký tự đầu) cho dễ nhìn
        public override string ToString() =>
            $"[{Id?[..8]}] {Name,-20} | {Email,-25} | {Address,-15} | {Age,2} tuổi | Điểm: {Grade:F1} ({GetRank()})";
    }
}
