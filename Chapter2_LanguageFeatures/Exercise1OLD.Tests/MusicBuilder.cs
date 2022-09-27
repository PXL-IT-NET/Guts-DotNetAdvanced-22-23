using Chapter2_LanguageFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Tests
{
    internal class MusicBuilder
    {
        private readonly Music _music;
        public MusicBuilder()
        {
            _music = new Music
            {
                 Title = Guid.NewGuid().ToString(),
                 Componist = Guid.NewGuid().ToString(),
                 ReleaseDate = DateTime.Today,
                 Description = Guid.NewGuid().ToString(),
            };            
        }

        public MusicBuilder WithTitle(string title)
        {
            _music.Title = title;
            return this;
        }

        public MusicBuilder WithComponist(string componist)
        {
            _music.Componist = componist;
            return this;
        }

        public MusicBuilder WithDescription(string description)
        {
            _music.Description = description;
            return this;
        }

        public MusicBuilder WithReleaseDate(DateTime releaseDate)
        {
            _music.ReleaseDate = releaseDate;
            return this;
        }

        public Music Build()
        {
            return _music;
        }
    }
}
