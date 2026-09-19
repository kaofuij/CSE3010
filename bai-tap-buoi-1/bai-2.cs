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
}