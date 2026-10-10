using System;
using System.Collections.Generic;
using System.Linq;
using BaiTapBuoi3.DataAccess;
using BaiTapBuoi3.Models;

namespace BaiTapBuoi3.Business
{
    /// <summary>
    /// Tầng Business cho Student - xử lý nghiệp vụ liên quan đến sinh viên
    /// </summary>
    public class StudentBLL
    {
        private readonly StudentDAL studentDAL;

        public StudentBLL(StudentDAL studentDAL)
        {
            this.studentDAL = studentDAL;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sinh viên
        /// </summary>
        public List<Student> GetAll()
        {
            return studentDAL.GetAll();
        }

        /// <summary>
        /// Tìm sinh viên theo mã
        /// </summary>
        public Student? GetById(string stdId)
        {
            if (string.IsNullOrWhiteSpace(stdId))
                return null;
            return studentDAL.GetById(stdId);
        }

        /// <summary>
        /// Tìm kiếm sinh viên theo tên
        /// </summary>
        public List<Student> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return studentDAL.GetAll();
            return studentDAL.SearchByName(keyword);
        }

        /// <summary>
        /// Validate thông tin sinh viên, trả về Dictionary lỗi theo tên trường
        /// </summary>
        public Dictionary<string, string> Validate(Student student, bool isUpdate = false)
        {
            var errors = new Dictionary<string, string>();

            // Validate mã sinh viên
            if (string.IsNullOrWhiteSpace(student.StdId))
                errors["StdId"] = "Mã sinh viên không được để trống.";
            else if (!isUpdate && studentDAL.Exists(student.StdId))
                errors["StdId"] = $"Mã sinh viên '{student.StdId}' đã tồn tại.";

            // Validate tên
            if (string.IsNullOrWhiteSpace(student.Name))
                errors["Name"] = "Tên sinh viên không được để trống.";

            // Validate điểm giữa kỳ
            if (student.MidPoint < 0 || student.MidPoint > 10)
                errors["MidPoint"] = "Điểm giữa kỳ phải từ 0 đến 10.";

            // Validate điểm cuối kỳ
            if (student.FinalPoint < 0 || student.FinalPoint > 10)
                errors["FinalPoint"] = "Điểm cuối kỳ phải từ 0 đến 10.";

            return errors;
        }

        /// <summary>
        /// Thêm sinh viên mới (có validate)
        /// </summary>
        public (bool success, Dictionary<string, string> errors) Insert(Student student)
        {
            var errors = Validate(student, isUpdate: false);
            if (errors.Count > 0)
                return (false, errors);

            bool result = studentDAL.Insert(student);
            if (!result)
                errors["StdId"] = $"Mã sinh viên '{student.StdId}' đã tồn tại.";

            return (result, errors);
        }

        /// <summary>
        /// Cập nhật sinh viên (có validate)
        /// </summary>
        public (bool success, Dictionary<string, string> errors) Update(Student student)
        {
            var errors = Validate(student, isUpdate: true);
            if (errors.Count > 0)
                return (false, errors);

            // Kiểm tra sinh viên có tồn tại không
            if (!studentDAL.Exists(student.StdId))
            {
                errors["StdId"] = $"Không tìm thấy sinh viên mã '{student.StdId}'.";
                return (false, errors);
            }

            bool result = studentDAL.Update(student);
            return (result, errors);
        }

        /// <summary>
        /// Xóa sinh viên
        /// </summary>
        public bool Delete(string stdId)
        {
            return studentDAL.Delete(stdId);
        }

        /// <summary>
        /// Sắp xếp danh sách theo điểm trung bình
        /// </summary>
        public List<Student> GetSortedByAvg(bool descending = true)
        {
            var list = studentDAL.GetAll();
            return descending
                ? list.OrderByDescending(s => s.AvgPoint).ToList()
                : list.OrderBy(s => s.AvgPoint).ToList();
        }

        /// <summary>
        /// Thống kê lớp
        /// </summary>
        public (int total, double classAvg, int passed, int failed, Student? top, Student? lowest) GetStatistics()
        {
            var list = studentDAL.GetAll();
            if (list.Count == 0)
                return (0, 0, 0, 0, null, null);

            return (
                total: list.Count,
                classAvg: Math.Round(list.Average(s => s.AvgPoint), 2),
                passed: list.Count(s => s.AvgPoint >= 5.0),
                failed: list.Count(s => s.AvgPoint < 5.0),
                top: list.MaxBy(s => s.AvgPoint),
                lowest: list.MinBy(s => s.AvgPoint)
            );
        }
    }
}
