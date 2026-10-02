using System;
using System.Collections.Generic;
using System.Linq;
using BaiTapBuoi2.Models;

namespace BaiTapBuoi2
{
    /// <summary>
    /// Quản lý danh sách các lớp môn học
    /// </summary>
    public class SubjectManager
    {
        private List<ClsSubject> subjects;

        public SubjectManager()
        {
            subjects = new List<ClsSubject>();
        }

        // ==================== Quản lý lớp môn học ====================

        public bool AddSubject(ClsSubject subject)
        {
            if (subjects.Any(s => s.Id == subject.Id))
            {
                Console.WriteLine($"  [!] Lớp môn học mã '{subject.Id}' đã tồn tại.");
                return false;
            }
            subjects.Add(subject);
            return true;
        }

        public bool RemoveSubject(string subjectId)
        {
            var subject = FindSubjectById(subjectId);
            if (subject == null)
            {
                Console.WriteLine($"  [!] Không tìm thấy lớp môn học mã '{subjectId}'.");
                return false;
            }
            subjects.Remove(subject);
            return true;
        }

        public ClsSubject? FindSubjectById(string id)
        {
            return subjects.FirstOrDefault(s => s.Id == id);
        }

        public List<ClsSubject> GetAllSubjects()
        {
            return subjects;
        }

        public void PrintAllSubjects()
        {
            if (subjects.Count == 0)
            {
                Console.WriteLine("  (Chưa có lớp môn học nào)");
                return;
            }

            Console.WriteLine($"  Danh sách {subjects.Count} lớp môn học:");
            Console.WriteLine("  " + new string('-', 70));
            for (int i = 0; i < subjects.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {subjects[i]}");
            }
            Console.WriteLine("  " + new string('-', 70));
        }

        // ==================== Dữ liệu mẫu ====================

        /// <summary>
        /// Tạo dữ liệu mẫu để demo
        /// </summary>
        public void SeedSampleData()
        {
            // Lớp môn C# nâng cao
            var csharp = new ClsSubject("CSE3010.01", "Lập trình C# nâng cao", "HK1 2026-2027", "Thầy Nguyễn Văn A");
            csharp.AddStudent(new Student("SV001", "Trần Văn Hùng", 8.5, 7.0));
            csharp.AddStudent(new Student("SV002", "Nguyễn Thị Lan", 9.0, 9.5));
            csharp.AddStudent(new Student("SV003", "Lê Minh Tuấn", 6.5, 5.0));
            csharp.AddStudent(new Student("SV004", "Phạm Thị Hoa", 7.0, 8.0));
            csharp.AddStudent(new Student("SV005", "Hoàng Văn Nam", 4.0, 3.5));
            csharp.AddStudent(new Student("SV006", "Đỗ Thị Mai", 8.0, 8.5));
            csharp.AddStudent(new Student("SV007", "Vũ Đức Anh", 5.5, 6.0));
            csharp.AddStudent(new Student("SV008", "Bùi Thị Ngọc", 9.5, 10.0));

            // Lớp môn Cấu trúc dữ liệu
            var dsa = new ClsSubject("CSE2010.02", "Cấu trúc dữ liệu và giải thuật", "HK1 2026-2027", "Cô Trần Thị B");
            dsa.AddStudent(new Student("SV001", "Trần Văn Hùng", 7.0, 6.5));
            dsa.AddStudent(new Student("SV003", "Lê Minh Tuấn", 8.0, 7.5));
            dsa.AddStudent(new Student("SV009", "Nguyễn Hoàng Long", 6.0, 5.5));
            dsa.AddStudent(new Student("SV010", "Trịnh Thị Thảo", 9.0, 8.0));

            AddSubject(csharp);
            AddSubject(dsa);
        }
    }
}
