using Test2Migrate.Migrations.ExtensionMethods;
using Test2Migrate.Migrations.Services;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Locators
{
    internal class TypeMigrationLocator : MigrationLocator<IMigration>, IDatabaseTypeMigrationDependencyLocator
    {
        public override void Locate(List<MigrationHistory> appliedData)
        {
            var Assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var migrationTypes =
               (from assembly in Assemblies
                from type in assembly.GetTypes()
                where typeof(IMigration).IsAssignableFrom(type) && !type.IsAbstract
                && !appliedData.Any(y => y.MigrationClassName == type.Name)
                select type).OrderBy(x=>x.Name).Distinct();
            Migrations = migrationTypes.Select(GetMigrationInstance).ToMigrationDictionary();

        }

        public override void Locate2(string runPendingData)
        {
            var Assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var migrationTypes =
               (from assembly in Assemblies
                from type in assembly.GetTypes()
                where typeof(IMigration).IsAssignableFrom(type) && !type.IsAbstract
                && type.Name == runPendingData
                select type).OrderBy(x => x.Name).Distinct();
            MigrationNames = migrationTypes.Select(GetMigrationInstance).ToMigrationDictionary();


        }

    }
}
