using System;
using SystemX.Services;
using System.Transactions;

namespace CoyoEden.DataAccess.SubSonicX
{
    /// <summary>
    /// Transaction context
    /// </summary>
    public sealed class TransactionContext:IContext
    {
        #region member variables
        /// <summary>
        /// the transaction scope of current context
        /// </summary>
        public TransactionScope Trans { get; set; }
        #endregion

        #region .ctor
        public TransactionContext() {
            Trans = new TransactionScope();
        }
        #endregion

        #region IContext Members

        public void Complete()
        {
            if (Trans != null)
            {
                Trans.Complete();
            }
        }

        public void Rollback()
        {
            //do nothing
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (Trans != null) {
                Trans.Dispose();
            }
        }

        #endregion
    }
}
