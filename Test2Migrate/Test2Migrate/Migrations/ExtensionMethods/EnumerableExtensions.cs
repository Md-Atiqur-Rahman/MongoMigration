using Test2Migrate.Migrations.Services;

namespace Test2Migrate.Migrations.ExtensionMethods
{
    internal static class EnumerableExtensions
    {
        internal static IDictionary<Type, IReadOnlyCollection<TMigrationType>> ToMigrationDictionary<TMigrationType>(
            this IEnumerable<TMigrationType> migrations)
            where TMigrationType : class, IMigration
        {
            var dictonary = new Dictionary<Type, IReadOnlyCollection<TMigrationType>>();
            var list = migrations.ToList();
            var types = (from m in list select m.Type).Distinct();

            foreach (var type in types)
            {
                if (dictonary.ContainsKey(type))
                    continue;

                var uniqueMigrations =
                    list.Where(m => m.Type == type).OrderBy(m => m.Type).ToList();
                dictonary.Add(type, uniqueMigrations);
            }

            return dictonary;
        }

        
    }
}
