using PosSystem.AdoDotNet;
using System;
using System.Data;
using System.Data.SqlClient;

namespace POSConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Run run = new Run();
            run.Start();
        }
    }
}
