using System;
using System.Data;
using System.Data.Common;

public class FakeDbTransaction : DbTransaction
{
    private readonly FakeDbConnection _connection;
    public override IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;

    public FakeDbTransaction(FakeDbConnection connection) => _connection = connection;
    public override void Commit() { } // On simule le commit
    public override void Rollback() { } // On simule le rollback
    protected override DbConnection DbConnection => _connection;
}
