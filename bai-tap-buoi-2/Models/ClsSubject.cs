using System;
using System.Collections.Generic;
using System.Linq;

namespace BaiTapBuoi2.Models
{
    /// <summary>
    /// Lớp môn học (ClassSubject) - chứa danh sách sinh viên
    /// </summary>
    public class ClsSubject
    {
        // Mã lớp môn học
        private string id = string.Empty;
        public string Id
        {
            get => id;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã lớp môn học không được để trống.");
                id = value;
            }
        }

        // Tên môn học
        private string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên môn học không được để trống.");
                name = value;
            }
        }

        // Học kỳ
        private string semester = string.Empty;
        public string Semester
        {
            get => semester;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Học kỳ không được để trống.");
                semester = value;
            }
        }

        // Giảng viên
        private string teacher = string.Empty;
        public string Teacher
        {
            get => teacher;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên giảng viên không được để trống.");
                teacher = value;
            }
        }

        // Danh sách sinh viên trong lớp môn học
        public List<Student> Students { get; set; }

        // Constructor
        public ClsSubject(string id, string name, string semester, string teacher)
        {
            Id = id;
            Name = name;
            Semester = semester;
            Teacher = teacher;
            Students = new List<Student>();
        }

        // ==================== CRUD sinh viên ====================

        /// <summary>
        /// Thêm sinh viên vào lớp môn học
        /// </summary>
        public bool AddStudent(Student student)
        {
            if (Students.Any(s => s.StdId == student.StdId))
            {
                Console.WriteLine($"  [!] Sinh viên mã '{student.StdId}' đã tồn tại trong lớp.");
                return false;
            }
            Students.Add(student);
            return true;
        }

        /// <summary>
        /// Xóa sinh viên khỏi lớp theo mã SV
        /// </summary>
        public bool RemoveStudent(string stdId)
        {
            var student = Students.FirstOrDefault(s => s.StdId == stdId);
            if (student == null)
            {
                Console.WriteLine($"  [!] Không tìm thấy sinh viên mã '{stdId}'.");
                return false;
            }
            Students.Remove(student);
            return true;
        }

        /// <summary>
        /// Tìm sinh viên theo mã SV
        /// </summary>
        public Student? FindStudentById(string stdId)
        {
            return Students.FirstOrDefault(s => s.StdId == stdId);
        }

        /// <summary>
        /// Tìm sinh viên theo tên (chứa từ khoá)
        /// </summary>
        public List<Student> FindStudentsByName(string keyword)
        {
            return Students
                .Where(s => s.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Cập nhật điểm sinh viên
        /// </summary>
        public bool UpdateStudentPoints(string stdId, double midPoint, double finalPoint)
        {
            var student = FindStudentById(stdId);
            if (student == null)
            {
                Console.WriteLine($"  [!] Không tìm thấy sinh viên mã '{stdId}'.");
                return false;
            }
            student.MidPoint = midPoint;
            student.FinalPoint = finalPoint;
            return true;
        }

        // ==================== Thống kê ====================

        /// <summary>
        /// Sắp xếp danh sách sinh viên theo điểm trung bình giảm dần
        /// </summary>
        public List<Student> GetStudentsSortedByAvg(bool descending = true)
        {
            return descending
                ? Students.OrderByDescending(s => s.AvgPoint).ToList()
                : Students.OrderBy(s => s.AvgPoint).ToList();
        }

        /// <summary>
        /// Sinh viên có điểm trung bình cao nhất
        /// </summary>
        public Student? GetTopStudent()
        {
            return Students.MaxBy(s => s.AvgPoint);
        }

        /// <summary>
        /// Sinh viên có điểm trung bình thấp nhất
        /// </summary>
        public Student? GetLowestStudent()
        {
            return Students.MinBy(s => s.AvgPoint);
        }

        /// <summary>
        /// Điểm trung bình của cả lớp
        /// </summary>
        public double GetClassAverage()
        {
            if (Students.Count == 0) return 0;
            return Math.Round(Students.Average(s => s.AvgPoint), 2);
        }

        /// <summary>
        /// Đếm số sinh viên đạt (điểm TB >= 5.0)
        /// </summary>
        public int CountPassedStudents()
        {
            return Students.Count(s => s.AvgPoint >= 5.0);
        }

        /// <summary>
        /// Đếm số sinh viên không đạt (điểm TB < 5.0)
        /// </summary>
        public int CountFailedStudents()
        {
            return Students.Count(s => s.AvgPoint < 5.0);
        }

        // ==================== Hiển thị ====================

        /// <summary>
        /// In thông tin lớp môn học
        /// </summary>
        public void PrintSubjectInfo()
        {
            Console.WriteLine($"  Mã lớp    : {Id}");
            Console.WriteLine($"  Môn học   : {Name}");
            Console.WriteLine($"  Học kỳ    : {Semester}");
            Console.WriteLine($"  Giảng viên: {Teacher}");
            Console.WriteLine($"  Sĩ số     : {Students.Count} sinh viên");
        }

        /// <summary>
        /// In bảng danh sách sinh viên
        /// </summary>
        public void PrintStudentTable(List<Student>? list = null)
        {
            var data = list ?? Students;
            if (data.Count == 0)
            {
                Console.WriteLine("  (Danh sách trống)");
                return;
            }

            string separator = "+" + new string('-', 14) + "+" + new string('-', 27) + "+"
                             + new string('-', 10) + "+" + new string('-', 12) + "+"
                             + new string('-', 8) + "+" + new string('-', 14) + "+";

            Console.WriteLine(separator);
            Console.WriteLine($"| {"Mã SV",-12} | {"Họ và tên",-25} | {"Giữa kỳ",8} | {"Cuối kỳ",10} | {"TB",6} | {"Xếp loại",-12} |");
            Console.WriteLine(separator);
            foreach (var s in data)
            {
                Console.WriteLine(s.ToString());
            }
            Console.WriteLine(separator);
            Console.WriteLine($"  Tổng số: {data.Count} sinh viên");
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - HK: {Semester} - GV: {Teacher} ({Students.Count} SV)";
        }
    }
}
