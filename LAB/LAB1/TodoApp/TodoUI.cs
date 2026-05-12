using System;

namespace TodoApp
{
    public class TodoUI
    {
        private readonly TodoService _todoService = new();

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                ShowTodos();
                ShowMenu();
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddTodo(); break;
                    case "2": DeleteTodo(); break;
                    case "3": ToggleTodo(); break;
                    case "4": EditTodo(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        break;
                }
                Console.WriteLine("\nNhấn Enter để tiếp tục...");
                Console.ReadLine();
            }
        }

        private void ShowTodos()
        {
            var todos = _todoService.GetTodos();
            Console.WriteLine("=== DANH SÁCH CÔNG VIỆC ===");
            if (todos.Count == 0)
            {
                Console.WriteLine("Chưa có công việc nào.");
            }
            else
            {
                foreach (var todo in todos)
                    Console.WriteLine(todo);
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\nChức năng:");
            Console.WriteLine("1. Thêm Todo");
            Console.WriteLine("2. Xoá Todo");
            Console.WriteLine("3. Đánh dấu hoàn thành");
            Console.WriteLine("4. Sửa nội dung");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");
        }

        private void AddTodo()
        {
            Console.Write("Nhập nội dung công việc: ");
            string? title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
                _todoService.AddTodo(title);
        }

        private void DeleteTodo()
        {
            Console.Write("Nhập ID công việc cần xoá: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bool result = _todoService.RemoveTodo(id);
                Console.WriteLine(result ? "Đã xoá thành công!" : "Không tìm thấy ID.");
            }
        }

        private void ToggleTodo()
        {
            Console.Write("Nhập ID cần đánh dấu hoàn thành: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bool result = _todoService.ToggleTodo(id);
                Console.WriteLine(result ? "Đã cập nhật trạng thái!" : "Không tìm thấy ID.");
            }
        }

        private void EditTodo()
        {
            Console.Write("Nhập ID cần sửa: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Nhập nội dung mới: ");
                var newTitle = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    bool result = _todoService.EditTodo(id, newTitle);
                    Console.WriteLine(result ? "Đã cập nhật!" : "Không tìm thấy ID.");
                }
            }
        }
    }
}
