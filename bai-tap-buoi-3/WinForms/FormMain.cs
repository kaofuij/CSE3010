using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BaiTapBuoi3.Models;
using BaiTapBuoi3.DataAccess;
using BaiTapBuoi3.Business;

namespace BaiTapBuoi3.WinForms
{
    /// <summary>
    /// Form chính - Quản lý sinh viên theo kiến trúc 3 tầng
    /// Presentation Layer: chỉ xử lý giao diện
    /// </summary>
    public class FormMain : Form
    {
        // ==================== Business Layer ====================
        private ClsSubjectBLL subjectBLL;
        private StudentBLL? studentBLL;

        // ==================== Controls ====================
        // Phần chọn lớp
        private Label lblChonLop;
        private ComboBox cboLop;

        // Phần thông tin sinh viên
        private GroupBox grpThongTin;
        private Label lblMa, lblTen, lblGiuaKy, lblCuoiKy, lblTB, lblXepLoai;
        private TextBox txtMa, txtTen, txtGiuaKy, txtCuoiKy, txtTB, txtXepLoai;

        // Nút chức năng
        private Button btnThem, btnSua, btnXoa, btnLamMoi;

        // Phần tìm kiếm
        private GroupBox grpTimKiem;
        private TextBox txtTimKiem;
        private Button btnTimKiem;

        // Bảng danh sách
        private DataGridView dgvSinhVien;

        // Thống kê
        private GroupBox grpThongKe;
        private Label lblTongSV, lblTBLop, lblSVDat, lblSVKhongDat;

        // ErrorProvider
        private ErrorProvider errorProvider;

        public FormMain()
        {
            // Khởi tạo tầng DAL → BLL
            var subjectData = DataSeeder.SeedSubjects();
            var subjectDAL = new ClsSubjectDAL(subjectData);
            subjectBLL = new ClsSubjectBLL(subjectDAL);

            InitializeComponents();
            LoadSubjectsToComboBox();
        }

        // ==================== Khởi tạo giao diện ====================

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Quản lý Sinh viên - Bài tập Buổi 3 (3-Layer)";
            this.Size = new Size(950, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f);

            // ErrorProvider
            errorProvider = new ErrorProvider(this);
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // ── Phần chọn lớp ──
            lblChonLop = new Label { Text = "Chọn lớp môn học:", Location = new Point(15, 15), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            cboLop = new ComboBox { Location = new Point(170, 12), Size = new Size(400, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            cboLop.SelectedIndexChanged += CboLop_SelectedIndexChanged;

            this.Controls.Add(lblChonLop);
            this.Controls.Add(cboLop);

            // ── Phần thông tin sinh viên ──
            grpThongTin = new GroupBox { Text = "Thông tin sinh viên", Location = new Point(15, 50), Size = new Size(560, 170) };

            lblMa = new Label { Text = "Mã SV:", Location = new Point(15, 30), AutoSize = true };
            txtMa = new TextBox { Location = new Point(110, 27), Size = new Size(150, 25) };
            txtMa.Leave += TxtMa_Leave; // Khi rời txtMa → auto-fill

            lblTen = new Label { Text = "Họ và tên:", Location = new Point(15, 65), AutoSize = true };
            txtTen = new TextBox { Location = new Point(110, 62), Size = new Size(250, 25) };

            lblGiuaKy = new Label { Text = "Giữa kỳ:", Location = new Point(15, 100), AutoSize = true };
            txtGiuaKy = new TextBox { Location = new Point(110, 97), Size = new Size(80, 25) };

            lblCuoiKy = new Label { Text = "Cuối kỳ:", Location = new Point(210, 100), AutoSize = true };
            txtCuoiKy = new TextBox { Location = new Point(280, 97), Size = new Size(80, 25) };

            lblTB = new Label { Text = "TB:", Location = new Point(380, 100), AutoSize = true };
            txtTB = new TextBox { Location = new Point(410, 97), Size = new Size(60, 25), ReadOnly = true, BackColor = Color.LightYellow };

            lblXepLoai = new Label { Text = "Xếp loại:", Location = new Point(15, 135), AutoSize = true };
            txtXepLoai = new TextBox { Location = new Point(110, 132), Size = new Size(120, 25), ReadOnly = true, BackColor = Color.LightYellow };

            grpThongTin.Controls.AddRange(new Control[] { lblMa, txtMa, lblTen, txtTen, lblGiuaKy, txtGiuaKy, lblCuoiKy, txtCuoiKy, lblTB, txtTB, lblXepLoai, txtXepLoai });
            this.Controls.Add(grpThongTin);

            // ── Nút chức năng ──
            btnThem = new Button { Text = "Thêm", Location = new Point(590, 70), Size = new Size(100, 35), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White };
            btnSua = new Button { Text = "Sửa", Location = new Point(700, 70), Size = new Size(100, 35), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White };
            btnXoa = new Button { Text = "Xóa", Location = new Point(810, 70), Size = new Size(100, 35), BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(590, 115), Size = new Size(100, 35), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White };

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            this.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            // ── Phần tìm kiếm ──
            grpTimKiem = new GroupBox { Text = "Tìm kiếm", Location = new Point(15, 230), Size = new Size(900, 55) };
            txtTimKiem = new TextBox { Location = new Point(15, 22), Size = new Size(350, 25), PlaceholderText = "Nhập mã SV hoặc tên để tìm kiếm..." };
            btnTimKiem = new Button { Text = "Tìm kiếm", Location = new Point(380, 20), Size = new Size(100, 28), BackColor = Color.FromArgb(155, 89, 182), ForeColor = Color.White };
            btnTimKiem.Click += BtnTimKiem_Click;
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged; // Tìm kiếm trên giao diện (real-time)

            grpTimKiem.Controls.AddRange(new Control[] { txtTimKiem, btnTimKiem });
            this.Controls.Add(grpTimKiem);

            // ── DataGridView ──
            dgvSinhVien = new DataGridView
            {
                Location = new Point(15, 295),
                Size = new Size(900, 270),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            dgvSinhVien.CellClick += DgvSinhVien_CellClick;

            this.Controls.Add(dgvSinhVien);

            // ── Thống kê ──
            grpThongKe = new GroupBox { Text = "Thống kê", Location = new Point(15, 575), Size = new Size(900, 50) };
            lblTongSV = new Label { Text = "Tổng: 0 SV", Location = new Point(15, 22), AutoSize = true };
            lblTBLop = new Label { Text = "TB lớp: 0.00", Location = new Point(200, 22), AutoSize = true };
            lblSVDat = new Label { Text = "Đạt: 0", Location = new Point(400, 22), AutoSize = true, ForeColor = Color.Green };
            lblSVKhongDat = new Label { Text = "Không đạt: 0", Location = new Point(550, 22), AutoSize = true, ForeColor = Color.Red };

            grpThongKe.Controls.AddRange(new Control[] { lblTongSV, lblTBLop, lblSVDat, lblSVKhongDat });
            this.Controls.Add(grpThongKe);
        }

        // ==================== Xử lý sự kiện ====================

        /// <summary>
        /// ComboBox: Khi chọn lớp → hiển thị sinh viên của lớp đó
        /// </summary>
        private void CboLop_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboLop.SelectedItem is ClsSubject selected)
            {
                var studentDAL = subjectBLL.GetStudentDALForSubject(selected.Id);
                studentBLL = new StudentBLL(studentDAL);
                LoadStudentGrid();
                ClearForm();
                UpdateStatistics();
            }
        }

        /// <summary>
        /// txtMa: Khi rời khỏi ô mã SV → auto-fill hoặc xóa trống
        /// </summary>
        private void TxtMa_Leave(object? sender, EventArgs e)
        {
            if (studentBLL == null) return;

            string stdId = txtMa.Text.Trim();
            if (string.IsNullOrEmpty(stdId))
            {
                ClearForm();
                return;
            }

            var student = studentBLL.GetById(stdId);
            if (student != null)
            {
                // Tồn tại → hiển thị thông tin
                txtTen.Text = student.Name;
                txtGiuaKy.Text = student.MidPoint.ToString("F1");
                txtCuoiKy.Text = student.FinalPoint.ToString("F1");
                txtTB.Text = student.AvgPoint.ToString("F2");
                txtXepLoai.Text = student.GetGrade();
                errorProvider.SetError(txtMa, "");
            }
            else
            {
                // Không tồn tại → xóa trống
                txtTen.Text = "";
                txtGiuaKy.Text = "";
                txtCuoiKy.Text = "";
                txtTB.Text = "";
                txtXepLoai.Text = "";
            }
        }

        /// <summary>
        /// Tìm kiếm real-time trên giao diện (lọc DataGridView)
        /// </summary>
        private void TxtTimKiem_TextChanged(object? sender, EventArgs e)
        {
            if (studentBLL == null) return;

            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadStudentGrid();
                return;
            }

            // Tìm kiếm trên giao diện: lọc theo mã hoặc tên
            var all = studentBLL.GetAll();
            var filtered = all.Where(s =>
                s.StdId.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                s.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            BindStudentGrid(filtered);
        }

        /// <summary>
        /// Nút Tìm kiếm: tìm kiếm phía cơ sở dữ liệu (qua BLL → DAL)
        /// </summary>
        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            if (studentBLL == null) return;

            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadStudentGrid();
                return;
            }

            // Tìm kiếm phía cơ sở dữ liệu (qua BLL)
            var byId = studentBLL.GetById(keyword);
            var byName = studentBLL.SearchByName(keyword);

            var results = new List<Student>();
            if (byId != null && !byName.Any(s => s.StdId == byId.StdId))
                results.Add(byId);
            results.AddRange(byName);

            BindStudentGrid(results);

            if (results.Count == 0)
                MessageBox.Show($"Không tìm thấy sinh viên với từ khóa '{keyword}'.", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Nút Thêm: validate → thêm qua BLL → hiển thị lỗi ErrorProvider
        /// </summary>
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (studentBLL == null) return;
            ClearErrors();

            var student = GetStudentFromForm();
            if (student == null) return;

            var (success, errors) = studentBLL.Insert(student);
            if (success)
            {
                MessageBox.Show($"Đã thêm sinh viên '{student.Name}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudentGrid();
                ClearForm();
                UpdateStatistics();
            }
            else
            {
                ShowErrors(errors);
            }
        }

        /// <summary>
        /// Nút Sửa: validate → cập nhật qua BLL → hiển thị lỗi ErrorProvider
        /// </summary>
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (studentBLL == null) return;
            ClearErrors();

            var student = GetStudentFromForm();
            if (student == null) return;

            var (success, errors) = studentBLL.Update(student);
            if (success)
            {
                MessageBox.Show($"Đã cập nhật sinh viên '{student.Name}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudentGrid();
                UpdateStatistics();
            }
            else
            {
                ShowErrors(errors);
            }
        }

        /// <summary>
        /// Nút Xóa
        /// </summary>
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (studentBLL == null) return;

            string stdId = txtMa.Text.Trim();
            if (string.IsNullOrEmpty(stdId))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên mã '{stdId}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (studentBLL.Delete(stdId))
                {
                    MessageBox.Show("Đã xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudentGrid();
                    ClearForm();
                    UpdateStatistics();
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy sinh viên mã '{stdId}'.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Nút Làm mới
        /// </summary>
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearForm();
            ClearErrors();
            txtTimKiem.Text = "";
            LoadStudentGrid();
        }

        /// <summary>
        /// Click vào dòng trên DataGridView → hiển thị thông tin lên form
        /// </summary>
        private void DgvSinhVien_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvSinhVien.Rows[e.RowIndex];
            txtMa.Text = row.Cells["StdId"].Value?.ToString() ?? "";
            txtTen.Text = row.Cells["Name"].Value?.ToString() ?? "";
            txtGiuaKy.Text = row.Cells["MidPoint"].Value?.ToString() ?? "";
            txtCuoiKy.Text = row.Cells["FinalPoint"].Value?.ToString() ?? "";
            txtTB.Text = row.Cells["AvgPoint"].Value?.ToString() ?? "";
            txtXepLoai.Text = row.Cells["Grade"].Value?.ToString() ?? "";
        }

        // ==================== Helpers ====================

        private void LoadSubjectsToComboBox()
        {
            cboLop.Items.Clear();
            var subjects = subjectBLL.GetAll();
            foreach (var s in subjects)
                cboLop.Items.Add(s);

            if (cboLop.Items.Count > 0)
                cboLop.SelectedIndex = 0;
        }

        private void LoadStudentGrid()
        {
            if (studentBLL == null) return;
            var students = studentBLL.GetAll();
            BindStudentGrid(students);
        }

        private void BindStudentGrid(List<Student> students)
        {
            dgvSinhVien.DataSource = null;
            dgvSinhVien.Columns.Clear();

            var data = students.Select(s => new
            {
                StdId = s.StdId,
                Name = s.Name,
                MidPoint = s.MidPoint,
                FinalPoint = s.FinalPoint,
                AvgPoint = s.AvgPoint,
                Grade = s.GetGrade()
            }).ToList();

            dgvSinhVien.DataSource = data;

            // Đặt tiêu đề cột tiếng Việt
            if (dgvSinhVien.Columns.Count >= 6)
            {
                dgvSinhVien.Columns["StdId"].HeaderText = "Mã SV";
                dgvSinhVien.Columns["Name"].HeaderText = "Họ và tên";
                dgvSinhVien.Columns["MidPoint"].HeaderText = "Giữa kỳ";
                dgvSinhVien.Columns["FinalPoint"].HeaderText = "Cuối kỳ";
                dgvSinhVien.Columns["AvgPoint"].HeaderText = "TB";
                dgvSinhVien.Columns["Grade"].HeaderText = "Xếp loại";
            }
        }

        /// <summary>
        /// Lấy thông tin sinh viên từ form, validate cơ bản kiểu dữ liệu
        /// </summary>
        private Student? GetStudentFromForm()
        {
            ClearErrors();
            bool hasError = false;

            string stdId = txtMa.Text.Trim();
            string name = txtTen.Text.Trim();

            if (string.IsNullOrEmpty(stdId))
            {
                errorProvider.SetError(txtMa, "Mã sinh viên không được để trống.");
                hasError = true;
            }

            if (string.IsNullOrEmpty(name))
            {
                errorProvider.SetError(txtTen, "Tên sinh viên không được để trống.");
                hasError = true;
            }

            if (!double.TryParse(txtGiuaKy.Text.Trim(), out double midPoint) || midPoint < 0 || midPoint > 10)
            {
                errorProvider.SetError(txtGiuaKy, "Điểm giữa kỳ phải là số từ 0 đến 10.");
                hasError = true;
            }

            if (!double.TryParse(txtCuoiKy.Text.Trim(), out double finalPoint) || finalPoint < 0 || finalPoint > 10)
            {
                errorProvider.SetError(txtCuoiKy, "Điểm cuối kỳ phải là số từ 0 đến 10.");
                hasError = true;
            }

            if (hasError) return null;

            return new Student(stdId, name, midPoint, finalPoint);
        }

        /// <summary>
        /// Hiển thị lỗi từ BLL lên ErrorProvider
        /// </summary>
        private void ShowErrors(Dictionary<string, string> errors)
        {
            foreach (var error in errors)
            {
                switch (error.Key)
                {
                    case "StdId": errorProvider.SetError(txtMa, error.Value); break;
                    case "Name": errorProvider.SetError(txtTen, error.Value); break;
                    case "MidPoint": errorProvider.SetError(txtGiuaKy, error.Value); break;
                    case "FinalPoint": errorProvider.SetError(txtCuoiKy, error.Value); break;
                }
            }
        }

        private void ClearForm()
        {
            txtMa.Text = "";
            txtTen.Text = "";
            txtGiuaKy.Text = "";
            txtCuoiKy.Text = "";
            txtTB.Text = "";
            txtXepLoai.Text = "";
            txtMa.Focus();
        }

        private void ClearErrors()
        {
            errorProvider.SetError(txtMa, "");
            errorProvider.SetError(txtTen, "");
            errorProvider.SetError(txtGiuaKy, "");
            errorProvider.SetError(txtCuoiKy, "");
        }

        private void UpdateStatistics()
        {
            if (studentBLL == null) return;

            var (total, classAvg, passed, failed, _, _) = studentBLL.GetStatistics();
            lblTongSV.Text = $"Tổng: {total} SV";
            lblTBLop.Text = $"TB lớp: {classAvg:F2}";
            lblSVDat.Text = $"Đạt: {passed}";
            lblSVKhongDat.Text = $"Không đạt: {failed}";
        }
    }
}
