using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Driver_Planner.ViewModels.Base;
using DriverPlanner.Command;
using DriverPlanner.DPService;
using DriverPlanner.Models.Classes;
using DriverPlanner.Models.Enums;

namespace DriverPlanner.ViewModels
{
	class ViewInstructorsViewModel : ViewModel
	{
		public ViewInstructorsViewModel()
		{
			using (DriverPlannerServiceClient dps = new DriverPlannerServiceClient())
			{
				var instrData = dps.GetFullProfiles();
				ListInstructors = new ObservableCollection<Instructor>(instrData.Item1);
				FeedbackList = new ObservableCollection<FeedBacks>(instrData.Item2);
				SelectedINdexInInstrList = -1;
				if (CurrentUserSingleton.CurrentRole == ERole.User) FeedBackAddVisibility = Visibility.Visible;
				else FeedBackAddVisibility = Visibility.Collapsed;

				#region Commands init
				AddFeedbackCommand = new LambdaCommand(OnExecuteAddFeedbackCommand, CanExecuteAddFeedbackCommand);

				#endregion
			}
		}

		#region Props

		private Visibility _feeBackAddVisibility = Visibility.Collapsed;

		public Visibility FeedBackAddVisibility
		{
			get { return _feeBackAddVisibility; }
			set { Set(ref _feeBackAddVisibility, value); }
		}

		private string _feedbackMsg;

		public string FeedbackMsg
		{
			get { return _feedbackMsg; }
			set { Set(ref _feedbackMsg, value); }
		}

		private Visibility _infoVisibility = Visibility.Hidden;

		public Visibility InfoVisibility
		{
			get { return _infoVisibility; }
			set { Set(ref _infoVisibility, value); }
		}
		private ObservableCollection<Instructor> _listInstructors;

		public ObservableCollection<Instructor> ListInstructors
		{
			get { return _listInstructors; }
			set { Set(ref _listInstructors, value); }
		}

		private ObservableCollection<FeedBacks> _feedbackList;

		public ObservableCollection<FeedBacks> FeedbackList
		{
			get { return _feedbackList; }
			set { Set(ref _feedbackList, value); }
		}

		private ObservableCollection<FeedBacks> _currentFeedbacks;

		public ObservableCollection<FeedBacks> CurrentFeedbacks
		{
			get { return _currentFeedbacks; }
			set { Set(ref _currentFeedbacks, value); }
		}

		private byte[] _instructorImage;

		public byte[] InstructorImage	
		{
			get { return _instructorImage; }
			set { Set(ref _instructorImage, value); }
		}

		private byte[] _carImage;

		public byte[] CarImage
		{
			get { return _carImage; }
			set { Set(ref _carImage, value); }
		}

		private int _selectedIndexInInstructorlist;

		public int SelectedINdexInInstrList
		{
			get { return _selectedIndexInInstructorlist; }
			set 
			{
				Set(ref _selectedIndexInInstructorlist, value);
				if (value!=-1)
				{
					CurrentInstructor = ListInstructors[value];
					using (var dps= new DriverPlannerServiceClient())
					{
						CarImage = dps.GetImage(CurrentInstructor.Car.ImageIndex);
						InstructorImage = dps.GetImage(CurrentInstructor.ImageIndex);
						CurrentFeedbacks = new ObservableCollection<FeedBacks>(FeedbackList.Where(t => t.InstructorID == CurrentInstructor.InstructorID).Reverse().ToList());
					}
				}
			}
		}

		private Instructor _currentInstructor;

		public Instructor CurrentInstructor
		{
			get { return _currentInstructor; }
			set 
			{ 
				Set(ref _currentInstructor, value);
				if (_currentInstructor!=null)
				{
					InfoVisibility = Visibility.Visible;
				}
				else
				{
					InfoVisibility = Visibility.Hidden;
				}
			}
		}

		#endregion

		#region Commands
		
		#region AddFeedbackCommand

		public ICommand AddFeedbackCommand { get; }
		private bool CanExecuteAddFeedbackCommand(object p)
		{
			return (CurrentUserSingleton.CurrentRole == ERole.User && CurrentInstructor != null && !String.IsNullOrEmpty(FeedbackMsg) && !String.IsNullOrWhiteSpace(FeedbackMsg));
		}

		private void OnExecuteAddFeedbackCommand(object p)
		{
			using (var dps = new DriverPlannerServiceClient())
			{
				string textFeedback = FeedbackMsg;

				FeedBacks fb = new FeedBacks();
				fb.FeedBackMessage = textFeedback;
				fb.UserID = ((User)CurrentUserSingleton.СurrentUser).UserID;
				fb.InstructorID = CurrentInstructor.InstructorID;

				if (dps.AddFeedback(fb))
				{
					FeedbackList = new ObservableCollection<FeedBacks>(dps.GetFeedbacks());
					CurrentFeedbacks = new ObservableCollection<FeedBacks>(FeedbackList.Where(t => t.InstructorID == CurrentInstructor.InstructorID).Reverse().ToList());
				}
			}
		}
		#endregion

		#endregion
	}
}
