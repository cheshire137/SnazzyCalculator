using Gtk;
using Gui;

public partial class MainWindow: Window
{
    private static IGui _gui;

    public static void Main(string[] args)
    {
#if OSX
        _gui = new CocoaSharp();
#elif LINUX
        _gui = new Gui.GtkSharp();
#endif
        _gui.PopulateWindow();
        _gui.ShowWindow();
    }

    public MainWindow(): base (WindowType.Toplevel)
    {
        Build();
    }
    
    protected void onDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}