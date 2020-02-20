using System.Linq;
using System.Threading.Tasks;
using FreeboxOS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.UWP
{
    [TestClass]
    public class UnitTest1
    {
        private ITVApi GetTVApi()
        {
            return new ServiceCollection().AddFreeboxOSAPI().BuildServiceProvider().GetService<ITVApi>();
        }

        [TestMethod]
        public async Task IsEnabledAsync()
        {
            Assert.AreEqual(await GetTVApi().IsEnabledAsync(), true);
        }

        [TestMethod]
        public async Task GetChannelsAsync()
        {
            Assert.IsTrue((await GetTVApi().GetChannelsAsync()).Count() > 0);
        }
    }
}
