namespace StudentApp
{
    /// <summary>
    /// Grade: A (8.5-10), B (7.0-8.4), C (5.5-6.9), D (4.0-5.4), F (< 4.0)
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Age { get; set; }
        public double Grade { get; set; }

        public string GetGradeLetter()
        {
            return Grade switch
            {
                >= 8.5 => "A",
                >= 7.0 => "B",
                >= 5.5 => "C",
                >= 4.0 => "D",
                _ => "F"
            };
        }

        public override string ToString()
        {
            return $"[{Id}] {Name,-20} | Tuổi: {Age,3} | Email: {Email,-25} | Địa chỉ: {Address,-20} | Điểm: {Grade:F1} ({GetGradeLetter()})";
        }

        public string ToFileString()
        {
            return $"{Id}|{Name}|{Email}|{Address}|{Age}|{Grade}";
        }

        public static Student FromFileString(string line)
        {
            var parts = line.Split('|');
            return new Student
            {
                Id      = int.Parse(parts[0].Trim()),
                Name    = parts[1].Trim(),
                Email   = parts[2].Trim(),
                Address = parts[3].Trim(),
                Age     = int.Parse(parts[4].Trim()),
                Grade   = double.Parse(parts[5].Trim())
            };
        }
    }
}
