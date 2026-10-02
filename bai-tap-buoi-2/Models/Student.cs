using System;

namespace BaiTapBuoi2.Models
{
    /// <summary>
    /// Lớp thực thể Sinh viên
    /// </summary>
    public class Student
    {
        // Mã sinh viên
        private string stdId = string.Empty;
        public string StdId
        {
            get => stdId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã sinh viên không được để trống.");
                stdId = value;
            }
        }

        // Tên sinh viên
        private string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên sinh viên không được để trống.");
                name = value;
            }
        }

        // Điểm giữa kỳ
        private double midPoint;
        public double MidPoint
        {
            get => midPoint;
            set
            {
                if (value < 0 || value > 10)
                    throw new ArgumentException("Điểm giữa kỳ phải nằm trong khoảng 0 - 10.");
                midPoint = value;
            }
        }

        // Điểm cuối kỳ
        private double finalPoint;
        public double FinalPoint
        {
            get => finalPoint;
            set
            {
                if (value < 0 || value > 10)
                    throw new ArgumentException("Điểm cuối kỳ phải nằm trong khoảng 0 - 10.");
                finalPoint = value;
            }
        }

        // Điểm trung bình (40% giữa kỳ + 60% cuối kỳ)
        public double AvgPoint => Math.Round(MidPoint * 0.4 + FinalPoint * 0.6, 2);

        // Constructor
        public Student(string stdId, string name, double midPoint, double finalPoint)
        {
            StdId = stdId;
            Name = name;
            MidPoint = midPoint;
            FinalPoint = finalPoint;
        }

        // Xếp loại theo điểm trung bình
        public string GetGrade()
        {
            return AvgPoint switch
            {
                >= 9.0 => "Xuất sắc",
                >= 8.0 => "Giỏi",
                >= 6.5 => "Khá",
                >= 5.0 => "Trung bình",
                >= 3.5 => "Yếu",
                _ => "Kém"
            };
        }

        public override string ToString()
        {
            return $"| {StdId,-12} | {Name,-25} | {MidPoint,8:F1} | {FinalPoint,10:F1} | {AvgPoint,6:F2} | {GetGrade(),-12} |";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Student other)
                return StdId == other.StdId;
            return false;
        }

        public override int GetHashCode()
        {
            return StdId.GetHashCode();
        }
    }
}
