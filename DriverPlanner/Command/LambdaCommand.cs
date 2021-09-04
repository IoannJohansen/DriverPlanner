using System;

namespace DriverPlanner.Command
{
	internal class LambdaCommand : DriverPlanner.Command.Base.Command
	{
		private readonly Action<object> _execute;
		private readonly Func<object, bool> _canExecute;

		public LambdaCommand(Action<object> _execute, Func<object, bool> _canExecute = null)
		{
			this._execute = _execute;
			this._canExecute = _canExecute;
		}
		public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

		public override void Execute(object parameter) => _execute(parameter);
	}
}
