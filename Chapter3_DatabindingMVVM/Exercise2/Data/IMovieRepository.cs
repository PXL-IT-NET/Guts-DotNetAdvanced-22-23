using System.Collections.Generic;
using Exercise2.Model;

namespace Exercise2.Data;

public interface IMovieRepository
{
    IList<Movie> GetAll();
}