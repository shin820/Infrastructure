using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Common.Thread.Lock
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public enum ReaderWriterLockMode
    {
        /// <summary>
        /// The read
        /// </summary>
        Read,

        /// <summary>
        /// The upgradeable read
        /// </summary>
        UpgradeableRead,

        /// <summary>
        /// The write
        /// </summary>
        Write
    }
}
