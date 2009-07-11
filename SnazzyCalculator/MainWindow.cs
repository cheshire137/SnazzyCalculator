using Gtk;

public partial class MainWindow: Gtk.Window
{	
    public const int WIN_WIDTH = 640;
    public const int WIN_HEIGHT = 480;
    public const string TITLE = "Snazzy Calculator";
    
    public MainWindow(): base (Gtk.WindowType.Toplevel)
    {
        Build();
    }
    
    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}