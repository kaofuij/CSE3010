using System;

int a, b, c;

Console.Write("Nhập a: ");
while (!int.TryParse(Console.ReadLine(), out a))
    Console.Write("Nhập sai! Nhập lại a: ");

Console.Write("Nhập b: ");
while (!int.TryParse(Console.ReadLine(), out b))
    Console.Write("Nhập sai! Nhập lại b: ");

Console.Write("Nhập c: ");
while (!int.TryParse(Console.ReadLine(), out c))
    Console.Write("Nhập sai! Nhập lại c: ");

Console.WriteLine($"{a} + {b} + {c} = {a + b + c}");