using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Web;

namespace Infrastructure.Common.Log
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class Logger : ILogger
    {
        private static ILogger _fileLogger;

        private readonly ILogRecorder _logRecorder;
        private bool _isIgnoreHttpBody = false;

        static Logger()
        {
            _fileLogger = new Logger(new FileLogRecorder());
        }

        private Logger(ILogRecorder logRecorder)
        {
            this._logRecorder = logRecorder;
        }

        public static ILogger FileLogger
        {
            get
            {
                return _fileLogger;
            }
        }

        public void Exception(Exception exception, string message)
        {
            LogInfo info = this.GetLogInfoFromException(exception);
            info.Message = message;
            this._logRecorder.SaveLog(info);
        }

        public void Exception(Exception exception)
        {
            LogInfo info = this.GetLogInfoFromException(exception);
            this._logRecorder.SaveLog(info);
        }

        public void Info(string message)
        {
            LogInfo info = this.GetLogInfo(message);
            this._logRecorder.SaveLog(info);
        }

        private LogInfo GetLogInfo(string message)
        {
            LogInfo logInfo = new LogInfo
            {
                Time = DateTime.Now,
                Message = message,
            };

            this.PopulateLogInfoFromHttp(logInfo);

            return logInfo;
        }

        private LogInfo GetLogInfoFromException(Exception exception)
        {
            LogInfo logInfo = new LogInfo
            {
                Time = DateTime.Now,
                Message = exception.Message,
            };
            this.PopulateLogInfoFromException(logInfo, exception);
            this.PopulateLogInfoFromHttp(logInfo);

            return logInfo;
        }

        private void PopulateLogInfoFromException(LogInfo logInfo, Exception exception)
        {
            logInfo.ExceptionType = exception.GetBaseException().GetType().ToString();
            logInfo.Exception = exception.ToString();
        }

        private void PopulateLogInfoFromHttp(LogInfo logInfo)
        {
            HttpContext current = HttpContext.Current;
            if (current == null)
            {
                return;
            }

            logInfo.Url = current.Request.RawUrl;
            logInfo.RequestType = current.Request.RequestType;
            logInfo.ContentEncoding = current.Request.ContentEncoding.ToString();

            if (current.Request.UrlReferrer != null)
            {
                logInfo.UrlReferrer = current.Request.UrlReferrer.ToString();
            }

            if (current.Request.Browser != null)
            {
                logInfo.Browser = current.Request.Browser.Type;
            }

            if (current.Request.IsAuthenticated)
            {
                logInfo.UserName = current.User.Identity.Name;
            }

            // 填充post提交的数据
            this.PopulateHttpPostData(logInfo);

            // 填充cookie数据
            PopulateCookies(logInfo);

            // 填充会话数据
            PopulateSessions(logInfo);
        }

        private static void PopulateSessions(LogInfo logInfo)
        {
            HttpContext current = HttpContext.Current;
            if (current.Session == null)
            {
                return;
            }

            foreach (string sessionKey in current.Session.Keys)
            {
                object sessionValue = current.Session[sessionKey];
                logInfo.Sessions.Add(new NameValue
                {
                    Name = sessionKey,
                    Value = sessionValue == null ? null : sessionValue.ToString()
                });
            }
        }

        private static void PopulateCookies(LogInfo logInfo)
        {
            HttpContext current = HttpContext.Current;
            if (current.Request.Cookies.Count <= 0)
            {
                return;
            }

            foreach (string cookieName in current.Request.Cookies.AllKeys)
            {
                HttpCookie cookie = current.Request.Cookies[cookieName];
                logInfo.Cookies.Add(new NameValue { Name = cookie.Name, Value = cookie.Value });
            }
        }

        private void PopulateHttpPostData(LogInfo logInfo)
        {
            HttpContext current = HttpContext.Current;
            if (this._isIgnoreHttpBody || current.Request.RequestType != "POST")
            {
                return;
            }

            if (current.Request.Files.Count == 0)
            {
                current.Request.InputStream.Position = 0;
                StreamReader reader = new StreamReader(current.Request.InputStream, current.Request.ContentEncoding);
                logInfo.PostData = reader.ReadToEnd();
                reader.Close();
                current.Request.InputStream.Position = 0;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string name in current.Request.Form.AllKeys)
                {
                    string[] values = current.Request.Form.GetValues(name);
                    if (values == null)
                    {
                        continue;
                    }

                    foreach (string value in values)
                    {
                        sb.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(value));
                    }
                }

                if (sb.Length > 0)
                {
                    sb.Remove(0, 1);
                    logInfo.PostData = sb.ToString();
                }
            }
        }
    }
}
