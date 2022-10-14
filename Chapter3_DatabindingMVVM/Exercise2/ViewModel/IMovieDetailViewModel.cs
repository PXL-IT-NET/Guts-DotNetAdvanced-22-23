using Exercise2.Command;
using Exercise2.Model;

namespace Exercise2.ViewModel;

public interface IMovieDetailViewModel : IViewModel
{
    DelegateCommand GiveFiveStarRatingCommand { get; }
    Movie? Movie { get; set; }
    bool HasNoMovie { get; }
}