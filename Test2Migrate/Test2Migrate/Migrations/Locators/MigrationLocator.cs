using System.Reflection;
using Test2Migrate.Migrations.Services;
using Test2Migrate.Models;

namespace Test2Migrate.Migrations.Locators
{
    public abstract class MigrationLocator<TMigrationType> : IMigrationLocator<TMigrationType>
       where TMigrationType : class, IMigration
    {
        
        private IDictionary<Type, IReadOnlyCollection<TMigrationType>> _migrations;
        private List<MigrationHistory> _appliedData;

        private IDictionary<Type, IReadOnlyCollection<TMigrationType>> _migrationNames;
        private string _runMigrationName;

        protected virtual IDictionary<Type, IReadOnlyCollection<TMigrationType>> Migrations
        {
            get
            {
                if (_migrations == null)
                    Locate(_appliedData);

                //if (_migrations.NullOrEmpty())
                //    _logger.Warn(new NoMigrationsFoundException());

                return _migrations;
            }
            set => _migrations = value;
        }
        public IEnumerable<TMigrationType> GetMigrations(Type type, List<MigrationHistory> appliedData)
        {
            _appliedData = appliedData;
            IReadOnlyCollection<TMigrationType> migrations;
            Migrations.TryGetValue(type, out migrations);

            return migrations ?? Enumerable.Empty<TMigrationType>();
        }

        public TMigrationType GetMigrationInstance(Type type)
        {
            ConstructorInfo constructor = type.GetConstructors()[0];

            if (constructor != null)
            {
                object[] args = constructor
                    .GetParameters()
                    .Select(o => o.ParameterType)
                    .Select(o => (IMigration)Activator.CreateInstance(o))
                    .ToArray();

                return Activator.CreateInstance(type, args) as TMigrationType;
            }

            return Activator.CreateInstance(type) as TMigrationType;
        }
        public abstract void Locate(List<MigrationHistory> appliedData);

        public abstract void Locate2(string appliedData);
        public IEnumerable<TMigrationType> GetMigrationByName(Type type, string runMigrationName)
        {
            _runMigrationName = runMigrationName;
            IReadOnlyCollection<TMigrationType> migrations;
            MigrationNames.TryGetValue(type, out migrations);

            return migrations ?? Enumerable.Empty<TMigrationType>();
        }

        protected virtual IDictionary<Type, IReadOnlyCollection<TMigrationType>> MigrationNames
        {
            get
            {
                if (_migrationNames == null)
                    Locate2(_runMigrationName);

                //if (_migrations.NullOrEmpty())
                //    _logger.Warn(new NoMigrationsFoundException());

                return _migrationNames;
            }
            set => _migrationNames = value;
        }

    }
}
