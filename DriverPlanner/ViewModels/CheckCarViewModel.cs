using Driver_Planner.ViewModels;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.DPService;

namespace DriverPlanner.ViewModels
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

		public AutoParkViewModel ParrentVm { get; set; }

		#endregion
	}
}
