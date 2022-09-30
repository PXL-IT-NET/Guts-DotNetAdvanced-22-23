namespace Exercise1;

public class CompositionSearcher
{
    public IList<Composition> SearchMusic(CompositionFilterDelegate filterMethod, string searchKeyword)
    {
        throw new NotImplementedException();
        
    }

    private IList<Composition> GetAllCompositions()
    {
        return new List<Composition>
        {
            new Composition {
                Title = "Für Elise",
                Description = "Een mogelijkheid is dat Elise een van zijn vele pianostudentes was, waar hij verliefd op was. Het is van Van Beethoven bekend dat hij voor deze jongedames muziekstukken schreef die veel te moeilijk waren, zodat hij verzekerd was van nog veel pianolessen.",
                Composer = "Ludwig Van Beethoven",
                ReleaseDate = new DateTime(1810, 04, 27)
            },
            new Composition {
                Title = "Het zwanenmeer",
                Description = "Het thema is liefde en verlooching. Het hele verhaal draait om de liefde tussen Siegfried en Odette. De tovenaar probeert dit kapot te maken en zorgt zo dat Siegfried Odette verloochent, door Siegfried met zijn dochter te laten trouwen.",
                Composer = "Pjotr Tsjaikovski",
                ReleaseDate = new DateTime(1877, 03, 04)
            },
            new Composition {
                Title = "Mattheus Passion",
                Description = "De Matthäus-Passion vertelt het lijdens- en sterfverhaal van Jezus als in het Evangelie volgens Matteüs",
                Composer = "J.S. Bach",
                ReleaseDate = new DateTime(1727, 04, 11)
            },
            new Composition{
                Title="Een kleine serenade",
                Description = "A stylesheet that makes html look beautiful...",
                Composer = "Kaelee",
                ReleaseDate = new DateTime(2019, 07, 18)
            }
        };
    }
}