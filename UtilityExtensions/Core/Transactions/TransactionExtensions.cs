using System;

namespace UtilityExtensions.Core.Transactions
{
    public static class TransactionExtensions
    {
        /// <summary>
        /// Adds a transaction to transaction the manager
        /// </summary>
        /// <param name="transaction"> </param>
        /// <param name="onExecuteTransaction"> </param>
        /// <param name="onRollbackTransaction"> </param>
        /// <returns> </returns>
        public static TransactionManager Add(this TransactionManager m, Transaction transaction, Action<Transaction> onExecuteTransaction = null, Action<Transaction> onRollbackTransaction = null)
        {
            m.AddTransaction(transaction, onExecuteTransaction, onRollbackTransaction);
            return m;
        }
    }
}