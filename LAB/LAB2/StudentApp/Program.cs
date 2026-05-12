using System.Text;
using StudentApp;

public class Program
{
    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding  = Encoding.UTF8;

        // MongoDB chạy mặc định ở localhost:27017 (không cần tạo DB/Collection trước)
        const string connStr = "mongodb://localhost:27017";

        var repository = new StudentRepository(connStr, "StudentDB", "Students");
        var service    = new StudentService(repository);
        var ui         = new StudentUI(service);

        await ui.Run();
    }
}
