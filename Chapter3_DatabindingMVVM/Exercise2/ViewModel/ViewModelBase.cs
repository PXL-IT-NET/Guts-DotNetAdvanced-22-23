using System.ComponentModel;

namespace Exercise2.ViewModel;

public abstract class ViewModelBase : IViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public abstract void Load();
}