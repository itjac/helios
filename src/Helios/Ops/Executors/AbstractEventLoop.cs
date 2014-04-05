using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helios.Channels;
using Helios.Concurrency;

namespace Helios.Ops.Executors
{
    /// <summary>
    /// Abstract base class for working with <see cref="IEventLoop"/> instances inside a <see cref="IChannel"/>
    /// </summary>
    public abstract class AbstractEventLoop : IEventLoop
    {
        protected AbstractEventLoop(IFiber scheduler)
        {
            Scheduler = scheduler;
        }

        protected IFiber Scheduler { get; private set; }

        public bool AcceptingJobs { get { return Scheduler.Executor.AcceptingJobs; } }

        public void Execute(Action op)
        {
            Scheduler.Add(op);
        }

        public Task ExecuteAsync(Action op)
        {
            return Scheduler.Executor.ExecuteAsync(op);
        }

        public void Execute(IList<Action> op)
        {
            foreach (var o in op)
            {
                Scheduler.Add(o);
            }
        }

        public Task ExecuteAsync(IList<Action> op)
        {
            return Scheduler.Executor.ExecuteAsync(op);
        }

        public void Execute(IList<Action> ops, Action<IEnumerable<Action>> remainingOps)
        {
            Scheduler.Executor.Execute(ops, remainingOps);
        }

        public Task ExecuteAsync(IList<Action> ops, Action<IEnumerable<Action>> remainingOps)
        {
            return Scheduler.Executor.ExecuteAsync(ops, remainingOps);
        }

        public void Shutdown()
        {
            Scheduler.Stop();
        }

        public void Shutdown(TimeSpan gracePeriod)
        {
            Scheduler.Shutdown(gracePeriod);
        }

        public Task GracefulShutdown(TimeSpan gracePeriod)
        {
            return Scheduler.GracefulShutdown(gracePeriod);
        }

        public bool WasDisposed { get; private set; }

        /// <summary>
        /// Returns a new <see cref="IEventLoop"/> that can be chained after this one
        /// </summary>
        public abstract IExecutor Next();

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool isDisposing)
        {
            if (isDisposing && !WasDisposed)
            {
                WasDisposed = true;
                Scheduler.Dispose();
            }
        }

        #endregion
    }
}