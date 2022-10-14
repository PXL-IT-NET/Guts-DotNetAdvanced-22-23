using System.ComponentModel;

namespace Exercise2.ViewModel;

public interface IViewModel : INotifyPropertyChanged
{
    void Load();
}