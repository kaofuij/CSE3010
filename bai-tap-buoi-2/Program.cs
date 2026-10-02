using System;
using System.Collections.Generic;
using BaiTapBuoi2.Models;

namespace BaiTapBuoi2
{
    public class Program
    {
        static SubjectManager manager = new SubjectManager();
        static ClsSubject? currentSubject = null;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Tạo dữ liệu mẫu
            manager.SeedSampleData();

            bool running = true;
            while (running)
            {
                PrintMainMenu();
                string? choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ManageSubjects();
                        break;
                    case "2":
                        SelectSubjectAndManageStudents();
                        break;
                    case "3":
                        ShowStatistics();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("  Đã thoát chương trình. Tạm biệt!");
                        break;
                    default:
                        Console.WriteLine("  [!] Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                        break;
                }
            }
        }

        // ==================== Menu chính ====================

        static void PrintMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║     QUẢN LÝ SINH VIÊN - BÀI TẬP BUỔI 2    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Quản lý lớp môn học                     ║");
            Console.WriteLine("║  2. Quản lý sinh viên trong lớp             ║");
            Console.WriteLine("║  3. Thống kê                                ║");
            Console.WriteLine("║  0. Thoát                                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.Write("  Chọn chức năng: ");
        }

        // ==================== 1. Quản lý lớp môn học ====================

        static void ManageSubjects()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("  ┌─────────────────────────────────────┐");
                Console.WriteLine("  │      QUẢN LÝ LỚP MÔN HỌC          │");
                Console.WriteLine("  ├─────────────────────────────────────┤");
                Console.WriteLine("  │  1. Xem danh sách lớp môn học      │");
                Console.WriteLine("  │  2. Thêm lớp môn học mới           │");
                Console.WriteLine("  │  3. Xóa lớp môn học                │");
                Console.WriteLine("  │  4. Xem chi tiết lớp môn học       │");
                Console.WriteLine("  │  0. Quay lại                       │");
                Console.WriteLine("  └─────────────────────────────────────┘");
                Console.Write("  Chọn: ");

                string? choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        manager.PrintAllSubjects();
                        break;
                    case "2":
                        AddNewSubject();
                        break;
                    case "3":
                        RemoveSubject();
                        break;
                    case "4":
                        ViewSubjectDetail();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("  [!] Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        static void AddNewSubject()
        {
            Console.WriteLine("  --- Thêm lớp môn học mới ---");
            Console.Write("  Nhập mã lớp: ");
            string id = Console.ReadLine()?.Trim() ?? "";

            Console.Write("  Nhập tên môn học: ");
            string name = Console.ReadLine()?.Trim() ?? "";

            Console.Write("  Nhập học kỳ: ");
            string semester = Console.ReadLine()?.Trim() ?? "";

            Console.Write("  Nhập tên giảng viên: ");
            string teacher = Console.ReadLine()?.Trim() ?? "";

            try
            {
                var subject = new ClsSubject(id, name, semester, teacher);
                if (manager.AddSubject(subject))
                    Console.WriteLine($"  [✓] Đã thêm lớp môn học '{name}' thành công.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  [!] Lỗi: {ex.Message}");
            }
        }

        static void RemoveSubject()
        {
            var subject = SelectSubject("cần xóa");
            if (subject == null) return;

            if (manager.RemoveSubject(subject.Id))
                Console.WriteLine($"  [✓] Đã xóa lớp môn học mã '{subject.Id}'.");
        }

        static void ViewSubjectDetail()
        {
            var subject = SelectSubject("cần xem");
            if (subject == null) return;

            Console.WriteLine();
            Console.WriteLine("  === THÔNG TIN LỚP MÔN HỌC ===");
            subject.PrintSubjectInfo();
            Console.WriteLine();
            Console.WriteLine("  === DANH SÁCH SINH VIÊN ===");
            subject.PrintStudentTable();
        }

        // ==================== 2. Quản lý sinh viên ====================

        static void SelectSubjectAndManageStudents()
        {
            currentSubject = SelectSubject("cần quản lý");
            if (currentSubject == null) return;

            Console.WriteLine($"  [✓] Đã chọn lớp: {currentSubject}");
            ManageStudents();
        }

        static void ManageStudents()
        {
            if (currentSubject == null) return;

            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine($"  ┌─────────────────────────────────────────────┐");
                Console.WriteLine($"  │  QUẢN LÝ SV - [{currentSubject.Id}] {currentSubject.Name,-15}│");
                Console.WriteLine($"  ├─────────────────────────────────────────────┤");
                Console.WriteLine($"  │  1. Xem danh sách sinh viên                │");
                Console.WriteLine($"  │  2. Thêm sinh viên                         │");
                Console.WriteLine($"  │  3. Xóa sinh viên                          │");
                Console.WriteLine($"  │  4. Cập nhật điểm sinh viên                │");
                Console.WriteLine($"  │  5. Tìm kiếm sinh viên theo mã             │");
                Console.WriteLine($"  │  6. Tìm kiếm sinh viên theo tên            │");
                Console.WriteLine($"  │  7. Sắp xếp theo điểm TB (giảm dần)       │");
                Console.WriteLine($"  │  8. Sắp xếp theo điểm TB (tăng dần)       │");
                Console.WriteLine($"  │  0. Quay lại                               │");
                Console.WriteLine($"  └─────────────────────────────────────────────┘");
                Console.Write("  Chọn: ");

                string? choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        currentSubject.PrintStudentTable();
                        break;
                    case "2":
                        AddStudentToSubject();
                        break;
                    case "3":
                        RemoveStudentFromSubject();
                        break;
                    case "4":
                        UpdateStudentPoints();
                        break;
                    case "5":
                        SearchStudentById();
                        break;
                    case "6":
                        SearchStudentByName();
                        break;
                    case "7":
                        var descList = currentSubject.GetStudentsSortedByAvg(descending: true);
                        Console.WriteLine("  === DANH SÁCH SẮP XẾP THEO ĐIỂM TB (GIẢM DẦN) ===");
                        currentSubject.PrintStudentTable(descList);
                        break;
                    case "8":
                        var ascList = currentSubject.GetStudentsSortedByAvg(descending: false);
                        Console.WriteLine("  === DANH SÁCH SẮP XẾP THEO ĐIỂM TB (TĂNG DẦN) ===");
                        currentSubject.PrintStudentTable(ascList);
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("  [!] Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        static void AddStudentToSubject()
        {
            if (currentSubject == null) return;

            Console.WriteLine("  --- Thêm sinh viên ---");
            Console.Write("  Mã sinh viên: ");
            string stdId = Console.ReadLine()?.Trim() ?? "";

            Console.Write("  Họ và tên: ");
            string name = Console.ReadLine()?.Trim() ?? "";

            double midPoint = ReadDouble("  Điểm giữa kỳ (0-10): ");
            double finalPoint = ReadDouble("  Điểm cuối kỳ (0-10): ");

            try
            {
                var student = new Student(stdId, name, midPoint, finalPoint);
                if (currentSubject.AddStudent(student))
                    Console.WriteLine($"  [✓] Đã thêm sinh viên '{name}' vào lớp.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  [!] Lỗi: {ex.Message}");
            }
        }

        static void RemoveStudentFromSubject()
        {
            if (currentSubject == null) return;

            currentSubject.PrintStudentTable();
            Console.Write("  Nhập mã SV cần xóa: ");
            string stdId = Console.ReadLine()?.Trim() ?? "";

            if (currentSubject.RemoveStudent(stdId))
                Console.WriteLine($"  [✓] Đã xóa sinh viên mã '{stdId}' khỏi lớp.");
        }

        static void UpdateStudentPoints()
        {
            if (currentSubject == null) return;

            currentSubject.PrintStudentTable();
            Console.Write("  Nhập mã SV cần cập nhật điểm: ");
            string stdId = Console.ReadLine()?.Trim() ?? "";

            var student = currentSubject.FindStudentById(stdId);
            if (student == null)
            {
                Console.WriteLine($"  [!] Không tìm thấy sinh viên mã '{stdId}'.");
                return;
            }

            Console.WriteLine($"  Sinh viên: {student.Name}");
            Console.WriteLine($"  Điểm hiện tại - Giữa kỳ: {student.MidPoint:F1}, Cuối kỳ: {student.FinalPoint:F1}");

            double midPoint = ReadDouble("  Điểm giữa kỳ mới (0-10): ");
            double finalPoint = ReadDouble("  Điểm cuối kỳ mới (0-10): ");

            try
            {
                if (currentSubject.UpdateStudentPoints(stdId, midPoint, finalPoint))
                    Console.WriteLine($"  [✓] Đã cập nhật điểm cho sinh viên '{student.Name}'.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  [!] Lỗi: {ex.Message}");
            }
        }

        static void SearchStudentById()
        {
            if (currentSubject == null) return;

            Console.Write("  Nhập mã SV cần tìm: ");
            string stdId = Console.ReadLine()?.Trim() ?? "";

            var student = currentSubject.FindStudentById(stdId);
            if (student == null)
            {
                Console.WriteLine($"  [!] Không tìm thấy sinh viên mã '{stdId}'.");
                return;
            }

            Console.WriteLine("  === KẾT QUẢ TÌM KIẾM ===");
            currentSubject.PrintStudentTable(new List<Student> { student });
        }

        static void SearchStudentByName()
        {
            if (currentSubject == null) return;

            Console.Write("  Nhập từ khóa tên cần tìm: ");
            string keyword = Console.ReadLine()?.Trim() ?? "";

            var results = currentSubject.FindStudentsByName(keyword);
            if (results.Count == 0)
            {
                Console.WriteLine($"  [!] Không tìm thấy sinh viên có tên chứa '{keyword}'.");
                return;
            }

            Console.WriteLine($"  === KẾT QUẢ TÌM KIẾM ('{keyword}') ===");
            currentSubject.PrintStudentTable(results);
        }

        // ==================== 3. Thống kê ====================

        static void ShowStatistics()
        {
            var subject = SelectSubject("cần xem thống kê");
            if (subject == null) return;

            if (subject.Students.Count == 0)
            {
                Console.WriteLine("  [!] Lớp chưa có sinh viên nào.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("  ╔═══════════════════════════════════════════════╗");
            Console.WriteLine($"  ║  THỐNG KÊ: [{subject.Id}] {subject.Name,-20}  ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");
            Console.WriteLine($"  ║  Tổng số sinh viên     : {subject.Students.Count,-20} ║");
            Console.WriteLine($"  ║  Điểm TB cả lớp        : {subject.GetClassAverage(),-20:F2}║");
            Console.WriteLine($"  ║  Số SV đạt (TB >= 5.0) : {subject.CountPassedStudents(),-20} ║");
            Console.WriteLine($"  ║  Số SV không đạt       : {subject.CountFailedStudents(),-20} ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");

            var top = subject.GetTopStudent();
            var lowest = subject.GetLowestStudent();

            if (top != null)
                Console.WriteLine($"  ║  SV điểm cao nhất: {top.Name,-15} ({top.AvgPoint:F2})  ║");
            if (lowest != null)
                Console.WriteLine($"  ║  SV điểm thấp nhất: {lowest.Name,-14} ({lowest.AvgPoint:F2})  ║");

            Console.WriteLine("  ╚═══════════════════════════════════════════════╝");

            Console.WriteLine();
            Console.WriteLine("  === BẢNG ĐIỂM (SẮP XẾP THEO TB GIẢM DẦN) ===");
            subject.PrintStudentTable(subject.GetStudentsSortedByAvg());
        }

        // ==================== Tiện ích ====================

        /// <summary>
        /// Cho phép chọn lớp môn học bằng số thứ tự (1, 2...) hoặc mã lớp (CSE3010.01)
        /// </summary>
        static ClsSubject? SelectSubject(string action)
        {
            var allSubjects = manager.GetAllSubjects();
            if (allSubjects.Count == 0)
            {
                Console.WriteLine("  (Chưa có lớp môn học nào)");
                return null;
            }

            manager.PrintAllSubjects();
            Console.Write($"  Chọn lớp {action} (nhập STT hoặc mã lớp): ");
            string input = Console.ReadLine()?.Trim() ?? "";

            // Thử tìm theo số thứ tự
            if (int.TryParse(input, out int index) && index >= 1 && index <= allSubjects.Count)
            {
                return allSubjects[index - 1];
            }

            // Thử tìm theo mã lớp
            var subject = manager.FindSubjectById(input);
            if (subject != null)
            {
                return subject;
            }

            Console.WriteLine($"  [!] Không tìm thấy lớp '{input}'. Hãy nhập STT (1, 2...) hoặc mã lớp.");
            return null;
        }

        static double ReadDouble(string prompt)
        {
            double value;
            Console.Write(prompt);
            while (!double.TryParse(Console.ReadLine(), out value) || value < 0 || value > 10)
            {
                Console.Write("  [!] Giá trị không hợp lệ (0-10). Nhập lại: ");
            }
            return value;
        }
    }
}
