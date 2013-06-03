using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattle.PokemonData;
using System.Windows.Input;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.IO;
using PokemonDataEditor.Util;
using Microsoft.Win32;

namespace PokemonDataEditor.ViewModel
{
    public class MainWindowViewModel
    {
        private string dataPath;

        public static ObservableCollection<PokemonType> PokemonTypes
        { get; private set; }

        public static ObservableCollection<MoveData> Moves
        { get; private set; }

        public static ObservableCollection<PokemonData> Pokemons
        { get; private set; }

        public ObservableCollection<MenuViewModel> Menus
        { get; private set; }

        public MainWindowViewModel()
        {
            PokemonTypes = new ObservableCollection<PokemonType>();
            Moves = new ObservableCollection<MoveData>();
            Pokemons = new ObservableCollection<PokemonData>();

            CreateMenus();
        }

        private void CreateMenus()
        {
            Menus = new ObservableCollection<MenuViewModel>();

            var file = new MenuViewModel("_File", null);
            var open = new MenuViewModel("_Open", new RelayCommand(p => Open()));
            var save = new MenuViewModel("_Save", new RelayCommand(p => Save()));
            var saveAs = new MenuViewModel("Save_As", new RelayCommand(p => SaveAs()));
            var close = new MenuViewModel("_Close", new RelayCommand(p => Close(), p => IsDataOpen));
            file.Children.Add(open);
            file.Children.Add(save);
            file.Children.Add(saveAs);
            file.Children.Add(close);

            Menus.Add(file);

        }

        private void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "(*.pgd)|*.pgd";
            if (dialog.ShowDialog() ?? false)
            {
                dataPath = dialog.FileName;
                LoadData();
            }
        }

        private void Save()
        {
            if (IsDataOpen)
                SaveData();
            else
                SaveAs();
        }

        private void SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "(*.pgd)|*.pgd";
            if (dialog.ShowDialog() ?? false)
            {
                dataPath = dialog.FileName;
                SaveData();
            }
        }

        private void Close()
        {
            PokemonTypes.Clear();
            Moves.Clear();
            Pokemons.Clear();
            dataPath = string.Empty;
        }

        private void LoadData()
        {
            using (var stream = new FileStream(dataPath, FileMode.Open))
            {
                var reader = new BinaryReader(stream);
                int count;

                PokemonTypes.Clear();
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    PokemonTypes.Add(PokemonType.FromStream(stream));
                }

                Moves.Clear();
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    Moves.Add(MoveData.FromStream(stream));
                }

                Pokemons.Clear();
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    Pokemons.Add(PokemonData.FromStream(stream));
                }
            }
        }

        private void SaveData()
        {
            using (var stream = new FileStream(dataPath, FileMode.Open))
            {
                var writer = new BinaryWriter(stream);

                writer.Write(PokemonTypes.Count);
                foreach (var type in PokemonTypes)
                    type.Save(stream);

                writer.Write(Moves.Count);
                foreach (var move in Moves)
                    move.Save(stream);

                writer.Write(Pokemons.Count);
                foreach (var pm in Pokemons)
                    pm.Save(stream);
            }
        }

        private bool IsDataOpen
        {
            get
            {
                return !string.IsNullOrEmpty(dataPath);
            }
        }
    }
}
