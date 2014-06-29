using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Common.Thread.Lock;
using NUnit.Framework;

namespace Infrastructure.UnitTest.Common.Thread.Lock
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here."), TestFixture]
    public class ReaderWriterLockTest
    {
        private ReaderWriterLockSlim _readerWriterLockSlim;

        [SetUp]
        public void SetUp()
        {
            this._readerWriterLockSlim = new ReaderWriterLockSlim();
        }

        [Test]
        public void ShouldEnterReaderLock()
        {
            using (new Infrastructure.Common.Thread.Lock.ReaderWriterLock(this._readerWriterLockSlim, ReaderWriterLockMode.Read))
            {
                Assert.IsTrue(this._readerWriterLockSlim.IsReadLockHeld);
            }

            Assert.IsFalse(this._readerWriterLockSlim.IsReadLockHeld);
        }

        [Test]
        public void ShouldEnterWriterLock()
        {
            using (new Infrastructure.Common.Thread.Lock.ReaderWriterLock(this._readerWriterLockSlim, ReaderWriterLockMode.Write))
            {
                Assert.IsTrue(this._readerWriterLockSlim.IsWriteLockHeld);
            }

            Assert.IsFalse(this._readerWriterLockSlim.IsReadLockHeld);
        }

        [Test]
        public void ShouldEnterUpgradeableReaderLock()
        {
            using (var lockObj = new Infrastructure.Common.Thread.Lock.ReaderWriterLock(this._readerWriterLockSlim, ReaderWriterLockMode.UpgradeableRead))
            {
                Assert.IsTrue(this._readerWriterLockSlim.IsUpgradeableReadLockHeld);
            }

            Assert.IsFalse(this._readerWriterLockSlim.IsUpgradeableReadLockHeld);
        }

        [Test]
        public void ShouldUpgradeToWriteLock()
        {
            using (var lockObj = new Infrastructure.Common.Thread.Lock.ReaderWriterLock(this._readerWriterLockSlim, ReaderWriterLockMode.UpgradeableRead))
            {
                lockObj.UpgradeToWriteLock(100);
                Assert.IsTrue(this._readerWriterLockSlim.IsWriteLockHeld);
            }
        }

        [Test]
        public void ShouldDowngradeToReadLock()
        {
            using (var lockObj = new Infrastructure.Common.Thread.Lock.ReaderWriterLock(this._readerWriterLockSlim, ReaderWriterLockMode.UpgradeableRead))
            {
                lockObj.UpgradeToWriteLock(100);
                lockObj.DowngradeToReadLock();
                Assert.IsTrue(this._readerWriterLockSlim.IsUpgradeableReadLockHeld);
            }
        }
    }
}
