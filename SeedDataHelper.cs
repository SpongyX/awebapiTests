using awebapi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awebapiTests;
public class SeedDataHelper
{
    private readonly TestDb _testDb;

    // Initalization of Test.Db
    public SeedDataHelper(TestDb testDb)
    {
        _testDb = testDb;
    }

    // add medicine to in memory db
    
    internal async Task<Medicines> InsertMedicineToDb(Guid id, string name, string description, bool isactive, DateTime Created_at, DateOnly Expiry_date)
    {
        var medicine = new Medicines {

            Med_id = id,
            Name = name,
            Description = description,
            Is_active = isactive,
            Created_at = Created_at,
            Expiry_date = Expiry_date
            
        };
        await _testDb.DBcontext.Medicines.AddAsync(medicine);
        await _testDb.DBcontext.SaveChangesAsync();

        return medicine;
    }
}
