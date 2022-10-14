using System.Collections.Generic;
using Exercise2.Model;

namespace Exercise2.ViewModel;

public interface ISideBarViewModel : IViewModel
{
    IList<Movie> Movies { get; }
    Movie? SelectedMovie { get; set; }
}