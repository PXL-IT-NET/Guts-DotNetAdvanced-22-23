using System.Windows;
using System.Windows.Data;

namespace Exercise1.Tests;

public static class BindingUtil
{
    public static void AssertBinding(FrameworkElement targetElement, DependencyProperty targetProperty,
        string expectedBindingPath, BindingMode allowedBindingMode)
    {
        BindingExpression binding = targetElement.GetBindingExpression(targetProperty);
        var errorMessage =
            $"Invalid 'Binding' for the '{targetProperty.Name}' property of {targetElement.Name}.";
        Assert.That(binding, Is.Not.Null, errorMessage);
        Assert.That(binding.ParentBinding.Path.Path, Is.EqualTo(expectedBindingPath), errorMessage);

        var allowedBindingModes = new List<BindingMode> { allowedBindingMode };
        var metaData = (FrameworkPropertyMetadata)targetProperty.GetMetadata(targetElement);
        if (allowedBindingMode == BindingMode.TwoWay && metaData.BindsTwoWayByDefault)
        {
            allowedBindingModes.Add(BindingMode.Default);
        }
        else if (allowedBindingMode == BindingMode.OneWay)
        {
            allowedBindingModes.Add(BindingMode.Default);
        }
        Assert.That(allowedBindingModes, Has.One.EqualTo(binding.ParentBinding.Mode), errorMessage);
    }

    public static void AssertElementBinding(FrameworkElement targetElement, DependencyProperty targetProperty, BindingMode allowedBindingMode, FrameworkElement sourceElement)
    {
        BindingExpression binding = targetElement.GetBindingExpression(targetProperty);
        var errorMessage =
            $"Invalid 'Binding' for the '{targetProperty.Name}' property of {targetElement.Name}.";
        Assert.That(binding, Is.Not.Null, errorMessage);

        var allowedBindingModes = new List<BindingMode> { allowedBindingMode };
        var metaData = (FrameworkPropertyMetadata)targetProperty.GetMetadata(targetElement);
        if (allowedBindingMode == BindingMode.TwoWay && metaData.BindsTwoWayByDefault)
        {
            allowedBindingModes.Add(BindingMode.Default);
        }
        else if (allowedBindingMode == BindingMode.OneWay)
        {
            allowedBindingModes.Add(BindingMode.Default);
        }
        Assert.That(allowedBindingModes, Has.One.EqualTo(binding.ParentBinding.Mode), errorMessage);

        errorMessage = $"Invalid 'Binding' source for the '{targetProperty.Name}' property of {targetElement.Name}.";
        Assert.That(binding.ParentBinding.ElementName, Is.EqualTo(sourceElement.Name), errorMessage);
    }

}