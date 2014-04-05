﻿using System;
using System.Threading.Tasks;
using Helios.Ops;

namespace Helios.Concurrency
{
    /// <summary>
    /// Interface for lightweight threading and execution
    /// </summary>
    public interface IFiber : IDisposable
    {
        /// <summary>
        /// The internal executor used to execute tasks
        /// </summary>
        IExecutor Executor { get; }

        bool WasDisposed { get; }

        void Add(Action op);

        /// <summary>
        /// Shuts down this Fiber within the allotted timeframe
        /// </summary>
        /// <param name="gracePeriod">The amount of time given for currently executing tasks to complete</param>
        void Shutdown(TimeSpan gracePeriod);

        /// <summary>
        /// Shuts down this fiber within the alloted timeframe and provides a task that can be waited on during the interim
        /// </summary>
        /// <param name="gracePeriod">The amount of time given for currently executing tasks to complete</param>
        Task GracefulShutdown(TimeSpan gracePeriod);

        /// <summary>
        /// Performs a hard-stop on the Fiber - no more actions can be executed
        /// </summary>
        void Stop();

        void Dispose(bool isDisposing);
    }
}
