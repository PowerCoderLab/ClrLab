// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System.Threading
{
    /// <summary>
    ///     Can be continued only with providing the data.
    /// </summary>
    internal struct ClrCoroutineContinuation
    {
        public IntPtr StackPointer;

        public IntPtr ResultPointer;

        public ClrCoroutineScheduler Scheduler;
    }
}
