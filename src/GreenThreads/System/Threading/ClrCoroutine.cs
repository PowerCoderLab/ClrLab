// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System.Threading
{
    using System.VM;

    /// <summary>
    ///     Green thread implemented in CLR virtual machine.
    /// </summary>
    public class ClrCoroutine: ClrFiber
    {
        private readonly ClrCoroutineStack _stack;

        private ClrCoroutineContinuation _continuation;

        public ClrCor

        /// <summary>
        ///     Gets current coroutine.
        /// </summary>
        public static ClrCoroutine? Current { get; }

        /// <summary>
        ///  Compiler intrisic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="awaitable"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static ref T Await<T>(ClrCompletionSource<T> awaitable, ClrCoroutineScheduler scheduler = default)
        {
            // Logic of woring on 
            var completionSource = (ClrCompletionSource<T>)awaitable;
        }
    }
}
