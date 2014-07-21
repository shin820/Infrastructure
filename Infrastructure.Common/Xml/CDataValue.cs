using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

//// 此处代码来源于博客【在.net中读写config文件的各种方法】的示例代码
//// http://www.cnblogs.com/fish-li/archive/2011/12/18/2292037.html

namespace Infrastructure.Common.Xml
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class CDataValue : IXmlSerializable
    {
        private string _value;

        public CDataValue()
        {
        }

        public CDataValue(string value)
        {
            this._value = value;
        }

        public string Value
        {
            get { return this._value; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            this._value = reader.ReadElementContentAsString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteCData(this._value);
        }

        public override string ToString()
        {
            return this._value;
        }

        public static implicit operator CDataValue(string text)
        {
            return new CDataValue(text);
        }

        public static implicit operator string(CDataValue text)
        {
            return text.ToString();
        }
    }
}
