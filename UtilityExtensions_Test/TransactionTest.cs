using NUnit.Framework;
using UtilityExtensions.Core.Transactions;

namespace UtilityExtensions_Test
{
    public class TransactionTest
    {
        private class Add10Transaction : Transaction
        {
            public int Value { get; private set; }

            public Add10Transaction(int value)
            {
                Value = value;
            }

            protected override void InternalExecute()
            {
                Value += 10;
            }

            protected override void InternalRollback()
            {
                Value -= 10;
            }
        }

        private class StringUpperCaseTransaction : Transaction
        {
            public string Result { get; private set; }
            private string input;

            public StringUpperCaseTransaction(string value)
            {
                input = value;
                Result = value;
            }

            protected override void InternalExecute()
            {
                Result = input.ToUpper();
            }

            protected override void InternalRollback()
            {
                Result = input;
            }
        }

        [Test]
        public void ExecuteSuccessTransaction()
        {
            Add10Transaction tran1 = new Add10Transaction(10);
            TransactionManager.Add(tran1).Execute();

            Assert.IsTrue(tran1.Value == 20);
        }

        [Test]
        public void RollbackSuccessTransaction()
        {
            Add10Transaction tran1 = new Add10Transaction(10);
            TransactionManager tm = TransactionManager.Add(tran1);
            tm.Execute();
            tm.Rollback();

            Assert.IsTrue(tran1.Value == 10);
        }

        [Test]
        public void ExecuteFailTransaction()
        {
            StringUpperCaseTransaction tran1 = new StringUpperCaseTransaction(null);
            try
            {
                TransactionManager.Add(tran1).Execute();
            }
            catch (TransactionException)
            {
                Assert.IsTrue(tran1.state == Transaction.State.Pending);
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}