using System.Windows;
using System.Windows.Data;
using NUnit.Framework;

namespace TestTools;

public static class BindingUtil
{
    public static BindingExpression AssertBinding(FrameworkElement targetElement, DependencyProperty targetProperty,
        string expectedBindingPath, BindingMode allowedBindingMode, string? targetElementName = null)
    {
        BindingExpression binding = targetElement.GetBindingExpression(targetProperty);

        if (string.IsNullOrEmpty(targetElementName))
        {
            targetElementName = targetElement.Name;
        }
        if (string.IsNullOrEmpty(targetElementName))
        {
            targetElementName = targetElement.GetType().Name;
        }

        var errorMessage =
            $"Invalid 'Binding' for the '{targetProperty.Name}' property of {targetElementName}.";
        Assert.That(binding, Is.Not.Null, errorMessage);
        Assert.That(binding.ParentBinding.Path.Path, Is.EqualTo(expectedBindingPath), errorMessage);

        var allowedBindingModes = new List<BindingMode> { allowedBindingMode };
        if (targetProperty.GetMetadata(targetElement) is FrameworkPropertyMetadata metaData)
        {
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

        return binding;
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