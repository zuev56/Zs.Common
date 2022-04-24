using System.Diagnostics;
using Xunit;
using Zs.Common.Extensions;

namespace Zs.Common.UnitTests
{
    public class LongExtensionTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(1020)]
        [InlineData(1025)]
        [InlineData(2000000)]
        [InlineData(3000000000)]
        [InlineData(4000000000000)]
        public void Test1(long byteAmount)
        {
            // TODO: ...
            Trace.WriteLine(byteAmount.BytesAmountToSizeString());
        }
    }
}