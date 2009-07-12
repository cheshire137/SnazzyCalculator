namespace SnazzyCalculator
{
	public enum NotificationType
	{
		Error,
		Info
	};
	
	/// <summary>
	/// Interface for a class intended to be used as a GUI.  Such a class
	/// should include a way of showing a window's contents, and displaying
	/// a message.
	/// </summary>
	public interface IGui
	{
	    void DisplayMessage(NotificationType t, string message);
        void PopulateWindow();
	    void ShowWindow();
	}
}
