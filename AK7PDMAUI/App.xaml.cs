using AK7PDMAUI.ViewModels;

namespace AK7PDMAUI;

public partial class App : Application
{
	public GlobalViewModel GlobalViewModel { get; set; } = new();

	public Window CurrentWindow { get; set; }

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

		window.Height = 500;
		window.Width = 700;
		CurrentWindow = window;
		return window;
    }

	public void OpenMainWindow(int height, int width)
	{
		Window window = new();
		window.Height = height;
		window.Width = width;
		this.CloseWindow(CurrentWindow);
		this.OpenWindow(window);
	}
}
