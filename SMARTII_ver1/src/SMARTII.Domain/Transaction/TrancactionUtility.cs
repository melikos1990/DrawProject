using System;
using System.Transactions;

namespace SMARTII.Domain.Transaction
{
    public static class TrancactionUtility
    {
        public static TransactionScope TransactionScope()
        {
            var scopeOption = TransactionScopeOption.Required;
            var asyncOption = TransactionScopeAsyncFlowOption.Enabled;
            var options = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = new TimeSpan(0, 2, 0)
            };

            return new TransactionScope(scopeOption, options, asyncOption);
        }

        public static TransactionScope NoTransactionScope()
        {
            var scopeOption = TransactionScopeOption.Suppress;

            return new TransactionScope(scopeOption);
        }

        public static TransactionScope TransactionScopeNoLock()
        {
            var scopeOption = TransactionScopeOption.Required;
            var asyncOption = TransactionScopeAsyncFlowOption.Enabled;
            var options = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.Snapshot,
                Timeout = new TimeSpan(0, 2, 0)
            };

            return new TransactionScope(scopeOption, options, asyncOption);
        }

    }
}
