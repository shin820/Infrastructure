using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Infrastructure.Common.Thread.Lock
{
    /// <summary>
    /// a ReaderWriterLockSlim wrapper
    /// </summary>
    public sealed class ReaderWriterLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _readwriteLock;

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public ReaderWriterLock(ReaderWriterLockSlim readWriteLock, ReaderWriterLockMode lockMode)
            : this(readWriteLock, lockMode, -1)
        {
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public ReaderWriterLock(ReaderWriterLockSlim readWriteLock, ReaderWriterLockMode lockMode, int timeout)
        {
            this._readwriteLock = readWriteLock;
            switch (lockMode)
            {
                case ReaderWriterLockMode.Read:
                    if (!this._readwriteLock.TryEnterReadLock(timeout))
                    {
                        throw new InvalidOperationException("无法获取锁ReaderWriterLockMode.Read");
                    }

                    break;
                case ReaderWriterLockMode.UpgradeableRead:
                    if (!this._readwriteLock.TryEnterUpgradeableReadLock(timeout))
                    {
                        throw new InvalidOperationException("无法获取锁ReaderWriterLockMode.UpgradeableRead");
                    }

                    break;
                case ReaderWriterLockMode.Write:
                    if (!this._readwriteLock.TryEnterWriteLock(timeout))
                    {
                        throw new InvalidOperationException("无法获取锁ReaderWriterLockMode.Write");
                    }

                    break;
                default:
                    throw new InvalidEnumArgumentException("lockMode", (int)lockMode, typeof(ReaderWriterLockMode));
            }
        }

        /// <summary>
        /// 获取写锁.
        /// </summary>
        /// <param name="timeout">超时时间.</param>
        /// <exception cref="System.InvalidOperationException">无法获取锁ReaderWriterLockMode.Write</exception>
        public void AcquireWriteLock(int timeout)
        {
            if (!this._readwriteLock.TryEnterWriteLock(timeout))
            {
                throw new InvalidOperationException("无法获取锁ReaderWriterLockMode.Write");
            }
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (this._readwriteLock.IsReadLockHeld)
            {
                this._readwriteLock.ExitReadLock();
            }

            if (this._readwriteLock.IsUpgradeableReadLockHeld)
            {
                this._readwriteLock.ExitUpgradeableReadLock();
            }

            if (this._readwriteLock.IsWriteLockHeld)
            {
                this._readwriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 降级到读锁.
        /// </summary>
        public void DowngradeToReadLock()
        {
            this._readwriteLock.ExitWriteLock();
        }

        /// <summary>
        /// 升级到写锁.
        /// </summary>
        /// <param name="timeout">超时时间.</param>
        /// <exception cref="System.InvalidOperationException">无法获取锁ReaderWriterLockMode.Write</exception>
        public void UpgradeToWriteLock(int timeout)
        {
            if (!this._readwriteLock.TryEnterWriteLock(timeout))
            {
                throw new InvalidOperationException("无法获取锁ReaderWriterLockMode.Write");
            }
        }
    }
}
