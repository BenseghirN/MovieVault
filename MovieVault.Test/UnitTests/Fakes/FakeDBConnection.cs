using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

public class FakeDbConnection : DbConnection
{
    private ConnectionState _state = ConnectionState.Closed;

    public override string ConnectionString { get; set; } = "Fake_Connection_String";
    public override string Database => "FakeDB";
    public override string DataSource => "FakeDataSource";
    public override string ServerVersion => "1.0";
    public override ConnectionState State => _state;

    public override void Open() => _state = ConnectionState.Open;
    public override void Close() => _state = ConnectionState.Closed;
    public override void ChangeDatabase(string databaseName) { }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Close();
        }
        base.Dispose(disposing);
    }
    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => new FakeDbTransaction(this);
    protected override DbCommand CreateDbCommand() => throw new NotImplementedException();
}
