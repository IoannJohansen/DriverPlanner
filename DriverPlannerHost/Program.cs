using System;
using System.ServiceModel;

namespace DriverPlannerHost
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var host = new ServiceHost(typeof(DriverPlannerService.DriverPlannerService)))
			{
				try
				{
					Console.ForegroundColor = ConsoleColor.Green;
					host.Open();
					Console.WriteLine("Connection alive");
					Console.ReadLine();
					
					
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.ReadLine();
					//throw;
				}
			}
		}
	}
}
