using System.Data;

namespace Kata.BusinessLogic.Interfaces
{
    public interface ITransactionHandler
    {
        Task ExecuteTransactionAsync(Func<IDbTransaction, IDbConnection, Task> operation);
    }
}
