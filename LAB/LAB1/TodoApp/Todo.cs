using System;

namespace TodoApp
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"[{(IsCompleted ? "x" : " ")}] {Id} : {Title}";
        }

        public string ToFileString()
        {
            return $"{Id}|{IsCompleted}|{Title}";
        }

        public static Todo FromFileString(string line)
        {
            var parts = line.Split('|');
            return new Todo
            {
                Id = int.Parse(parts[0].Trim()),
                IsCompleted = bool.Parse(parts[1].Trim()),
                Title = parts[2].Trim()
            };
        }
    }
}
