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

---

## 📂 Bài tập Buổi 3

> Thư mục: `bai-tap-buoi-3/`

### Tách kiến trúc 3 tầng (3-Layer Architecture)

Áp dụng mô hình phân tách tầng cho ứng dụng Quản lý Sinh viên:

```
┌────────────────────────────────────────────────────────┐
│                   Presentation Layer                   │
│   • ConsoleUI.cs (Chạy trên macOS/Linux/Windows)       │
│   • WinForms/FormMain.cs (Giao diện Windows Forms)     │
└──────────────────────────┬─────────────────────────────┘
                           │
┌──────────────────────────▼─────────────────────────────┐
│                  Business Logic Layer                  │
│   • StudentBLL.cs    (Validate, nghiệp vụ SV, thống kê)│
│   • ClsSubjectBLL.cs (Nghiệp vụ lớp môn học)           │
└──────────────────────────┬─────────────────────────────┘
                           │
┌──────────────────────────▼─────────────────────────────┐
│                   Data Access Layer                    │
│   • StudentDAL.cs    (CRUD SV với DataList)            │
│   • ClsSubjectDAL.cs (CRUD Lớp với DataList)           │
│   • DataSeeder.cs    (Dữ liệu mẫu khởi tạo)            │
└──────────────────────────┬─────────────────────────────┘
                           │
┌──────────────────────────▼─────────────────────────────┐
│                      Models Layer                      │
│   • Student.cs       (Thực thể Sinh viên)              │
│   • ClsSubject.cs    (Thực thể Lớp môn học)            │
└────────────────────────────────────────────────────────┘
```

#### Chi tiết các tầng:

1. **Models Layer (`Models/`)**:
   - `Student`: Thuộc tính `StdId`, `Name`, `MidPoint`, `FinalPoint`, `AvgPoint`, phân loại học lực (`GetGrade()`).
   - `ClsSubject`: Thuộc tính `Id`, `Name`, `Semester`, `Teacher`, `Students`.

2. **Data Access Layer (`DataAccess/`)**:
   - Ứng với mỗi thực thể, xây dựng một lớp thao tác trực tiếp với nguồn dữ liệu (`DataList`):
     - `StudentDAL`: `GetAll()`, `GetById()`, `SearchByName()`, `Insert()`, `Update()`, `Delete()`, `Exists()`.
     - `ClsSubjectDAL`: `GetAll()`, `GetById()`, `Insert()`, `Update()`, `Delete()`.
     - `DataSeeder`: Cung cấp dữ liệu mẫu khởi tạo.

3. **Business Logic Layer (`Business/`)**:
   - `StudentBLL`:
     - Kiểm tra / Validate dữ liệu (mã không trùng, điểm trong khoảng 0-10, các trường không rỗng). Trả về danh sách lỗi theo từng trường.
     - Xử lý nghiệp vụ: Thêm, sửa, xóa, tìm kiếm, sắp xếp, tính toán thống kê (điểm TB lớp, sinh viên cao nhất, thấp nhất, số đạt / không đạt).
   - `ClsSubjectBLL`:
     - Quản lý danh sách lớp, cung cấp dữ liệu cho ComboBox, liên kết `StudentDAL` tương ứng với từng lớp được chọn.

4. **Presentation Layer**:
   - **WinForms (`WinForms/FormMain.cs`)**:
     - `ComboBox` (`cboLop`): Hiển thị danh sách lớp, khi chọn lớp sẽ nạp danh sách SV tương ứng vào `DataGridView`.
     - Tìm kiếm 2 cơ chế: Tìm kiếm real-time lọc trên giao diện và nút **Tìm kiếm** gọi xuống tầng DAL/Database.
     - Ô nhập mã `txtMa` (`Leave` event): Nếu mã sinh viên tồn tại thì tự động điền thông tin lên form; nếu không tồn tại thì tự động xóa trống các ô.
     - Thêm / Sửa sinh viên: Bắt lỗi nghiệp vụ bằng `ErrorProvider` gắn trực tiếp vào các điều khiển bị sai (`txtMa`, `txtTen`, `txtGiuaKy`, `txtCuoiKy`).
   - **Console UI (`Presentation/ConsoleUI.cs`)**:
     - Triển khai đầy đủ các luồng tương tự WinForms (chọn ComboBox, xem danh sách dạng bảng, tra cứu auto-fill, validate báo lỗi từng trường, thêm/sửa/xóa, thống kê).
     - Chạy mượt mà trên macOS và mọi hệ điều hành hỗ trợ .NET.

#### Cách chạy:

- **Chạy Console (macOS / Linux / Windows):**
  ```bash
  cd bai-tap-buoi-3
  dotnet run
  ```

- **Chạy WinForms (Chỉ Windows):**
  ```bash
  cd bai-tap-buoi-3/WinForms
  dotnet run
  ```