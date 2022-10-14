using System.ComponentModel;
using Exercise2.Model;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\Model\Movie.cs;")]
public class MovieTests
{
    [MonitoredTest("Movie - Constructor - Should initialize properties correctly")]
    public void _01_Constructor_ShouldInitializePropertiesCorrectly()
    {
        var movie = new Movie();

        Assert.That(movie.Title, Is.EqualTo(string.Empty), "Title should be an empty string");
        Assert.That(movie.OpeningCrawl, Is.EqualTo(string.Empty), "OpeningCrawl should be an empty string");
        Assert.That(movie.Rating, Is.EqualTo(1), "Rating should be 1");
    }

    [MonitoredTest("Movie - Rating - Should always be between 1 and 5")]
    public void _02_Rating_ShouldAlwaysBeBetween1And5()
    {
        var movie = new Movie();

        movie.Rating = 1;
        Assert.That(movie.Rating, Is.EqualTo(1));

        movie.Rating = 0;
        Assert.That(movie.Rating, Is.EqualTo(1), "When a value lower than 1 is set, the Rating must become 1");

        movie.Rating = 6;
        Assert.That(movie.Rating, Is.EqualTo(5), "When a value higher than 5 is set, the Rating must become 5");
    }

    [MonitoredTest("Movie - Rating - Should notify when it is changed")]
    public void _03_Rating_ShouldNotifyWhenIsIsChanged()
    {
        var movie = new Movie();

        INotifyPropertyChanged? notifier = movie as INotifyPropertyChanged;
        Assert.That(notifier, Is.Not.Null, "The Movie class should be able to notify about its property changes");

        bool notified = false;
        notifier.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(Movie.Rating))
            {
                notified = true;
            }
        };

        movie.Rating = 4;
        Assert.That(notified, Is.True);
    }
}