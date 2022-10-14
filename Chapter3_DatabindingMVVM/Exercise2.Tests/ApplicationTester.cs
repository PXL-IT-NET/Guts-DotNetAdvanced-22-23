using System.Windows;
using System.Windows.Markup;

namespace Exercise2.Tests;

internal class ApplicationTester
{
    private readonly App _application;
    private bool _resourcesLoaded;
    private ResourceDictionary? _brushResources;
    private ResourceDictionary? _converterResources;

    public ApplicationTester()
    {
        _application = Application.Current as App ?? new App();
    }

    public void LoadApplication(Action? componentInitializer = null)
    {
        try
        {
            _application.InitializeComponent(); //loads resources
            _resourcesLoaded = true;
        }
        catch (Exception)
        {
            _resourcesLoaded = false;
        }

        try
        {
            componentInitializer?.Invoke();
        }
        catch (XamlParseException)
        {
            _resourcesLoaded = false;
        }
    }

    public T TryGetApplicationResource<T>(string key) where T:class
    {
        AssertApplicationResourcesAreLoaded();

        bool found = false;
        int dictionaryIndex = 0;
        T? resource = null;
        while (!found && dictionaryIndex < _application.Resources.MergedDictionaries.Count)
        {
            ResourceDictionary resourceDictionary = _application.Resources.MergedDictionaries[dictionaryIndex];
            found = resourceDictionary.Keys.OfType<string>().Any(k => k == key);
            if (found)
            {
                resource = resourceDictionary[key] as T;
            }
            dictionaryIndex++;
        }
        Assert.That(found, Is.True, $"Cannot find a resource with key '{key}' in the application resources");
        Assert.That(resource, Is.Not.Null, $"The resource with key '{key}' should be of type {typeof(T)}");
        return resource!;
    }

    public void AssertApplicationResourcesAreLoaded()
    {
        Assert.That(_resourcesLoaded, Is.True,
            "Could not load the application resources (App.xaml).\r\n" +
            "Make sure the 'Source' of each ResourceDictionary starts with 'pack://application:,,,/Exercise2;component' followed by the relative URI to the source.\r\n" +
            "This is needed because the resources are also used in the test project.\r\n" +
            "Example: pack://application:,,,/Exercise2;component/Resources/SomeResource.xaml \r\n" +
            "More info: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf");

        _brushResources = _application.Resources.MergedDictionaries.FirstOrDefault(rd => rd.Source.ToString().Contains("Brushes.xaml"));
        Assert.That(_brushResources, Is.Not.Null, "The 'Brushes.xaml' resources are not included in the application resources");

        _converterResources = _application.Resources.MergedDictionaries.FirstOrDefault(rd => rd.Source.ToString().Contains("Converters.xaml"));
        Assert.That(_converterResources, Is.Not.Null, "The 'Converters.xaml' resources are not included in the application resources");

    }

}