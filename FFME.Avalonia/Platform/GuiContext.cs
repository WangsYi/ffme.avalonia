using Avalonia;
using Avalonia.Threading;

namespace FFME.Platform
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using static Application;

    /// <summary>
    /// Provides properties and methods for the
    /// WPF or Windows Forms GUI Threading context.
    /// </summary>
    internal sealed class GuiContext : IGuiContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuiContext"/> class.
        /// </summary>
        public GuiContext()
        {
            Thread = Thread.CurrentThread;
            ThreadContext = SynchronizationContext.Current;

            // Try to extract the dispatcher from the current thread
            try { GuiDispatcher = Dispatcher.UIThread; }
            catch { /* Ignore error as app might not be available or context is not WPF */ }


            IsValid = GuiDispatcher == null;
        }

        /// <summary>
        /// Gets the thread on which this context was created.
        /// </summary>
        public Thread Thread { get; }

        /// <summary>
        /// Returns true if this context is valid.
        /// </summary>
        internal bool IsValid { get; }

        /// <summary>
        /// Gets the synchronization context.
        /// </summary>
        internal SynchronizationContext ThreadContext { get; }

        /// <summary>
        /// Gets the GUI dispatcher. Only valid for WPF contexts.
        /// </summary>
        internal Dispatcher GuiDispatcher { get; }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConfiguredTaskAwaitable InvokeAsync(Action callback) =>
            InvokeAsyncInternal(DispatcherPriority.DataBind, callback, null).ConfigureAwait(true);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnqueueInvoke(Action callback) => InvokeAsync(callback);

        /// <summary>
        /// Invokes a task on the GUI thread.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The awaitable task.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task InvokeAsyncInternal(DispatcherPriority priority, Delegate callback, params object[] arguments)
        {
            if (Thread == Thread.CurrentThread)
            {
                callback.DynamicInvoke(arguments);
                return;
            }

            try
            {
                // We try here because we'd like to catch cancellations and ignore then
                await GuiDispatcher.InvokeAsync(() => { callback.DynamicInvoke(arguments); }, priority);
                return;
            }
            catch (OperationCanceledException)
            {
                // Ignore cancellation
                Debug.WriteLine($"FFME {nameof(GuiContext)}.{nameof(InvokeAsyncInternal)}: Operation was cancelled");
            }
        }
    }
}
