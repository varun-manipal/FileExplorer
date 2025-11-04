using System.ComponentModel;
using FileExplorer.ViewModel;

namespace FileExplorer;

public partial class MainPage : ContentPage,
    INotifyPropertyChanged
{
    private double leftNavInitialWidth = 260;
    private readonly double minLeftNavWidth = 150;
    private readonly double maxLeftNavWidth = 500;

    private FileExplorerViewModel fileExplorerViewModel = new FileExplorerViewModel();
    
    public MainPage()
    {
        InitializeComponent();
        BindingContext = fileExplorerViewModel;
        this.LeftNavColumn.Width = leftNavInitialWidth;
    }
    
    private void OnSplitterPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                var newWidth = leftNavInitialWidth + e.TotalX;
                newWidth = Math.Max(minLeftNavWidth, Math.Min(maxLeftNavWidth, newWidth));
                this.LeftNavColumn.Width = newWidth;
                break;

            case GestureStatus.Completed:
                leftNavInitialWidth = LeftNavColumn.Width.Value;
                break;
        }
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Ensure we store the initial width
        leftNavInitialWidth = LeftNavColumn.Width.Value;

        // Set platform-native resize cursor for the splitter
        SetSplitterCursor();
    }

    private void OnSplitterPointerEntered(object sender, PointerEventArgs e)
    {
        SplitterHandle.BackgroundColor = new Color(0, 0, 0, 06);
    }

    private void OnSplitterPointerExited(object sender, PointerEventArgs e)
    {
        SplitterHandle.BackgroundColor = Colors.Transparent;
    }

    private void SetSplitterCursor()
    {
        if (SplitterHandle?.Handler == null) return;

    }
}