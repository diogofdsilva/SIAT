using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIAT.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = new AnalyzePositionWorker();
            Console.WriteLine("Created new worker - Waiting!");
            Console.Read();
            Console.WriteLine("Leaving!");
        }
    }
}
