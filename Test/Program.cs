using Hub.Controllers;
using Hub.DataAccess.Repositories;
using Hub.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            SectionTreeController contr = new SectionTreeController();
            var x = contr.InitializeTree();
        }
    }
}
