using System.Reflection;

namespace Exercise2.Tests;

internal class TestHelper
{
    public static TypeInfo? GetNewStudentEventHandlerTypeInfo()
    {
        Type type = typeof(StudentAdministration);
        Assembly assembly = Assembly.GetAssembly(type)!;
        return assembly.DefinedTypes.FirstOrDefault(t =>
        {
            if (!typeof(MulticastDelegate).IsAssignableFrom(t)) return false;

            //check signature (must return bool and have a Composition and a string parameter)
            var methodInfo = t.DeclaredMethods.First(p => p.Name == "Invoke");

            if (methodInfo.ReturnType.Name.ToLower() != "void") return false;
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 2) return false;
            if (parameters[0].ParameterType != typeof(object)) return false;
            if (parameters[1].ParameterType != typeof(StudentEventArgs)) return false;
            return true;
        });
    }

    public static bool IsMarkedAsNullable(PropertyInfo p)
    {
        return new NullabilityInfoContext().Create(p).ReadState is NullabilityState.Nullable;
    }
}