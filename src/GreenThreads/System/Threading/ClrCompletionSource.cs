// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System.Threading
{
    public class ClrCompletionSource<T>: IClrAwaitable<T>
    {
        public ClrCoroutineContinuation Continuation { get; }

        public void SetResult(T result)
        {
            // Pseudo code
            // 
        }

        public void SetError(Exception error)
        {

        }
    }

    public class Promise
    {

    }
}
