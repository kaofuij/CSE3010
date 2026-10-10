using System;
using System.Collections.Generic;
using BaiTapBuoi3.Models;

namespace BaiTapBuoi3.DataAccess
{
    /// <summary>
    /// Khởi tạo dữ liệu mẫu cho DataList
    /// </summary>
    public static class DataSeeder
    {
        public static List<ClsSubject> SeedSubjects()
        {
            var subjects = new List<ClsSubject>();

            // Lớp môn C# nâng cao
            var csharp = new ClsSubject("CSE3010.01", "Lập trình C# nâng cao", "HK1 2026-2027", "Thầy Nguyễn Văn A");
            csharp.Students.Add(new Student("SV001", "Trần Văn Hùng", 8.5, 7.0));
            csharp.Students.Add(new Student("SV002", "Nguyễn Thị Lan", 9.0, 9.5));
            csharp.Students.Add(new Student("SV003", "Lê Minh Tuấn", 6.5, 5.0));
            csharp.Students.Add(new Student("SV004", "Phạm Thị Hoa", 7.0, 8.0));
            csharp.Students.Add(new Student("SV005", "Hoàng Văn Nam", 4.0, 3.5));
            csharp.Students.Add(new Student("SV006", "Đỗ Thị Mai", 8.0, 8.5));
            csharp.Students.Add(new Student("SV007", "Vũ Đức Anh", 5.5, 6.0));
            csharp.Students.Add(new Student("SV008", "Bùi Thị Ngọc", 9.5, 10.0));
            subjects.Add(csharp);

            // Lớp môn Cấu trúc dữ liệu
            var dsa = new ClsSubject("CSE2010.02", "Cấu trúc dữ liệu và giải thuật", "HK1 2026-2027", "Cô Trần Thị B");
            dsa.Students.Add(new Student("SV001", "Trần Văn Hùng", 7.0, 6.5));
            dsa.Students.Add(new Student("SV003", "Lê Minh Tuấn", 8.0, 7.5));
            dsa.Students.Add(new Student("SV009", "Nguyễn Hoàng Long", 6.0, 5.5));
            dsa.Students.Add(new Student("SV010", "Trịnh Thị Thảo", 9.0, 8.0));
            subjects.Add(dsa);

            return subjects;
        }
    }
}
