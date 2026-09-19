using System;

namespace ExampleCAdvance.Hinh
{
    public interface IHinh
    {
        double getDienTich();
        double getChuVi();
    }

    public class HinhTron : IHinh
    {
        private double banKinh;
        public double BanKinh
        {
            get => banKinh;
            set
            {
                if (value < 0)
                    throw new Exception("Bán kính không được âm");
                banKinh = value;
            }
        }

        public HinhTron(double banKinh)
        {
            BanKinh = banKinh;
        }

        public double getDienTich() => Math.PI * banKinh * banKinh;
        public double getChuVi() => 2 * Math.PI * banKinh;
    }

    public class Bai2
    {
        public static void Run()
        {
            Console.Write("Nhập bán kính hình tròn: ");
            double r;
            while (!double.TryParse(Console.ReadLine(), out r) || r < 0)
                Console.Write("Bán kính không hợp lệ (phải >= 0)! Nhập lại: ");

            IHinh hinhTron = new HinhTron(r);
            Console.WriteLine($"Chu vi hình tròn: {hinhTron.getChuVi():F2}");
            Console.WriteLine($"Diện tích hình tròn: {hinhTron.getDienTich():F2}");
        }
    }
}