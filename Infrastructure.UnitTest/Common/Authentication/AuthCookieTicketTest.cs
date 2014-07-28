using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Infrastructure.Common.Authentication;
using Moq;
using NUnit.Framework;

namespace Infrastructure.UnitTest.Common.Authentication
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here."), TestFixture]
    public class AuthCookieTicketTest
    {
        [Test]
        public void ShouldSetCookie()
        {
            HttpCookie cookie = null;
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpResponseBase> response = new Mock<HttpResponseBase>();
            context.SetupGet(t => t.Response).Returns(response.Object);
            response.Setup(t => t.SetCookie(It.IsAny<HttpCookie>())).Callback<HttpCookie>(t => cookie = t);

            AuthCookieTicket ticket = new AuthCookieTicket("testUser");
            ticket.SetTicketCookie(context.Object);

            Assert.IsNotNull(cookie);
            Assert.IsNotNull(cookie["AuthCookieTicketName"]);
            Assert.AreEqual("AuthCookieName", cookie.Name);
            response.Verify(t => t.AddHeader("P3P", "CP=\"CAO PSA OUR\""));
        }

        [Test]
        public void ShouldGetTicketFromCookie()
        {
            var cookie = this.MakeTicketCookie("liux");
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            HttpCookieCollection cookies = new HttpCookieCollection { cookie };
            request.SetupGet(t => t.Cookies).Returns(cookies);
            context.SetupGet(t => t.Request).Returns(request.Object);

            var ticket = AuthCookieTicket.GetTicketCookie(context.Object);
            Assert.IsNotNull(ticket);
            Assert.AreEqual("liux", ticket.User);
        }

        [Test]
        public void ShouldClearCookie()
        {
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            request.SetupGet(t => t.Cookies).Returns(new HttpCookieCollection { new HttpCookie("AuthCookieName") });

            Mock<HttpResponseBase> response = new Mock<HttpResponseBase>();
            HttpCookieCollection cookies = new HttpCookieCollection();
            response.SetupGet(t => t.Cookies).Returns(cookies);

            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            context.SetupGet(t => t.Request).Returns(request.Object);
            context.SetupGet(t => t.Response).Returns(response.Object);

            AuthCookieTicket.ClearTicketCookie(context.Object);
            Assert.AreEqual(1, cookies.Count);
        }

        private HttpCookie MakeTicketCookie(string user)
        {
            HttpCookie cookie = null;
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpResponseBase> response = new Mock<HttpResponseBase>();
            context.SetupGet(t => t.Response).Returns(response.Object);
            response.Setup(t => t.SetCookie(It.IsAny<HttpCookie>())).Callback<HttpCookie>(t => cookie = t);

            AuthCookieTicket ticket = new AuthCookieTicket(user);
            ticket.SetTicketCookie(context.Object);

            return cookie;
        }
    }
}
