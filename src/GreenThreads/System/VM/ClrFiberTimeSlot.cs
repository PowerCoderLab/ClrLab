// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System.VM
{
    using System.Runtime.CompilerServices;

    [AsyncMethodBuilder(typeof(ClrFiberTimeSlotStateMachineBuilder))]
    public class ClrFiberTimeSlot
    {
    }

    //public struct TaskLike
    //{
    //    public TaskLikeAwaiter GetAwaiter() => default(TaskLikeAwaiter);
    //}
}
