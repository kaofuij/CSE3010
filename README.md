# CSE3010 - Lập trình nâng cao (C#)

## Yêu cầu

- [.NET SDK](https://dotnet.microsoft.com/download) (phiên bản 8.0 trở lên)

## Cách chạy

```bash
cd <tên-thư-mục-bài-tập>
dotnet run
```

---

## 📂 Bài tập Buổi 1

> Thư mục: `bai-tap-buoi-1/`

### Bài 1: Nhập và tính tổng 3 số nguyên
- Nhập 3 số nguyên `a`, `b`, `c` từ bàn phím
- Dùng `int.TryParse` để kiểm tra và yêu cầu nhập lại nếu sai định dạng
- In ra kết quả tổng `a + b + c`

### Bài 2: Interface & tính diện tích, chu vi hình tròn
- Định nghĩa interface `IHinh` gồm 2 phương thức: `getDienTich()`, `getChuVi()`
- Class `HinhTron` implement `IHinh`, có validate bán kính không được âm

---

## 📂 Bài tập Buổi 2

> Thư mục: `bai-tap-buoi-2/`

### Ứng dụng Quản lý Sinh viên

**Lớp thực thể:**

| Class | Thuộc tính |
|-------|-----------|
| `Student` | `stdId`, `name`, `midPoint`, `finalPoint` |
| `ClsSubject` | `id`, `Name`, `Semester`, `teacher`, `List<Student> students` |

**Quan hệ:** 1 `ClsSubject` chứa nhiều `Student` (1 - *)

**Chức năng:**
- Quản lý lớp môn học (CRUD)
- Quản lý sinh viên trong lớp (thêm, xóa, cập nhật điểm)
- Tìm kiếm sinh viên theo mã hoặc tên
- Sắp xếp theo điểm trung bình
- Thống kê: điểm TB lớp, SV cao/thấp nhất, số đạt/không đạt