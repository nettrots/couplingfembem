﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SbB.Diploma;
using SbB.Diploma.Methods;

namespace TestingCouplingClesses
{
    class Program
    {
        static void Main(string[] args)
        {
            CouplingMethod target = new CouplingMethod(@"c:\Documents and Settings\Admin\Мои документы\Visual Studio 2008\Projects\1\CouplingFEMBEM\CouplingFEMBEM\Cofig\example.yaml");
            
            target.assemble();
            target.run();
            target.fillSystem();
            target.solveSystem();
            string s = "dfgdf";
            Console.WriteLine("Hello 1");
            Console.WriteLine("Hello 2");
            
            Console.Write("prevet!!");
            int i = 1;
            double r = 5;
        }
    }
}
