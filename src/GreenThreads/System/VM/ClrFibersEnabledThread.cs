// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System.VM
{
    using System.Runtime.InteropServices;
    using System.Threading;

    public class ClrFibersEnabledThread
    {
        private jmpbuf _beforeFiberStack;

        public ClrFibersEnabledThread()
        {
            ThreadExitFiber = new ThreadExitClrFiber(this);
        }

        public Thread OSThread { get; }

        /// <summary>
        ///     Address of Proc() method to be called from unmanagedthread.
        /// </summary>
        public IntPtr EntryPointAddress { get; }

        /// <summary>
        ///     This fiber is becomes accessible when
        /// </summary>
        public ClrFiber ThreadExitFiber { get; }

        private static bool IsRegularManagedThread()
        {
            return false;
        }

        /// <summary>
        ///     CLR controlled fiber-enabled execution. To be called from unmanaged environment.
        /// </summary>
        /// <param name="fiberGcHandleIntPtr">The handle to the special fibers that are part of schedulers.</param>
        public void Proc(IntPtr fiberGcHandleIntPtr)
        {
            var fiberGcHandle = GCHandle.FromIntPtr(fiberGcHandleIntPtr);

            if (IsRegularManagedThread())
            {
                throw new NotSupportedException(
                    "Thread that at least once called regular C# thread cannot be used as Fiber-Enabled thread.");
            }

            // Initializing System.Threading.Thread in a special way, so CLR VM will handle different fiber-oriented behavior.
            InitAsFiberEnabledThread();

            // Moving to hard-hack of the stack.
            if (!Cpp.setjmp(ref _beforeFiberStack))
            {
                // Stop using C++
                // Working on assembly level

                // switching C++ OS stack to ClrFiber managed stack:
                var fiber = (ClrFiber)fiberGcHandle.Target;

                // Very special stack condition
                var returnAddress = NativeCodeConstants.InstructionOffset("exitThread");
                Cpu.StackBasePointer =
                    Cpu.StackBasePointer +
                    0x10; // Hacking stack base pointer, now it's tuned to the address of the "returnAddress" variable.

                // After this hack the "ret" assembly command will jump to "exitThread".
                ;

                // What happens if we call Native method inside the "Managed Stack"
                // Making thread exit fiber as current.
                ThreadExitFiber._suspendState.StackPointer = Cpu.StackPointer;
                ThreadExitFiber._suspendState.StackBasePointer = Cpu.StackBasePointer; // This is native stack pointer
                ThreadExitFiber._suspendState.ContinuationInstructionAddress =
                    NativeCodeConstants.InstructionOffset("exitThread");
                ClrFiber._current = ThreadExitFiber;

                // Do the same code as this IL instruction produces.
                // Whenever ClrFiber executes "ret" assembly instruction will return to "exitThread".
                ThreadExitFiber.TransferFlowOpcode(
                    fiber); // After this call, current thread is fully controlled by the preemptive multitasking.

                // When preemptive multitasking switch control to the "ThreadExitFiber", thread flow will jump to this location:
                exitThread: ;
            }

            // Cpp now works again.
            ReleaseFiberEnabledThread();
        }

        private void InitAsFiberEnabledThread()
        {
        }

        private void ReleaseFiberEnabledThread()
        {
        }

        private class ThreadExitClrFiber : ClrFiber
        {
            private readonly ClrFibersEnabledThread _owner;

            public ThreadExitClrFiber(ClrFibersEnabledThread owner)
            {
                _owner = owner;
                // Initializing small stack. Fiber will do nothing.
                _stack = new ClrFiberDynamicStack();
            }

            public override void Proc()
            {
                // Do nothing
            }
        }
    }
}
