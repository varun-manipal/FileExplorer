using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FileExplorer.Model;

namespace FileExplorer.ViewModel;

public sealed class FileExplorerViewModel: INotifyPropertyChanged
{
    private string currentPath;
    
    private FileItem? selectedItem;
    public FileItem? SelectedItem
    {
        get => selectedItem;
        set
        {
            if (selectedItem == value) return;
            selectedItem = value;
            OnPropertyChanged();
            if (value?.IsDirectory == true)
            {
                NavigateTo(value.FullPath);
            }
        }
    }

    public FileExplorerViewModel()
    {
        this.CurrentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public string CurrentPath
    {
        get => currentPath;
        set { 
            currentPath = value; 
            this.LoadDirectory(currentPath);
            OnPropertyChanged(); 
        }
    }
    
    public ObservableCollection<FileItem> FileList { get; } = new();
    
    private void LoadDirectory(string path)
    {
        try
        {
            if (!Directory.Exists(path)) return;
            FileList.Clear();

            foreach (var dir in SafeEnum(() => Directory.EnumerateDirectories(path)))
            {
                var di = new DirectoryInfo(dir);
                FileList.Add(new FileItem
                {
                    Name = di.Name,
                    FullPath = di.FullName,
                    IsDirectory = true,
                    DateModified = di.LastWriteTime,
                    Type = "File folder",
                    SizeBytes = 0,
                    Icon = "📁"
                });
            }

            foreach (var file in SafeEnum(() => Directory.EnumerateFiles(path)))
            {
                var fi = new FileInfo(file);
                FileList.Add(new FileItem
                {
                    Name = fi.Name,
                    FullPath = fi.FullName,
                    IsDirectory = false,
                    DateModified = fi.LastWriteTime,
                    Type = fi.Extension.TrimStart('.').ToUpperInvariant() + " File",
                    SizeBytes = fi.Length,
                    Icon = "📄"
                });
            }

            //ApplySort();
        }
        catch
        {
        }
    }
    
    private void NavigateTo(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path)) return;
        if (!string.Equals(CurrentPath, path, StringComparison.OrdinalIgnoreCase))
        {
            // _back.Push(CurrentPath);
            // _forward.Clear();
            CurrentPath = path;
            LoadDirectory(CurrentPath);
        }
        // RaiseNavCanExecChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private static IEnumerable<string> SafeEnum(Func<IEnumerable<string>> f)
    {
        try { return f(); } catch { return []; }
    }
}