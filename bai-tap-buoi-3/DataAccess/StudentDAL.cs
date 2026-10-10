using System;
using System.Collections.Generic;
using System.Linq;
using BaiTapBuoi3.Models;

namespace BaiTapBuoi3.DataAccess
{
    /// <summary>
    /// Tầng DataAccess cho Student - thao tác CRUD trực tiếp với nguồn dữ liệu (DataList)
    /// </summary>
    public class StudentDAL
    {
        // Nguồn dữ liệu (DataList) - trong thực tế sẽ là Database
        private readonly List<Student> dataList;

        public StudentDAL(List<Student> dataList)
        {
            this.dataList = dataList;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sinh viên
        /// </summary>
        public List<Student> GetAll()
        {
            return dataList.ToList();
        }

        /// <summary>
        /// Tìm sinh viên theo mã
        /// </summary>
        public Student? GetById(string stdId)
        {
            return dataList.FirstOrDefault(s => s.StdId == stdId);
        }

        /// <summary>
        /// Tìm sinh viên theo tên (chứa từ khóa)
        /// </summary>
        public List<Student> SearchByName(string keyword)
        {
            return dataList
                .Where(s => s.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Thêm sinh viên vào DataList
        /// </summary>
        public bool Insert(Student student)
        {
            if (dataList.Any(s => s.StdId == student.StdId))
                return false;

            dataList.Add(student);
            return true;
        }

        /// <summary>
        /// Cập nhật thông tin sinh viên
        /// </summary>
        public bool Update(Student student)
        {
            var existing = GetById(student.StdId);
            if (existing == null)
                return false;

            existing.Name = student.Name;
            existing.MidPoint = student.MidPoint;
            existing.FinalPoint = student.FinalPoint;
            return true;
        }

        /// <summary>
        /// Xóa sinh viên theo mã
        /// </summary>
        public bool Delete(string stdId)
        {
            var student = GetById(stdId);
            if (student == null)
                return false;

            dataList.Remove(student);
            return true;
        }

        /// <summary>
        /// Kiểm tra mã sinh viên đã tồn tại chưa
        /// </summary>
        public bool Exists(string stdId)
        {
            return dataList.Any(s => s.StdId == stdId);
        }
    }
}
