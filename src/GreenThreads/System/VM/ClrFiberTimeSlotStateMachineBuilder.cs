// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System.VM
{
    using System.Runtime.CompilerServices;

    public sealed class ClrFiberTimeSlotStateMachineBuilder
    {
        public ClrFiberTimeSlotStateMachineBuilder()
            => Console.WriteLine(".ctor");

        public TaskLike Task => default(TaskLike);

        public static ClrFiberTimeSlotStateMachineBuilder Create()
            => new ClrFiberTimeSlotStateMachineBuilder();

        public void SetResult() => Console.WriteLine("SetResult");

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("Start");
            stateMachine.MoveNext();
        }

        // AwaitOnCompleted, AwaitUnsafeOnCompleted, SetException 
        // and SetStateMachine are empty
    }
}
