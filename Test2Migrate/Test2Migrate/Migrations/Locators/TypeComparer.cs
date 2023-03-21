namespace Test2Migrate.Migrations.Locators
{
    public class TypeComparer : IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            return x.AssemblyQualifiedName == y.AssemblyQualifiedName;
        }

        public int GetHashCode(Type obj)
        {
            return obj.AssemblyQualifiedName.GetHashCode();
        }
    }
}
