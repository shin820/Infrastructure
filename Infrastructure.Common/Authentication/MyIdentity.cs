using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Authentication
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class MyIdentity : DataCollection<string, object, StringComparer>, IIdentity
    {
        private MyIdentity(IIdentity identity, AuthCookieTicket authTicket)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            this.Name = identity.Name;
            this.AuthenticationType = identity.AuthenticationType;
            this.IsAuthenticated = identity.IsAuthenticated;

            this.Initalize(authTicket);
        }

        private void Initalize(AuthCookieTicket authTicket)
        {
            foreach (var entry in authTicket)
            {
                this.Add(entry.Key, entry.Value);
            }
        }

        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }
    }
}
