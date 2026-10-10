using BaiTapBuoi3.DataAccess;
using BaiTapBuoi3.Business;
using BaiTapBuoi3.Presentation;

// Khởi tạo dữ liệu mẫu
var subjectData = DataSeeder.SeedSubjects();

// Khởi tạo các tầng: DAL → BLL → Presentation
var subjectDAL = new ClsSubjectDAL(subjectData);
var subjectBLL = new ClsSubjectBLL(subjectDAL);

// Chạy giao diện Console
var ui = new ConsoleUI(subjectBLL);
ui.Run();
