using System.Reflection;

namespace Exercise1.Tests
{
    public static class ExtensionMethods
    {
        public static bool HasProperty(this Type type, string name)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(p => p.Name == name);
        }

        public static bool HasDefaultConstructor(this Type t)
        {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
        }

        public static DateTime Next(this DateTime start)
        {
            Random random = new Random();      
            return start.AddDays(random.Next(100));
        }

    }
}
