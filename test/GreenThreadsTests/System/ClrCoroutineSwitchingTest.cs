// Copyright (c) PowerCoderLab community. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace System
{
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.VM;
    using Xunit;

    public class ClrCoroutineSwitchingTest
    {
        [Fact]
        public unsafe async Task SimpleTest()
        {
            // Should be wrapped by something like ValueTask
            var clrAwaitable = AsyncPrimitives.Delay(TimeSpan.FromSeconds(2));

            // This call:
            var waitCompleted = ClrCoroutine.Await(clrAwaitable);

            // is down-compile to IL code:
            unsafe
            {
                var currentCoroutine = ClrCoroutine.Current;
                if (ClrCoroutine.Current == null)
                {
                    throw new InvalidOperationException();

                }
            }

            resume0:

            return;
        }
    }
}
