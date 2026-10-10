using System;
using System.Collections.Generic;
using BaiTapBuoi3.DataAccess;
using BaiTapBuoi3.Models;

namespace BaiTapBuoi3.Business
{
    /// <summary>
    /// Tầng Business cho ClsSubject - xử lý nghiệp vụ liên quan đến lớp môn học
    /// </summary>
    public class ClsSubjectBLL
    {
        private readonly ClsSubjectDAL subjectDAL;

        public ClsSubjectBLL(ClsSubjectDAL subjectDAL)
        {
            this.subjectDAL = subjectDAL;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách lớp môn học (dùng cho ComboBox)
        /// </summary>
        public List<ClsSubject> GetAll()
        {
            return subjectDAL.GetAll();
        }

        /// <summary>
        /// Tìm lớp môn học theo mã
        /// </summary>
        public ClsSubject? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            return subjectDAL.GetById(id);
        }

        /// <summary>
        /// Thêm lớp môn học mới
        /// </summary>
        public bool Insert(ClsSubject subject)
        {
            return subjectDAL.Insert(subject);
        }

        /// <summary>
        /// Cập nhật thông tin lớp môn học
        /// </summary>
        public bool Update(ClsSubject subject)
        {
            return subjectDAL.Update(subject);
        }

        /// <summary>
        /// Xóa lớp môn học
        /// </summary>
        public bool Delete(string id)
        {
            return subjectDAL.Delete(id);
        }

        /// <summary>
        /// Lấy StudentDAL từ lớp môn học được chọn
        /// (Dùng để tạo StudentBLL cho lớp cụ thể)
        /// </summary>
        public StudentDAL GetStudentDALForSubject(string subjectId)
        {
            var subject = subjectDAL.GetById(subjectId);
            if (subject == null)
                throw new ArgumentException($"Không tìm thấy lớp mã '{subjectId}'.");

            return new StudentDAL(subject.Students);
        }
    }
}
