using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Log
{
    public interface ILogger
    {
        void Exception(Exception exception, string message);
        void Exception(Exception exception);
        void Info(string message);
    }
}
