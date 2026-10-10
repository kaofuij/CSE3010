using System;
using System.Collections.Generic;

namespace BaiTapBuoi3.Models
{
    /// <summary>
    /// Lớp môn học (ClassSubject) - chứa danh sách sinh viên
    /// </summary>
    public class ClsSubject
    {
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

        public List<Student> Students { get; set; }

        public ClsSubject(string id, string name, string semester, string teacher)
        {
            Id = id;
            Name = name;
            Semester = semester;
            Teacher = teacher;
            Students = new List<Student>();
        }

        /// <summary>
        /// Hiển thị tên lớp cho ComboBox / danh sách
        /// </summary>
        public override string ToString()
        {
            return $"[{Id}] {Name} - {Semester}";
        }
    }
}
