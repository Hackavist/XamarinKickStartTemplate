using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TemplateFoundation.ExtensionMethods;

namespace TemplateFoundation.Commands
{
	/// <summary>
	///     An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public sealed class AsyncCommand<T> : IAsyncCommand<T>
	{
		#region Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.AsyncCommand`1" /> class.
		/// </summary>
		/// <param name="execute">
		///     The Function executed when Execute or ExecuteAysnc is called. This does not check canExecute
		///     before executing and will execute even if canExecute is false
		/// </param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">
		///     If an exception is thrown in the Task, <c>onException</c> will execute. If onException is
		///     null, the exception will be re-thrown
		/// </param>
		/// <param name="continueOnCapturedContext">
		///     If set to <c>true</c> continue on captured context; this will ensure that the
		///     Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this
		///     will allow the Synchronization Context to continue on a different thread
		/// </param>
		public AsyncCommand(Func<T, Task> execute,
			Func<object, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = true)
		{
			this.execute = execute ??
			               throw new ArgumentNullException(nameof(execute),
				               $"Async Command can't be null : Command Name = {nameof(execute)}");
			this.canExecute = canExecute ?? (_ => true);
			this.onException = onException;
			this.continueOnCapturedContext = continueOnCapturedContext;
		}

		#endregion

		#region Interface Implementations

		/// <summary>
		///     Occurs when changes occur that affect whether or not the command should execute
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => this.weakEventManager.AddEventHandler(value);
			remove => this.weakEventManager.RemoveEventHandler(value);
		}

		#endregion

		#region Constant Fields

		private readonly Func<T, Task> execute;
		private readonly Func<object, bool> canExecute;
		private readonly Action<Exception> onException;
		private readonly bool continueOnCapturedContext;
		private readonly WeakEventManager.WeakEventManager weakEventManager = new WeakEventManager.WeakEventManager();
		private volatile bool executionInProgress;

		#endregion

		#region Methods

		/// <summary>
		///     Determines whether the command can execute in its current state
		/// </summary>
		/// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
		/// <param name="parameter">
		///     Data used by the command. If the command does not require data to be passed, this object can be
		///     set to null.
		/// </param>
		public bool CanExecute(object parameter)
		{
			if (this.executionInProgress) return false;
			return this.canExecute == null || this.canExecute(parameter);
		}

		/// <summary>
		///     Raises the CanExecuteChanged event.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			this.weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));
		}

		/// <summary>
		///     Executes the Command as a Task
		/// </summary>
		/// <returns>The executed Task</returns>
		/// <param name="parameter">
		///     Data used by the command. If the command does not require data to be passed, this object can be
		///     set to null.
		/// </param>
		public Task ExecuteAsync(T parameter)
		{
			try
			{
				this.executionInProgress = true;
				RaiseCanExecuteChanged();
				return this.execute(parameter);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			finally
			{
				this.executionInProgress = false;
				RaiseCanExecuteChanged();
			}
		}

		void ICommand.Execute(object parameter)
		{
			if (!CanExecute(parameter)) return;
			switch (parameter)
			{
				case T validParameter:
					ExecuteAsync(validParameter).SafeFireAndForget(this.continueOnCapturedContext, this.onException);
					break;
				case null when !typeof(T).IsValueType:
					ExecuteAsync(default).SafeFireAndForget(this.continueOnCapturedContext, this.onException);
					break;
				default:
					throw new InvalidCommandParameterException(typeof(T), parameter.GetType());
			}
		}

		#endregion
	}

	/// <summary>
	///     An implmentation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public sealed class AsyncCommand : IAsyncCommand
	{
		#region Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.AsyncCommand`1" /> class.
		/// </summary>
		/// <param name="execute">
		///     The Function executed when Execute or ExecuteAysnc is called. This does not check canExecute
		///     before executing and will execute even if canExecute is false
		/// </param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">
		///     If an exception is thrown in the Task, <c>onException</c> will execute. If onException is
		///     null, the exception will be re-thrown
		/// </param>
		/// <param name="continueOnCapturedContext">
		///     If set to <c>true</c> continue on captured context; this will ensure that the
		///     Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this
		///     will allow the Synchronization Context to continue on a different thread
		/// </param>
		public AsyncCommand(Func<Task> execute,
			Func<object, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = true)
		{
			this.execute = execute ??
			               throw new ArgumentNullException(nameof(execute),
				               $"Async Command can't be null : Command Name = {nameof(execute)}");
			this.canExecute = canExecute ?? (_ => true);
			this.onException = onException;
			this.continueOnCapturedContext = continueOnCapturedContext;
		}

		#endregion

		#region Interface Implementations

		/// <summary>
		///     Occurs when changes occur that affect whether or not the command should execute
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => this.weakEventManager.AddEventHandler(value);
			remove => this.weakEventManager.RemoveEventHandler(value);
		}

		#endregion

		#region Constant Fields

		private volatile bool executionInProgress;
		private readonly Func<Task> execute;
		private readonly Func<object, bool> canExecute;
		private readonly Action<Exception> onException;
		private readonly bool continueOnCapturedContext;
		private readonly WeakEventManager.WeakEventManager weakEventManager = new WeakEventManager.WeakEventManager();

		#endregion

		#region Methods

		/// <summary>
		///     Determines whether the command can execute in its current state
		/// </summary>
		/// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
		/// <param name="parameter">
		///     Data used by the command. If the command does not require data to be passed, this object can be
		///     set to null.
		/// </param>
		public bool CanExecute(object parameter)
		{
			if (this.executionInProgress) return false;
			return this.canExecute == null || this.canExecute(parameter);
		}

		/// <summary>
		///     Raises the CanExecuteChanged event.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			this.weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));
		}

		/// <summary>
		///     Executes the Command as a Task
		/// </summary>
		/// <returns>The executed Task</returns>
		public Task ExecuteAsync()
		{
			try
			{
				this.executionInProgress = true;
				RaiseCanExecuteChanged();
				return this.execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			finally
			{
				this.executionInProgress = false;
				RaiseCanExecuteChanged();
			}
		}

		void ICommand.Execute(object parameter)
		{
			if (CanExecute(parameter))
				this.execute().SafeFireAndForget(this.continueOnCapturedContext, this.onException);
		}

		#endregion
	}
}