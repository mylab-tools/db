using System;
using System.Diagnostics;
using System.IO;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class TestDbFixture : IDisposable
    {
        private readonly DataConnection _connection;
        private readonly string _filename;

        public ITestOutputHelper Output { get; set; }

        public string ConnectionString { get; }

        public TestDbFixture()
        {
            DataConnection.TurnTraceSwitchOn(TraceLevel.Verbose);

            _filename = $"{Guid.NewGuid():N}.db";
            ConnectionString = $"Data Source={_filename};";

            _connection = new DataConnection(new SQLiteDataProvider(), ConnectionString);
            _connection.OnTraceConnection += info =>
            {
                if (info.TraceInfoStep == TraceInfoStep.BeforeExecute)
                {
                    Debug.WriteLine(info.SqlText);
                    Output?.WriteLine(info.SqlText);
                }
            };

            _connection.CreateTable<TestDbEntity>();
            _connection.GetTable<TestDbEntity>().Insert(() => new TestDbEntity
                {
                    Id = 0,
                    Value = "foo"
                });
        }

        public void Dispose()
        {
            _connection.OnTraceConnection = null;
            _connection.Dispose();

            File.Delete(_filename);
        }
    }

    [Table("test")]
    public class TestDbEntity
    {
        [Column("id", IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column("value")]
        public string Value { get; set; }
    }
}