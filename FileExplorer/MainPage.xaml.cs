using System.ComponentModel;

namespace FileExplorer;

public partial class MainPage : ContentPage,
    INotifyPropertyChanged
{
    private double _leftNavInitialWidth = 260;
    private double _minLeftNavWidth = 150;
    private double _maxLeftNavWidth = 500;
    
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        this.LeftNavColumn.Width = _leftNavInitialWidth;
    }
    
    private void OnSplitterPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                var newWidth = _leftNavInitialWidth + e.TotalX;
                newWidth = Math.Max(_minLeftNavWidth, Math.Min(_maxLeftNavWidth, newWidth));
                this.LeftNavColumn.Width = newWidth;
                break;

            case GestureStatus.Completed:
                _leftNavInitialWidth = LeftNavColumn.Width.Value;
                break;
        }
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Ensure we store the initial width
        _leftNavInitialWidth = LeftNavColumn.Width.Value;

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

#if WINDOWS
    try
    {
        var fe = SplitterHandle.Handler.PlatformView as Microsoft.UI.Xaml.FrameworkElement;
        if (fe != null)
        {
            fe.ProtectedCursor = Microsoft.UI.Input.InputSystemCursor.Create(
                Microsoft.UI.Input.InputSystemCursorShape.SizeWestEast
            );
        }
    }
    catch { /* ignore */ }
#endif
//
// #if MACCATALYST
//         try
//         {
//             var uiView = SplitterHandle.Handler.PlatformView as UIKit.UIView;
//             if (uiView != null)
//             {
//                 // Add a pointer interaction to indicate resize (horizontal)
//                 var interaction = new UIKit.UIPointerInteraction(new HorizontalResizePointerDelegate());
//                 uiView.AddInteraction(interaction);
//             }
//         }
//         catch { /* ignore */ }
// #endif
    }
//
// #if MACCATALYST
//     class HorizontalResizePointerDelegate : UIKit.UIPointerInteractionDelegate
//     {
//         public override UIKit.UIPointerStyle GetStyle(UIKit.UIPointerInteraction interaction, UIKit.UIPointerRegion region)
//         {
//             // Use a horizontal resize style; Apple doesn't expose a direct "ew-resize",
//             // but a beam with shape hints reads as a resize affordance.
//             var targetedPreview = new UIKit.UITargetedPreview(interaction.View);
//             return UIKit.UIPointerStyle.Create(targetedPreview);
//         }
//     }
// #endif
}