using System;
using System.Diagnostics.CodeAnalysis;
using Infrastructure.Common.AppDomain;
using NUnit.Framework;

namespace Infrastructure.UnitTest.AppDomain
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class AppDomainContextTest
    {
        [Test]
        public void ShouldGetProxyFromTempAppDomain()
        {
            using (var context = new AppDomainContext<Proxy>())
            {
                Assert.AreEqual("TemporaryDomain", context.Proxy.GetAppDomainName());
            }
        }

        [Test]
        public void ShouldCreateDomainWithSpecifiedName()
        {
            using (var context = new AppDomainContext<Proxy>("temp"))
            {
                Assert.AreEqual("temp", context.Proxy.GetAppDomainName());
            }
        }

        public class Proxy : MarshalByRefObject
        {
            public string GetAppDomainName()
            {
                return System.AppDomain.CurrentDomain.FriendlyName;
            }
        }
    }
}
