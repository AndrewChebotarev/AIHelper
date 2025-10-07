namespace AIHelper
{
    public partial class App : Application
    {
        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
                singleViewPlatform.MainView = new MainWindow { DataContext = new MainWindowViewModel() };

            base.OnFrameworkInitializationCompleted();
        }
    }
}
