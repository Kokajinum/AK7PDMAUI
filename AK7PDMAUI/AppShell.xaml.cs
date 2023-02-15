namespace AK7PDMAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("CreateBookPage", typeof(CreateBookPage));
		Routing.RegisterRoute("EditBookPage", typeof(EditBookPage));	
	}

	//public void EnableAdminShellContent()
	//{
	//	SetFlyoutItemIsVisible(UsersPageShellContent, true);
	//}
}
