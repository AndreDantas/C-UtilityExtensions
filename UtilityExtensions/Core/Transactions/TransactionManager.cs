using System;
using System.Collections.Generic;

namespace UtilityExtensions.Core.Transactions
{
    public sealed class TransactionManager
    {
        public class TransactionHandler
        {
            public int Order { get; private set; }
            public Transaction transaction;
            public Action<Transaction> onExecute;
            public Action<Transaction> onRollback;

            public TransactionHandler(int order, Transaction transaction, Action<Transaction> onExecute, Action<Transaction> onRollback)
            {
                this.Order = order;
                this.transaction = transaction;
                this.onExecute = onExecute;
                this.onRollback = onRollback;
            }
        }

        public struct Settings
        {
            public bool throwExceptionOnError;
            public bool rollbackOnError;
            public Action<TransactionException> onExecuteError;
            public Action<TransactionException> onRollbackError;

            public static readonly Settings DEFAULT = new Settings { throwExceptionOnError = true, rollbackOnError = true };
        }

        private List<TransactionHandler> transactions = new List<TransactionHandler>();
        public IReadOnlyCollection<TransactionHandler> Transactions => transactions.AsReadOnly();
        private Settings settings;

        private TransactionManager()
        {
        }

        /// <summary>
        /// Initiates the transaction manager with settings
        /// </summary>
        /// <param name="settings"> </param>
        /// <returns> </returns>
        public static TransactionManager UseSettings(Settings settings)
        {
            return new TransactionManager { settings = settings };
        }

        /// <summary>
        /// Initiates the transaction manager with default settings and adds the first transaction
        /// </summary>
        /// <param name="transaction"> </param>
        /// <param name="onExecuteTransaction"> </param>
        /// <param name="onRollbackTransaction"> </param>
        /// <returns> </returns>
        public static TransactionManager Add(Transaction transaction, Action<Transaction> onExecuteTransaction = null, Action<Transaction> onRollbackTransaction = null)
        {
            TransactionManager m = new TransactionManager { settings = Settings.DEFAULT };
            m.AddTransaction(transaction, onExecuteTransaction, onRollbackTransaction);
            return m;
        }

        /// <summary>
        /// Adds a transaction
        /// </summary>
        /// <param name="transaction"> </param>
        /// <param name="onExecuteTransaction"> </param>
        /// <param name="onRollbackTransaction"> </param>
        public void AddTransaction(Transaction transaction, Action<Transaction> onExecuteTransaction = null, Action<Transaction> onRollbackTransaction = null)
        {
            transactions.Add(new TransactionHandler(transactions.Count + 1, transaction, onExecuteTransaction, onRollbackTransaction));
        }

        /// <summary>
        /// Executes all transactions in the order they were added
        /// </summary>
        public void Execute()
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                TransactionHandler handler = transactions[i];
                if (handler.transaction != null)
                {
                    try
                    {
                        //Try to execute the transaction.
                        if (handler.transaction.Execute())
                        {
                            try
                            {
                                handler.onExecute?.Invoke(handler.transaction);
                            }
                            catch { }
                        }
                    }
                    catch (Exception e)
                    {
                        //If a transaction failed, rollback all previous transactions.
                        if (settings.rollbackOnError)
                        {
                            Rollback();
                        }

                        TransactionException transactionException = GetTransactionException(handler, e);

                        try
                        {
                            settings.onExecuteError?.Invoke(transactionException);
                        }
                        catch { }

                        if (settings.throwExceptionOnError)
                        {
                            throw transactionException;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Rollbacks all transactions in the reverse order they were added
        /// </summary>
        public void Rollback()
        {
            for (int i = transactions.Count - 1; i >= 0; i--)
            {
                TransactionHandler handler = transactions[i];
                if (handler.transaction != null)
                {
                    try
                    {
                        //Try to rollback the transaction.
                        if (handler.transaction.Rollback())
                        {
                            try
                            {
                                handler.onRollback?.Invoke(handler.transaction);
                            }
                            catch { }
                        }
                    }
                    catch (Exception e)
                    {
                        TransactionException transactionException = GetTransactionException(handler, e);

                        try
                        {
                            settings.onRollbackError?.Invoke(transactionException);
                        }
                        catch { }

                        if (settings.throwExceptionOnError)
                        {
                            throw transactionException;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes all transactions from the manager
        /// </summary>
        /// <returns> </returns>
        public void Clear()
        {
            transactions.Clear();
        }

        private TransactionException GetTransactionException(TransactionHandler handler, Exception exception)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            string state = handler.transaction?.state == Transaction.State.Pending ? "execute" : "rollback";
            return new TransactionException($"Transaction {handler.Order} ({handler.transaction.GetType()}) failed to {state}, with error: {exception?.Message}", handler.transaction, exception);
        }
    }
}