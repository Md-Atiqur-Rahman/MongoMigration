using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test2Migrate.Migrations.Services;
using Test2Migrate.Models;

namespace Test2Migrate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationsController : ControllerBase
    {
        private readonly IMongoMigrationsRunner _mongoMigrationsRunner;
        private readonly IMigrationHistoryService _migrationHistoryService;
        public MigrationsController(IMongoMigrationsRunner mongoMigrationsRunner,
            IMigrationHistoryService migrationHistoryService)
        {
            _mongoMigrationsRunner = mongoMigrationsRunner;
            _migrationHistoryService = migrationHistoryService;
        }
        
        [HttpGet("GetAllAppliedMigartionHistory")]
        public ActionResult<List<MigrationHistory>> GetAllAppliedMigartionHistory()
        {
            return _migrationHistoryService.GetAllAppliedMigartionHistory();
            
        }
        [HttpGet("GetPendingMigartion")]
        public ActionResult<List<MigrationHistory>> GetPendingMigartion()
        {
            return _migrationHistoryService.GetPendingMigartionFileNames();

        }
        [HttpGet("RunMigartionByMigrationName", Name ="RunMigartionByMigrationName{migrationName}")]
        public async Task<ActionResult> RunMigartionByMigrationName(string migrationName)
        {

            await _mongoMigrationsRunner.RunMigrationByFileName(migrationName);
            return Ok();

        }
        [HttpPost("RunMigration")]
        public async Task<ActionResult> Migrate()
        {
             await _mongoMigrationsRunner.RunAllPendingMigration();
            return Ok();
        }
    }
}
