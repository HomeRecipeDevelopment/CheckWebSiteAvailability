using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWebsiteAvailability
{
    class Program
    {
        static void Main(string[] args)
        {
            //var MyUrl = Console.ReadLine();
            AvailabilityCheck CurrentStatus = new AvailabilityCheck();
            string Status = CurrentStatus.CurrentStatus();
            //string Stat = CurrentStatus.GetCurrentStatus(MyUrl);
            Console.WriteLine(Status);
            
        }

    }
}
