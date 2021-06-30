using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver_Planner.ViewModels
{
	class CheckCarViewModel : ViewModel
	{
		public CheckCarViewModel()
		{

		}

		public CheckCarViewModel(Cars instance)
		{
			this.Car = instance;

		}

		public Cars Car { get; set; }

		#region Props

		public AutoParkViewModel ParrentVM { get; set; }

		#endregion
	}
}
