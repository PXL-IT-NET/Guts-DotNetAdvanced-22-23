namespace Exercise1
{
    public class Movie
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }

        public Movie()
        {
            Title = "Unknown";
            Director = "Unknown";
            ReleaseYear = 0;
        }
    }
}
