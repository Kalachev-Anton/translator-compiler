﻿using recursive_descent_translator_app;
using System;
using System.Windows.Forms;

namespace compilers_recursive_descent_v1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());  // Создание экземпляра Form1
        }
    }
}
