using DartApp.AppLogic.Contracts;
using DartApp.Domain.Contracts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DartApp.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<IPlayer> AllPlayers { get; set; }

        public Visibility ShowSelectedPlayer => SelectedPlayer == null ? Visibility.Hidden : Visibility.Visible;

        public IPlayer? SelectedPlayer { get; set; }

        public IPlayerStats? PlayerStats { get; set; }

        public MainWindow(IPlayerService playerService)
        {
            InitializeComponent();
            AllPlayers = new ObservableCollection<IPlayer>();
        }

        private void OnPlayerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void OnAddPlayerClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void OnAddGameResultClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnCalculateStats(object sender, RoutedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
