using System;
using System.Collections.Generic;
using System.Linq;
using BaiTapBuoi3.Models;

namespace BaiTapBuoi3.DataAccess
{
    /// <summary>
    /// Tầng DataAccess cho ClsSubject - thao tác CRUD trực tiếp với nguồn dữ liệu (DataList)
    /// </summary>
    public class ClsSubjectDAL
    {
        // Nguồn dữ liệu (DataList)
        private readonly List<ClsSubject> dataList;

        public ClsSubjectDAL(List<ClsSubject> dataList)
        {
            this.dataList = dataList;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách lớp môn học
        /// </summary>
        public List<ClsSubject> GetAll()
        {
            return dataList.ToList();
        }

        /// <summary>
        /// Tìm lớp môn học theo mã
        /// </summary>
        public ClsSubject? GetById(string id)
        {
            return dataList.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Thêm lớp môn học
        /// </summary>
        public bool Insert(ClsSubject subject)
        {
            if (dataList.Any(s => s.Id == subject.Id))
                return false;

            dataList.Add(subject);
            return true;
        }

        /// <summary>
        /// Cập nhật thông tin lớp môn học
        /// </summary>
        public bool Update(ClsSubject subject)
        {
            var existing = GetById(subject.Id);
            if (existing == null)
                return false;

            existing.Name = subject.Name;
            existing.Semester = subject.Semester;
            existing.Teacher = subject.Teacher;
            return true;
        }

        /// <summary>
        /// Xóa lớp môn học theo mã
        /// </summary>
        public bool Delete(string id)
        {
            var subject = GetById(id);
            if (subject == null)
                return false;

            dataList.Remove(subject);
            return true;
        }
    }
}
