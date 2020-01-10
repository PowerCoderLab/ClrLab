// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System.VM
{
    using System.Collections.Concurrent;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///     C# visible, but mostly implemented at CLR VM level in C++.
    /// </summary>
    /// <remarks>
    /// Let's imagine that switch from ClrFiber stack to "Native" stack is also cheap.
    /// Currently I found one major problem - when ClrFiber calls to native function, the main stack should be switched. It would be better to implement Fiber-Stack on a different registry set.
    /// When fiber exits, it should return control to the ??? what fiber ?.
    /// </remarks>
    public abstract class ClrFiber
    {
        /// <summary>
        /// We should maintain all fibers collection to perform GC on its stacks.
        /// </summary>
        internal static readonly ConcurrentBag<ClrFiber> _allFibers = new ConcurrentBag<ClrFiber>();

        [ThreadStatic] internal static ClrFiber? _current;

        /// <summary>
        ///     Dynamically allocated stack on the heap. C++ implemented.
        /// </summary>
        internal ClrFiberDynamicStack _stack;

        internal ClrFiberSuspendState _suspendState;

        internal ClrFiber()
        {
            // Do some initialization.
            // Fibers are always created in the suspended state.
            _allFibers.Add(this);
        }

        private bool IsSuspended { get; }

        /// <summary>
        ///     The fiber green-thread.
        /// </summary>
        public abstract void Proc();

        /// <summary>
        ///     IL instruction. Transfer OS thread flow to a different fiber.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [CpuRegistryBarrier] // JIT should not rely on Cpu registers after this IL instruction.
        public void TransferFlowOpcode(ClrFiber nextFiber)
        {
            // Generated assembly instructions pseudo code:

            // Transferring flow to our self.

            // What should be guarantied before this instruction:
            if (!ReferenceEquals(_current, nextFiber) || !nextFiber.IsSuspended)
            {
                throw new UnpredictableBehavior();
            }

            // Saving resume point in the current fiber.
            _suspendState.StackPointer = Cpu.StackPointer; // On x64 machine it's an RSP register.
            _suspendState.StackBasePointer = Cpu.StackBasePointer; // On x64 machine it's an RBP register.
            
            _suspendState.ContinuationInstructionAddress =
                NativeCodeConstants.InstructionOffset("resume"); // JIT generated code constant

            // Currently we are in the critical state, setting-up stack state for the new fiber.
            _current = nextFiber;
            nextFiber._suspendState.RestoreBaseCpuRegisters();
            Cpu.StackPointer = nextFiber._suspendState.StackPointer;
            Cpu.StackBasePointer = nextFiber._suspendState.StackBasePointer;
            Cpu.Jump(nextFiber._suspendState.ContinuationInstructionAddress);

            // Critical state ended.
            // After this call we appear on the "resume:" label of the !!!nextFiber!!!, not this.
            // ------------------------------------------------------------------------------- Preemptive switch

            // RESUME from other ClrFiber
            // This instruction already performed it's work
            // The "Resume" label it's a next instruction location
            resume: ;

            // Here there is no any assumption about a state of CPU registers
        }
    }
}
