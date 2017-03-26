namespace HttpLogger.SampleApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new SampleApp.App());
        }
    }
}