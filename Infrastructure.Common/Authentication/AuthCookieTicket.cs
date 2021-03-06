﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml.Serialization;
using Infrastructure.Common.Serialization;

namespace Infrastructure.Common.Authentication
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    [Serializable]
    public class AuthCookieTicket : DataCollection<string, object, StringComparer>
    {
        private const string AuthCookieName = "AuthCookieName";
        private const string AuthCookieTicketName = "AuthCookieTicketName";

        private static ConcurrentDictionary<string, byte[]> secretKeys = new ConcurrentDictionary<string, byte[]>();

        public byte[] Signature { get; set; }
        public string Token { get; set; }
        public string User { get; set; }

        public AuthCookieTicket(string user)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            // 创建随机Token
            this.Token = Guid.NewGuid().ToString();
            this.User = user;
            this.Add("User", user);
            this.Signature = this.ComputeHash();
        }

        public void SetTicketCookie(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var cookie = new HttpCookie(AuthCookieName)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath
            };
            cookie[AuthCookieTicketName] = this.Serialize();
            context.Response.SetCookie(cookie);
            context.Response.AddHeader("P3P", "CP=\"CAO PSA OUR\"");
        }

        public static void ClearTicketCookie(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpCookie cookie = context.Request.Cookies[AuthCookieName];
            if (cookie != null)
            {
                cookie = new HttpCookie(AuthCookieName);
                cookie.Expires = DateTime.Now.AddDays(-1d);
                context.Response.Cookies.Add(cookie);
            }
        }

        public static AuthCookieTicket GetTicketCookie(HttpContextBase context)
        {
            AuthCookieTicket ticket = null;
            var cookie = context.Request.Cookies[AuthCookieName];
            if (cookie != null && cookie.Values[AuthCookieTicketName] != null)
            {
                ticket = Deserialize(cookie.Values[AuthCookieTicketName]);
            }

            if (ticket != null && !ticket.IsValid())
            {
                throw new SecurityException("AuthCookieTicket票据的内容被篡改.");
            }

            return ticket;
        }


        private bool IsValid()
        {
            // 校验票据自身的内容是否被篡改
            bool isValid;
            var hmac = new HMACSHA1(this.GetSecretKey());

            try
            {
                byte[] buffer = hmac.ComputeHash(Encoding.Unicode.GetBytes(this.ToString()));
                isValid = this.Signature.SequenceEqual(buffer);
            }
            finally
            {
                Array.Clear(hmac.Key, 0, hmac.Key.Length);
                hmac.Clear();
            }

            return isValid;
        }

        private string Serialize()
        {
            string ticketString = Convert.ToBase64String(BinarySerializationHelper.Serialize(this));

            return ticketString;
        }

        private static AuthCookieTicket Deserialize(string ticketString)
        {
            AuthCookieTicket ticket = null;

            try
            {
                ticket = BinarySerializationHelper.Deserialize<AuthCookieTicket>(Convert.FromBase64String(ticketString));
            }
            catch (FormatException e)
            {
                // 传入的ticketString不是有效的base64字符串
                ticket = null;
            }
            catch (InvalidOperationException e)
            {
                // Deserialize失败
                ticket = null;
            }

            return ticket;
        }

        private byte[] ComputeHash()
        {
            byte[] buffer;
            var key = this.GetSecretKey();
            var hmac = new HMACSHA1(key);

            try
            {
                buffer = hmac.ComputeHash(Encoding.Unicode.GetBytes(this.ToString()));
            }
            finally
            {
                Array.Clear(hmac.Key, 0, hmac.Key.Length);
                hmac.Clear();
            }

            return buffer;
        }

        private byte[] GetSecretKey()
        {
            byte[] key = new HMACSHA1().Key;
            return secretKeys.GetOrAdd(this.ToString(), key);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}-{1}", this.Token, this.User);
        }
    }
}
