namespace StudentApp
{
    /// <summary>
    /// UI LAYER - Tất cả tương tác console, không chứa logic nghiệp vụ
    /// </summary>
    public class StudentUI
    {
        private readonly IStudentService _service;

        public StudentUI(IStudentService service)
        {
            _service = service;
        }

        public async Task Run()
        {
            while (true)
            {
                Console.Clear();
                ShowHeader("QUẢN LÝ SINH VIÊN");
                await ShowAllStudents();
                ShowMainMenu();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": await AddStudent();    break;
                    case "2": await EditStudent();   break;
                    case "3": await DeleteStudent(); break;
                    case "4": await SearchMenu();    break;
                    case "0":
                        Console.WriteLine("\n  Tạm biệt!");
                        return;
                    default:
                        PrintError("Lựa chọn không hợp lệ!");
                        break;
                }

                Pause();
            }
        }

        // ──────────────────── MENU ────────────────────
        private void ShowMainMenu()
        {
            Console.WriteLine("\n  Chức năng:");
            Console.WriteLine("  1. Thêm sinh viên");
            Console.WriteLine("  2. Sửa thông tin sinh viên");
            Console.WriteLine("  3. Xoá sinh viên");
            Console.WriteLine("  4. Tìm kiếm sinh viên");
            Console.WriteLine("  0. Thoát");
            Console.Write("\nChọn: ");
        }

        // ──────────────────── DISPLAY ────────────────────
        private async Task ShowAllStudents()
        {
            var list = await _service.GetAllAsync();
            ShowHeader("DANH SÁCH SINH VIÊN");
            if (list.Count == 0)
            {
                Console.WriteLine("  (Chưa có sinh viên nào.)");
                return;
            }
            PrintSeparator();
            foreach (var s in list)
                Console.WriteLine("  " + s);
            PrintSeparator();
            Console.WriteLine($"  Tổng: {list.Count} sinh viên.");
        }

        private void PrintList(List<Student> list, string emptyMsg = "Không tìm thấy sinh viên nào.")
        {
            if (list.Count == 0) { PrintError(emptyMsg); return; }
            PrintSeparator();
            foreach (var s in list)
                Console.WriteLine("  " + s);
            PrintSeparator();
            Console.WriteLine($"  Tìm thấy: {list.Count} kết quả.");
        }

        // ──────────────────── ADD ────────────────────
        private async Task AddStudent()
        {
            ShowHeader("THÊM SINH VIÊN MỚI");
            string name    = ReadString("Tên sinh viên");
            string email   = ReadString("Email");
            string address = ReadString("Địa chỉ");
            int    age     = ReadInt("Tuổi");
            double grade   = ReadDouble("Điểm (0-10)");

            var (success, message, student) = await _service.AddAsync(name, email, address, age, grade);
            if (success)
                PrintSuccess($"{message} -> ID: {student!.Id}");
            else
                PrintError(message);
        }

        // ──────────────────── EDIT ────────────────────
        private async Task EditStudent()
        {
            ShowHeader("SỬA THÔNG TIN SINH VIÊN");
            string id = ReadString("Nhập ID cần sửa (copy từ danh sách)");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) { PrintError("Không tìm thấy sinh viên."); return; }

            Console.WriteLine($"  Thông tin hiện tại: {existing}");
            Console.WriteLine("  (Nhấn Enter để giữ nguyên giá trị cũ)\n");

            string name    = ReadStringOptional("Tên mới",    existing.Name);
            string email   = ReadStringOptional("Email mới",  existing.Email);
            string address = ReadStringOptional("Địa chỉ mới", existing.Address);
            int    age     = ReadIntOptional("Tuổi mới",      existing.Age);
            double grade   = ReadDoubleOptional("Điểm mới",   existing.Grade);

            var (success, message) = await _service.UpdateAsync(id, name, email, address, age, grade);
            if (success) PrintSuccess(message);
            else         PrintError(message);
        }

        // ──────────────────── DELETE ────────────────────
        private async Task DeleteStudent()
        {
            ShowHeader("XOÁ SINH VIÊN");
            string id = ReadString("Nhập ID cần xoá (copy từ danh sách)");
            Console.Write("  Bạn chắc chắn muốn xoá? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y") return;

            var (success, message) = await _service.DeleteAsync(id);
            if (success) PrintSuccess(message);
            else         PrintError(message);
        }

        // ──────────────────── SEARCH ────────────────────
        private async Task SearchMenu()
        {
            Console.Clear();
            ShowHeader("TÌM KIẾM SINH VIÊN");
            Console.WriteLine("  1. Tìm theo ID");
            Console.WriteLine("  2. Tìm theo Tên");
            Console.WriteLine("  3. Tìm theo Địa chỉ");
            Console.WriteLine("  4. Tìm theo Xếp loại (A/B/C/D/F)");
            Console.Write("\nChọn: ");

            switch (Console.ReadLine())
            {
                case "1":
                    string searchId = ReadString("Nhập ID");
                    PrintList(await _service.SearchByIdAsync(searchId));
                    break;
                case "2":
                    string name = ReadString("Nhập tên (hoặc một phần tên)");
                    PrintList(await _service.SearchByNameAsync(name));
                    break;
                case "3":
                    string addr = ReadString("Nhập địa chỉ");
                    PrintList(await _service.SearchByAddressAsync(addr));
                    break;
                case "4":
                    string rank = ReadString("Nhập xếp loại (A/B/C/D/F)");
                    PrintList(await _service.SearchByGradeRankAsync(rank));
                    break;
                default:
                    PrintError("Lựa chọn không hợp lệ.");
                    break;
            }
        }

        // ──────────────────── INPUT HELPERS ────────────────────
        private string ReadString(string label)
        {
            Console.Write($"  {label}: ");
            return Console.ReadLine() ?? string.Empty;
        }

        private string ReadStringOptional(string label, string defaultVal)
        {
            Console.Write($"  {label} [{defaultVal}]: ");
            var input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? defaultVal : input;
        }

        private int ReadInt(string label)
        {
            while (true)
            {
                Console.Write($"  {label}: ");
                if (int.TryParse(Console.ReadLine(), out int val)) return val;
                PrintError("Vui lòng nhập số nguyên hợp lệ.");
            }
        }

        private int ReadIntOptional(string label, int defaultVal)
        {
            Console.Write($"  {label} [{defaultVal}]: ");
            var input = Console.ReadLine();
            return (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int val))
                ? defaultVal : val;
        }

        private double ReadDouble(string label)
        {
            while (true)
            {
                Console.Write($"  {label}: ");
                if (double.TryParse(Console.ReadLine(), out double val)) return val;
                PrintError("Vui lòng nhập số thực hợp lệ.");
            }
        }

        private double ReadDoubleOptional(string label, double defaultVal)
        {
            Console.Write($"  {label} [{defaultVal}]: ");
            var input = Console.ReadLine();
            return (string.IsNullOrWhiteSpace(input) || !double.TryParse(input, out double val))
                ? defaultVal : val;
        }

        // ──────────────────── UI HELPERS ────────────────────
        private void ShowHeader(string title) =>
            Console.WriteLine($"\n=== {title} ===");

        private void PrintSeparator() =>
            Console.WriteLine("  " + new string('-', 100));

        private void PrintSuccess(string msg) =>
            Console.WriteLine($"\n  ✔ {msg}");

        private void PrintError(string msg) =>
            Console.WriteLine($"\n  ✘ {msg}");

        private void Pause()
        {
            Console.WriteLine("\nNhấn Enter để tiếp tục...");
            Console.ReadLine();
        }
    }
}
