// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System.VM
{
    public struct ClrFiberSuspendState
    {
        public IntPtr StackPointer;
        public IntPtr StackBasePointer;
        public IntPtr ContinuationInstructionAddress;

        public void CaptureBaseCpuRegisters()
        {

        }

        public void RestoreBaseCpuRegisters()
        {

        }
    }
}
