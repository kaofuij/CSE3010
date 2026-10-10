using System;
using System.Collections.Generic;
using System.Linq;
using BaiTapBuoi3.Models;
using BaiTapBuoi3.DataAccess;
using BaiTapBuoi3.Business;

namespace BaiTapBuoi3.Presentation
{
    /// <summary>
    /// Tầng Presentation (Console) - chỉ xử lý giao diện
    /// Mô phỏng hành vi của WinForms: ComboBox chọn lớp, hiển thị SV, tìm kiếm, validate
    /// </summary>
    public class ConsoleUI
    {
        private readonly ClsSubjectBLL subjectBLL;
        private StudentBLL? studentBLL;
        private ClsSubject? selectedSubject;

        public ConsoleUI(ClsSubjectBLL subjectBLL)
        {
            this.subjectBLL = subjectBLL;
        }

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool running = true;
            while (running)
            {
                PrintMainMenu();
                string? choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": SelectSubjectFromComboBox(); break;
                    case "2": DisplayStudentList(); break;
                    case "3": SearchStudent(); break;
                    case "4": LookupStudentById(); break;
                    case "5": AddStudent(); break;
                    case "6": UpdateStudent(); break;
                    case "7": DeleteStudent(); break;
                    case "8": ShowStatistics(); break;
                    case "0":
                        running = false;
                        Console.WriteLine("  Đã thoát. Tạm biệt!");
                        break;
                    default:
                        Console.WriteLine("  [!] Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        void PrintMainMenu()
        {
            Console.WriteLine();
            string subjectInfo = selectedSubject != null
                ? $" [{selectedSubject.Id}] {selectedSubject.Name}"
                : " (Chưa chọn lớp)";

            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║     QUẢN LÝ SINH VIÊN - BUỔI 3 (3-LAYER)           ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════╣");
            Console.WriteLine($"  Lớp hiện tại:{subjectInfo}");
            Console.WriteLine("╠══════════════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Chọn lớp môn học (ComboBox)                     ║");
            Console.WriteLine("║  2. Xem danh sách sinh viên                         ║");
            Console.WriteLine("║  3. Tìm kiếm sinh viên                              ║");
            Console.WriteLine("║  4. Tra cứu SV theo mã (auto-fill)                  ║");
            Console.WriteLine("║  5. Thêm sinh viên                                  ║");
            Console.WriteLine("║  6. Sửa sinh viên                                   ║");
            Console.WriteLine("║  7. Xóa sinh viên                                   ║");
            Console.WriteLine("║  8. Thống kê                                        ║");
            Console.WriteLine("║  0. Thoát                                           ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.Write("  Chọn: ");
        }

        // ==================== 1. Chọn lớp (mô phỏng ComboBox) ====================

        void SelectSubjectFromComboBox()
        {
            var subjects = subjectBLL.GetAll();
            if (subjects.Count == 0)
            {
                Console.WriteLine("  (Chưa có lớp môn học nào)");
                return;
            }

            Console.WriteLine("  ┌─── ComboBox: Chọn lớp môn học ───┐");
            for (int i = 0; i < subjects.Count; i++)
            {
                Console.WriteLine($"  │  {i + 1}. {subjects[i]}");
            }
            Console.WriteLine("  └──────────────────────────────────┘");
            Console.Write("  Chọn (nhập STT): ");

            if (int.TryParse(Console.ReadLine()?.Trim(), out int idx) && idx >= 1 && idx <= subjects.Count)
            {
                selectedSubject = subjects[idx - 1];

                // Khi chọn lớp → tạo StudentBLL cho lớp đó (qua DAL)
                var studentDAL = subjectBLL.GetStudentDALForSubject(selectedSubject.Id);
                studentBLL = new StudentBLL(studentDAL);

                Console.WriteLine($"  [✓] Đã chọn lớp: {selectedSubject}");
                Console.WriteLine();

                // Hiển thị sinh viên của lớp đấy (giống yêu cầu: khi chọn lớp → hiển thị SV)
                DisplayStudentList();
            }
            else
            {
                Console.WriteLine("  [!] Lựa chọn không hợp lệ.");
            }
        }

        // ==================== 2. Hiển thị danh sách SV ====================

        void DisplayStudentList()
        {
            if (!EnsureSubjectSelected()) return;

            var students = studentBLL!.GetAll();
            PrintStudentTable(students);
        }

        // ==================== 3. Tìm kiếm ====================

        void SearchStudent()
        {
            if (!EnsureSubjectSelected()) return;

            Console.Write("  Tìm theo: (1) Mã SV  (2) Tên SV: ");
            string? mode = Console.ReadLine()?.Trim();

            List<Student> results;
            if (mode == "1")
            {
                Console.Write("  Nhập mã SV: ");
                string stdId = Console.ReadLine()?.Trim() ?? "";
                var student = studentBLL!.GetById(stdId);
                results = student != null ? new List<Student> { student } : new List<Student>();
            }
            else
            {
                Console.Write("  Nhập từ khóa tên: ");
                string keyword = Console.ReadLine()?.Trim() ?? "";
                results = studentBLL!.SearchByName(keyword);
            }

            if (results.Count == 0)
                Console.WriteLine("  [!] Không tìm thấy kết quả.");
            else
            {
                Console.WriteLine($"  === Tìm thấy {results.Count} sinh viên ===");
                PrintStudentTable(results);
            }
        }

        // ==================== 4. Tra cứu theo mã (auto-fill) ====================

        /// <summary>
        /// Mô phỏng: nhập mã SV vào txtMa → nếu tồn tại → hiển thị thông tin → nếu không → xóa trống
        /// </summary>
        void LookupStudentById()
        {
            if (!EnsureSubjectSelected()) return;

            Console.Write("  [txtMa] Nhập mã sinh viên: ");
            string stdId = Console.ReadLine()?.Trim() ?? "";

            var student = studentBLL!.GetById(stdId);
            if (student != null)
            {
                Console.WriteLine("  ── Auto-fill thông tin ──");
                Console.WriteLine($"  [txtMa]       : {student.StdId}");
                Console.WriteLine($"  [txtTen]      : {student.Name}");
                Console.WriteLine($"  [txtGiuaKy]   : {student.MidPoint:F1}");
                Console.WriteLine($"  [txtCuoiKy]   : {student.FinalPoint:F1}");
                Console.WriteLine($"  [lblTB]       : {student.AvgPoint:F2}");
                Console.WriteLine($"  [lblXepLoai]  : {student.GetGrade()}");
            }
            else
            {
                Console.WriteLine("  ── Mã không tồn tại → Xóa trống các trường ──");
                Console.WriteLine("  [txtMa]       : (trống)");
                Console.WriteLine("  [txtTen]      : (trống)");
                Console.WriteLine("  [txtGiuaKy]   : (trống)");
                Console.WriteLine("  [txtCuoiKy]   : (trống)");
                Console.WriteLine("  [lblTB]       : (trống)");
                Console.WriteLine("  [lblXepLoai]  : (trống)");
            }
        }

        // ==================== 5. Thêm SV (có validate + ErrorProvider) ====================

        void AddStudent()
        {
            if (!EnsureSubjectSelected()) return;

            Console.WriteLine("  --- Thêm sinh viên mới ---");
            var student = InputStudentInfo();
            if (student == null) return;

            var (success, errors) = studentBLL!.Insert(student);
            if (success)
            {
                Console.WriteLine($"  [✓] Đã thêm sinh viên '{student.Name}' thành công.");
            }
            else
            {
                PrintValidationErrors(errors);
            }
        }

        // ==================== 6. Sửa SV (có validate + ErrorProvider) ====================

        void UpdateStudent()
        {
            if (!EnsureSubjectSelected()) return;

            Console.Write("  Nhập mã SV cần sửa: ");
            string stdId = Console.ReadLine()?.Trim() ?? "";

            var existing = studentBLL!.GetById(stdId);
            if (existing == null)
            {
                Console.WriteLine($"  [!] Không tìm thấy sinh viên mã '{stdId}'.");
                return;
            }

            Console.WriteLine($"  Đang sửa: {existing.Name} (giữa kỳ: {existing.MidPoint:F1}, cuối kỳ: {existing.FinalPoint:F1})");
            Console.WriteLine("  (Nhấn Enter để giữ nguyên giá trị cũ)");

            Console.Write($"  Tên [{existing.Name}]: ");
            string name = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(name)) name = existing.Name;

            Console.Write($"  Điểm giữa kỳ [{existing.MidPoint:F1}]: ");
            string midStr = Console.ReadLine()?.Trim() ?? "";
            double midPoint = string.IsNullOrEmpty(midStr) ? existing.MidPoint : double.Parse(midStr);

            Console.Write($"  Điểm cuối kỳ [{existing.FinalPoint:F1}]: ");
            string finalStr = Console.ReadLine()?.Trim() ?? "";
            double finalPoint = string.IsNullOrEmpty(finalStr) ? existing.FinalPoint : double.Parse(finalStr);

            var updated = new Student(stdId, name, midPoint, finalPoint);
            var (success, errors) = studentBLL.Update(updated);

            if (success)
                Console.WriteLine($"  [✓] Đã cập nhật sinh viên '{name}'.");
            else
                PrintValidationErrors(errors);
        }

        // ==================== 7. Xóa SV ====================

        void DeleteStudent()
        {
            if (!EnsureSubjectSelected()) return;

            DisplayStudentList();
            Console.Write("  Nhập mã SV cần xóa: ");
            string stdId = Console.ReadLine()?.Trim() ?? "";

            if (studentBLL!.Delete(stdId))
                Console.WriteLine($"  [✓] Đã xóa sinh viên mã '{stdId}'.");
            else
                Console.WriteLine($"  [!] Không tìm thấy sinh viên mã '{stdId}'.");
        }

        // ==================== 8. Thống kê ====================

        void ShowStatistics()
        {
            if (!EnsureSubjectSelected()) return;

            var (total, classAvg, passed, failed, top, lowest) = studentBLL!.GetStatistics();

            if (total == 0)
            {
                Console.WriteLine("  [!] Lớp chưa có sinh viên nào.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"  ╔═══════════════════════════════════════════╗");
            Console.WriteLine($"  ║  THỐNG KÊ: {selectedSubject}");
            Console.WriteLine($"  ╠═══════════════════════════════════════════╣");
            Console.WriteLine($"  ║  Tổng số SV       : {total}");
            Console.WriteLine($"  ║  Điểm TB cả lớp   : {classAvg:F2}");
            Console.WriteLine($"  ║  Số SV đạt (≥5.0) : {passed}");
            Console.WriteLine($"  ║  Số SV không đạt  : {failed}");
            if (top != null)
                Console.WriteLine($"  ║  SV cao nhất      : {top.Name} ({top.AvgPoint:F2})");
            if (lowest != null)
                Console.WriteLine($"  ║  SV thấp nhất     : {lowest.Name} ({lowest.AvgPoint:F2})");
            Console.WriteLine($"  ╚═══════════════════════════════════════════╝");

            Console.WriteLine();
            var sorted = studentBLL.GetSortedByAvg();
            PrintStudentTable(sorted);
        }

        // ==================== Tiện ích ====================

        bool EnsureSubjectSelected()
        {
            if (selectedSubject == null || studentBLL == null)
            {
                Console.WriteLine("  [!] Chưa chọn lớp môn học. Hãy chọn lớp trước (mục 1).");
                return false;
            }
            return true;
        }

        Student? InputStudentInfo()
        {
            try
            {
                Console.Write("  Mã sinh viên: ");
                string stdId = Console.ReadLine()?.Trim() ?? "";

                Console.Write("  Họ và tên: ");
                string name = Console.ReadLine()?.Trim() ?? "";

                Console.Write("  Điểm giữa kỳ (0-10): ");
                if (!double.TryParse(Console.ReadLine(), out double midPoint))
                {
                    Console.WriteLine("  [ErrorProvider: txtGiuaKy] Điểm giữa kỳ không hợp lệ.");
                    return null;
                }

                Console.Write("  Điểm cuối kỳ (0-10): ");
                if (!double.TryParse(Console.ReadLine(), out double finalPoint))
                {
                    Console.WriteLine("  [ErrorProvider: txtCuoiKy] Điểm cuối kỳ không hợp lệ.");
                    return null;
                }

                return new Student(stdId, name, midPoint, finalPoint);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  [ErrorProvider] {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Mô phỏng ErrorProvider: hiển thị lỗi theo từng trường
        /// </summary>
        void PrintValidationErrors(Dictionary<string, string> errors)
        {
            Console.WriteLine("  ── ErrorProvider ──");
            foreach (var error in errors)
            {
                string controlName = error.Key switch
                {
                    "StdId" => "txtMa",
                    "Name" => "txtTen",
                    "MidPoint" => "txtGiuaKy",
                    "FinalPoint" => "txtCuoiKy",
                    _ => error.Key
                };
                Console.WriteLine($"  [⚠ {controlName}] {error.Value}");
            }
        }

        void PrintStudentTable(List<Student> students)
        {
            if (students.Count == 0)
            {
                Console.WriteLine("  (Danh sách trống)");
                return;
            }

            string sep = "+" + new string('-', 14) + "+" + new string('-', 27) + "+"
                       + new string('-', 10) + "+" + new string('-', 12) + "+"
                       + new string('-', 8) + "+" + new string('-', 14) + "+";

            Console.WriteLine(sep);
            Console.WriteLine($"| {"Mã SV",-12} | {"Họ và tên",-25} | {"Giữa kỳ",8} | {"Cuối kỳ",10} | {"TB",6} | {"Xếp loại",-12} |");
            Console.WriteLine(sep);
            foreach (var s in students)
                Console.WriteLine(s.ToString());
            Console.WriteLine(sep);
            Console.WriteLine($"  Tổng: {students.Count} sinh viên");
        }
    }
}
