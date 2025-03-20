using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess;
using System.Data;

namespace Kata.BusinessLogic.Handlers
{
    public class TransactionHandler : ITransactionHandler
    {
        private readonly ISqlDataAccess _dataAccess;

        public TransactionHandler(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task ExecuteTransactionAsync(Func<IDbTransaction, IDbConnection, Task> operation)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                await operation(transaction, connection);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
