using awebapi.DTOs;
using awebapi.Entities;
using awebapi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using FluentAssertions;

namespace awebapiTests
{
    public class MedicinesServiceTests : IDisposable
    {
        private readonly TestDb _testDb;
        private readonly MedicinesService _medicinesService;
        private readonly SeedDataHelper _dataHelper;

        public MedicinesServiceTests()
        {
           _testDb = new TestDb();
           _dataHelper = new SeedDataHelper(_testDb);
           _medicinesService = new MedicinesService(_testDb.DBcontext);
        }

        [Fact]
        public async Task FetchAllAsync_ToSuccess()
        {
            await _dataHelper.InsertMedicineToDb( Guid.NewGuid(), "Test", "test", true, DateTime.Now, DateOnly.FromDateTime(DateTime.Now));

            var result = await _medicinesService.FetchAllAsync();
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task DeleteAsync_toPass()
        {
            var id = Guid.NewGuid();
            await _dataHelper.InsertMedicineToDb(id, "Test", "test", true, DateTime.Now, DateOnly.FromDateTime(DateTime.Now));
            await _medicinesService.DeleteAsync(id);

            var result = await _medicinesService.FetchByIdAsync(id);

            result.Should().BeNull();

        }

        [Fact]
        public async Task isActiveUpdate_toPass()
        {
            var id = Guid.NewGuid();
            await _dataHelper.InsertMedicineToDb(id, "Test", "test", true, DateTime.Now, DateOnly.FromDateTime(DateTime.Now));
            await _medicinesService.UpdateActivation(id, false);

            var result = await _medicinesService.FetchByIdAsync(id);

            result?.Is_active.Should().BeFalse();

        }


        [Fact]
        public async Task UpdateMed_toPass()
        {
            var id = Guid.NewGuid();
            var addedMed = await _dataHelper.InsertMedicineToDb(id, "test", "test", true, DateTime.Now, DateOnly.FromDateTime(DateTime.Now));

            addedMed.Name = "";
            addedMed.Description = "Description";
            addedMed.Is_active = false;
            _medicinesService.UpdateMed(addedMed);
            var result = await _medicinesService.FetchByIdAsync(id);

            result?.Name.Should().Be(addedMed.Name);
        }

        [Fact]
        public async Task getByDate_toPass()
        {
            var id = Guid.NewGuid();
            var createdDate = DateTime.Now;
            var newMed = await _dataHelper.InsertMedicineToDb(id, "test", "test", false, createdDate, DateOnly.FromDateTime(DateTime.Now));
            var result = await _medicinesService.GetByDate(createdDate);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task getByDate_toFail()
        {
            var id = Guid.NewGuid();
            var createdDate = DateTime.Now;
            var newMed = await _dataHelper.InsertMedicineToDb(id, "test", "test", false, createdDate, DateOnly.FromDateTime(DateTime.Now));
            var result = await _medicinesService.GetByDate(new DateTime(2024, 9, 6, 14, 30, 0));

            result.Should().BeNullOrEmpty();
        }


        [Fact]

        public async Task GetbyExpiryDate_toPass()
        {
            var id = Guid.NewGuid();
            var expiryDate = new DateOnly(2023, 6, 9);
            var newMed = await _dataHelper.InsertMedicineToDb(id, "test", "test", true, DateTime.Now, expiryDate );
            var result = await _medicinesService.GetbyExpiryDate(new DateOnly(2023, 6, 10));

            result.Should().NotBeNullOrEmpty();

        }

        [Fact]
        public async Task GetbyExpiryDate_toFail()
        {
            var id = Guid.NewGuid();
            var expiryDate = new DateOnly(2023, 6, 9);
            var newMed = await _dataHelper.InsertMedicineToDb(id, "test", "test", true, DateTime.Now, expiryDate);
            var result = await _medicinesService.GetbyExpiryDate(expiryDate);

            result.Should().BeNullOrEmpty();

        }


        public void Dispose()
        {
            _testDb.Dispose();
        }
    }
}
