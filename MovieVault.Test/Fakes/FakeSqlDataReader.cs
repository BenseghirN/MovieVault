using System.Collections;
using System.Data;
using System.Data.Common;

namespace MovieVault.Test.Fakes
{
    public class FakeSqlDataReader : DbDataReader
    {
        private readonly List<Dictionary<string, object>> _rows;
        private int _currentIndex = -1;

        public FakeSqlDataReader(List<Dictionary<string, object>> rows)
        {
            _rows = rows;
        }
        public override bool Read() => ++_currentIndex < _rows.Count;
        public override int Depth { get; }
        public override bool IsClosed { get; }
        public override int RecordsAffected { get; }

        public override object this[int ordinal] => throw new NotImplementedException();

        public override object this[string name] => _rows[_currentIndex][name];
        public override object GetValue(int ordinal) => this[GetName(ordinal)];
        public override int GetValues(object[] values)
        {
            if (_currentIndex < 0 || _currentIndex >= _rows.Count)
                return 0;

            var row = _rows[_currentIndex];
            int count = 0;

            foreach (var value in row.Values)
            {
                values[count] = value;
                count++;
            }

            return count;
        }

        public override bool GetBoolean(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal) => (int)GetValue(ordinal);
        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetString(int ordinal) => (string)GetValue(ordinal);
        public override bool IsDBNull(int ordinal) => GetValue(ordinal) is DBNull;
        public override int FieldCount => _rows.Count > 0 ? _rows[0].Count : 0;
        public override bool HasRows => _rows.Count > 0;
        public override string GetName(int ordinal) => _rows[0].Keys.ElementAt(ordinal);
        public override bool NextResult() => false;
        public override Task<bool> ReadAsync(CancellationToken cancellationToken) => Task.FromResult(Read());
        public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken) =>
            Task.FromResult(IsDBNull(ordinal));
    }
}
