using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common.Xml;

namespace Infrastructure.Common.Log
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class FileLogRecorder : ILogRecorder
    {
        private const string SeparateLine = "<!--##############f2781505-f286-4c9d-b73d-fa78eae22723$$$$$$$$$$$$$-->";
        private string _folder;

        public FileLogRecorder()
        {
            string folder = ConfigurationManager.AppSettings["Log:LogFileFolder"];
            if (string.IsNullOrEmpty(folder))
            {
                folder = "ExceptionLog";
            }

            folder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, folder);
            try
            {
                if (Directory.Exists(folder) == false)
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch (Exception ex)
            {
                throw new DirectoryNotFoundException(string.Format("配置错误,路径{0}不存在，且无法创建。", folder), ex);
            }

            this._folder = folder;
        }

        public void SaveLog(LogInfo logInfo)
        {
            string filePath = string.Format("{0}\\{1}.log", this._folder, DateTime.Now.ToString("yyyy-MM-dd"));
            string xml = XmlSerializationHelper.XmlSerialize(logInfo, Encoding.UTF8);
            SaveToFile(xml, filePath);
        }

        /// <summary>
        /// 将一段文本追加到指定的文件结尾，并添加分隔行。
        /// </summary>
        /// <param name="text">要写入的文本</param>
        /// <param name="filePath">要写入的文件路径</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void SaveToFile(string text, string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }

            string contents = text + "\r\n\r\n" + SeparateLine + "\r\n\r\n";
            File.AppendAllText(filePath, contents, Encoding.UTF8);
        }
    }
}
