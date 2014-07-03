using System;

namespace Infrastructure.Common.AppDomain
{
    /// <summary>
    /// 用来帮助获取临时应用程序域中的对象
    /// </summary>
    /// <typeparam name="TProxy">The type of the proxy.</typeparam>
    public sealed class AppDomainContext<TProxy> : IDisposable
        where TProxy : class, new()
    {
        private System.AppDomain _domain;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDomainContext{TProxy}"/> class.
        /// </summary>
        public AppDomainContext()
            : this("TemporaryDomain")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDomainContext{TProxy}"/> class.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <exception cref="System.ArgumentNullException">domainName</exception>
        public AppDomainContext(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
            {
                throw new ArgumentNullException("domainName");
            }

            // 创建应用程序域
            var setup = new AppDomainSetup
            {
                ApplicationBase = System.AppDomain.CurrentDomain.BaseDirectory
            };
            this._domain = System.AppDomain.CreateDomain(domainName, null, setup);

            string assemblyLocation = this.GetAssemblyLocation(typeof(TProxy).Assembly.GetName().Name);

            // 获取临时应用程序域中的代理对象
            this.Proxy = (TProxy)this._domain.CreateInstanceFromAndUnwrap(assemblyLocation, typeof(TProxy).FullName);

            // 附加程序集解析器
            string resolverAssemblyLocation =
                this.GetAssemblyLocation(typeof(AssemblyResolver).Assembly.GetName().Name);
            var assemblyResolver = (AssemblyResolver)this._domain.CreateInstanceFromAndUnwrap(resolverAssemblyLocation, typeof(AssemblyResolver).FullName);
            assemblyResolver.AttachResolver(this._domain);
        }

        private string GetAssemblyLocation(string assemblyName)
        {
            // 先通过MyAssemblyResolver获取程序集位置，保证从正确的位置获取程序集文件
            string probeLocation = AssemblyResolver.GetAssemblyLocation(assemblyName + ".dll");
            return string.IsNullOrEmpty(probeLocation) ? typeof(TProxy).Assembly.Location : probeLocation;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AppDomainContext{TProxy}"/> class.
        /// </summary>
        ~AppDomainContext()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <value>
        /// The proxy.
        /// </value>
        public TProxy Proxy { get; private set; }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (null != this._domain)
            {
                System.AppDomain.Unload(this._domain);
                this._domain = null;
                this.Proxy = null;
            }

            if (disposing)
            {
                this._disposed = true;
            }
        }
    }
}
