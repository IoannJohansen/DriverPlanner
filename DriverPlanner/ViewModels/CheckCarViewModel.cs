using DriverPlanner.Entities;
using DriverPlanner.ViewModels;
using DriverPlanner.ViewModels.Base;

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
