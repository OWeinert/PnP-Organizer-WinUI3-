using Microsoft.UI.Xaml;
using PnPOrganizer.Interfaces;
using PnPOrganizer.Views;
using PnPOrganizer.ViewServices;

namespace PnPOrganizer.Services
{
    public class AppActivationService : IAppActivationService
    {
        private readonly MainWindow _mainWindow;
        private readonly IWindowingService _windowingService;
        private readonly IAppTitleBarService _appTitleBarService;
        private readonly INavigationViewService _navigationViewService;
        private readonly ISettingsService _settingsService;
        private readonly IAppThemeService _appThemeService;
        private readonly ILocalizationService _localizationService;
        private readonly ISoundSettingsService _soundSettingsService;

        public AppActivationService(
            MainWindow mainWindow,
            IWindowingService windowingService,
            IAppTitleBarService appTitleBarService,
            INavigationViewService navigationViewService,
            ISettingsService settingsService,
            IAppThemeService appThemeService,
            ILocalizationService localizationService,
            ISoundSettingsService soundSettingsService)
        {
            _mainWindow = mainWindow;
            _windowingService = windowingService;
            _appTitleBarService = appTitleBarService;
            _navigationViewService = navigationViewService;
            _settingsService = settingsService;
            _appThemeService = appThemeService;
            _localizationService = localizationService;
            _soundSettingsService = soundSettingsService;
        }

        public void Activate(object activationArgs)
        {
            InitializeServices();
            _mainWindow.Activate();
        }

        private void InitializeServices()
        {
            _windowingService.Initialize(_mainWindow);

            _appTitleBarService.Initialize(_mainWindow.TitleBar);

            _navigationViewService.Initialize(_mainWindow.AppNavigationViewControl, _mainWindow.ContentFrameControl);

            if (_mainWindow.Content is FrameworkElement rootElement)
            {
                _appThemeService.Initialize(rootElement);
            }

            _localizationService.Initialize();

            _soundSettingsService.Initialize();
        }
    }
}