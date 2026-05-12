using System.Text;
using StudentApp;

public class Program
{
    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding  = Encoding.UTF8;

        const string connStr = "mongodb+srv://thuyet1230_db_user:%40Thuyet22082005@cluster0.idbloqs.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        var repository = new StudentRepository(connStr, "StudentDB", "Students");
        var service    = new StudentService(repository);
        var ui         = new StudentUI(service);

        await ui.Run();
    }
}
