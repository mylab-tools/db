using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using MyLab.Db;
using Xunit;

namespace UnitTests
{
    public class AutoTransactionBehavior : IClassFixture<TestDbFixture>
    {
        private readonly TestDbFixture _db;

        /// <summary>
        /// Initializes a new instance of <see cref="AutoTransactionBehavior"/>
        /// </summary>
        public AutoTransactionBehavior(TestDbFixture db)
        {
            _db = db;
        }

        [Fact]
        public async Task ShouldCommitChangesAutomatically()
        {
            //Arrange
            var entId = new Random((int)(DateTime.Now.Ticks - DateTime.Now.Date.Ticks)).Next();

            //Act
            await _db.Connection.PerformAutoTransactionAsync(dc =>
            {
                return dc.GetTable<TestDbEntity>().InsertAsync(() => new TestDbEntity
                {
                    Id = entId,
                    Value = "foo"
                });
            });

            var found = await _db.Connection
                .GetTable<TestDbEntity>()
                .FirstOrDefaultAsync(e => e.Id == entId);

            //Assert
            Assert.NotNull(found);
            Assert.Equal("foo", found.Value);
        }

        [Fact]
        public async Task ShouldRevertChangesIfException()
        {
            //Arrange
            var entId = new Random((int) (DateTime.Now.Ticks - DateTime.Now.Date.Ticks)).Next();
            var exceptionToThrow = new Exception();

            //Act

            Exception caughtException = null;
            try
            {
                await _db.Connection.PerformAutoTransactionAsync(async dc =>
                {
                    await dc.GetTable<TestDbEntity>().InsertAsync(() => new TestDbEntity
                    {
                        Id = entId,
                        Value = "foo"
                    });

                    throw exceptionToThrow;
                });
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            var found = await _db.Connection
                .GetTable<TestDbEntity>()
                .FirstOrDefaultAsync(e => e.Id == entId);

            //Assert
            Assert.Equal(exceptionToThrow, caughtException);
            Assert.Null(found);
        }
    }
}
