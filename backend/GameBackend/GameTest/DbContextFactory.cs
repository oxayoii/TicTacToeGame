using DataBase;
using Microsoft.EntityFrameworkCore;

namespace GameTest;

public static class DbContextFactory
{
    public static DataBaseContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DataBaseContext(options);
    }
}
