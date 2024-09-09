using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awebapiTests;

public class TestDb
{

    // Initial setup of dbcontext to be used in tests classes

    private readonly SqliteConnection _connection;

    //call dbcontext
    public HealthDbContext DBcontext { get; }

    public TestDb()
    {
        // create and open a connection

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var contextOptions = new DbContextOptionsBuilder<HealthDbContext>()
            .UseSqlite(_connection)
            .Options;
        // fill and setup inmemory DBcontext with dbcontext "healthdbcontext"
        DBcontext = new HealthDbContext(contextOptions);
        DBcontext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Dispose();
        DBcontext.Dispose();
    }
}

