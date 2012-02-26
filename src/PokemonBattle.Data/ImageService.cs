using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics.Contracts;
using LightStudio.PokemonBattle.Data;
using LightStudio.Tactic.DataModels.IO;

namespace LightStudio.PokemonBattle.Data
{
    public class ImageService
    {
        private DataCollection dataCollection;
        private DataConfiguration configuration;

        internal ImageService(DataCollection data, DataConfiguration config)
        {
            this.dataCollection = data;
            this.configuration = config;
        }

        public ImageSource GetPokemonIcon(int id)
        {
            return GetImage(string.Format(configuration.IconPath, id));
        }

        public ImageSource GetPokemonMaleFront(int id)
        {
            return GetImage(string.Format(configuration.FrontPath, id));
        }

        public ImageSource GetPokemonFemaleFront(int id)
        {
            ImageSource image = GetImage(string.Format(configuration.FemaleFrontPath, id));
            if (image == null)
                return GetPokemonMaleFront(id);
            return image;
        }

        public ImageSource GetPokemonMaleBack(int id)
        {
            return GetImage(string.Format(configuration.BackPath, id));
        }

        public ImageSource GetPokemonFemaleBack(int id)
        {
            ImageSource image = GetImage(string.Format(configuration.FemaleBackPath, id));
            if (image == null)
                return GetPokemonMaleBack(id);
            return image;
        }

        private ImageSource GetImage(string relativePath)
        {
            string absolutePath = dataCollection.GetAbsolutePath(relativePath);
            if (!File.Exists(absolutePath)) return null;
            return new BitmapImage(new Uri(absolutePath, UriKind.Absolute));
        }

    }
}
