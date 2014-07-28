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
            AuthCookieTicket ticket = new AuthCookieTicket("liux");

            var cookie = this.MakeTicketCookie(ticket);
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            HttpCookieCollection cookies = new HttpCookieCollection { cookie };
            request.SetupGet(t => t.Cookies).Returns(cookies);

            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            context.SetupGet(t => t.Request).Returns(request.Object);

            var ticketFromCookie = AuthCookieTicket.GetTicketCookie(context.Object);

            Assert.IsNotNull(ticketFromCookie);
            Assert.AreEqual(1, ticketFromCookie.Count);
            Assert.AreEqual("liux", ticketFromCookie.User);
        }

        [Test]
        public void ShouldGetTicketDataFromCookie()
        {
            AuthCookieTicket ticket = new AuthCookieTicket("liux");
            ticket.Add("p1", 123);
            Guid guid = Guid.NewGuid();
            ticket.Add("p2", guid);

            var cookie = this.MakeTicketCookie(ticket);

            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            HttpCookieCollection cookies = new HttpCookieCollection { cookie };
            request.SetupGet(t => t.Cookies).Returns(cookies);

            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            context.SetupGet(t => t.Request).Returns(request.Object);

            var ticketFromCookie = AuthCookieTicket.GetTicketCookie(context.Object);

            Assert.IsNotNull(ticketFromCookie);
            Assert.AreEqual(3, ticketFromCookie.Count);
            Assert.AreEqual("liux", ticketFromCookie["User"]);
            Assert.AreEqual(123, ticketFromCookie["p1"]);
            Assert.AreEqual(guid, ticketFromCookie["p2"]);
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

        private HttpCookie MakeTicketCookie(AuthCookieTicket ticket)
        {
            HttpCookie cookie = null;
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpResponseBase> response = new Mock<HttpResponseBase>();
            context.SetupGet(t => t.Response).Returns(response.Object);
            response.Setup(t => t.SetCookie(It.IsAny<HttpCookie>())).Callback<HttpCookie>(t => cookie = t);
            ticket.SetTicketCookie(context.Object);

            return cookie;
        }
    }
}
