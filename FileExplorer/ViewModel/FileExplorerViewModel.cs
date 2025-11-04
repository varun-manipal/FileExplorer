using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileExplorer.ViewModel;

public class FileExplorerViewModel: INotifyPropertyChanged
{
    private string currentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public string CurrentPath
    {
        get => currentPath;
        set { currentPath = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}