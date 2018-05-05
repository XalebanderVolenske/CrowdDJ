using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.DAL;

namespace CrowdDj.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                MySeed.Seed(context);
            }
        }
    }
}
