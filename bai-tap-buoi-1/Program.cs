using System;
using BaiTapBuoi1;
using ExampleCAdvance.Hinh;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("===============================");
Console.WriteLine("       BÀI TẬP BUỔI 1          ");
Console.WriteLine("===============================");
Console.WriteLine("1. Chạy Bài 1 (Tính tổng a + b + c)");
Console.WriteLine("2. Chạy Bài 2 (Hình tròn: Chu vi & Diện tích)");
Console.WriteLine("0. Thoát");
Console.WriteLine("===============================");
Console.Write("Chọn bài cần chạy (0 - 2): ");

string? choice = Console.ReadLine();
Console.WriteLine();

switch (choice)
{
    case "1":
        Bai1.Run();
        break;
    case "2":
        Bai2.Run();
        break;
    case "0":
        Console.WriteLine("Đã thoát chương trình.");
        break;
    default:
        Console.WriteLine("Lựa chọn không hợp lệ!");
        break;
}
