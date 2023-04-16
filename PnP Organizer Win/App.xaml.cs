using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using PnPorganizer.Core.Character;
using PnPOrganizer.Core.Character.SkillSystem;
using PnPOrganizer.Interfaces;
using PnPOrganizer.Services;
using PnPOrganizer.Services.Interfaces;
using PnPOrganizer.ViewModels;
using PnPOrganizer.ViewModels.Maps;
using PnPOrganizer.Views;
using PnPOrganizer.ViewServices;
using PnPOrganizer.ViewServices.Interfaces;
using Serilog;

namespace PnPOrganizer
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            InitializeComponent();
            UnhandledException += App_UnhandledException;
            _host = BuildHost();
            Ioc.Default.ConfigureServices(_host.Services);
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception, "An unhandled exception occured!");
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            IAppActivationService appWindowService = Ioc.Default.GetRequiredService<IAppActivationService>();
            appWindowService.Activate(args);
        }

        private static IHost BuildHost() => Host.CreateDefaultBuilder()
            .ConfigureLogging((context, logger) =>
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    .WriteTo.File($"\\logs\\log_.txt", rollingInterval: RollingInterval.Minute, fileSizeLimitBytes: 52428800)
                    .CreateLogger();

                logger.AddSerilog(dispose: true);
            })
            .ConfigureServices((context, services) =>
            {
                _ = services
                    .AddSingleton<ISettingsService, LocalSettingsService>()
                    .AddSingleton<IAppThemeService, AppThemeService>()
                    .AddSingleton<ILocalizationService, LocalizationService>()
                    .AddSingleton<ISoundSettingsService, SoundSettingsService>()
                    .AddSingleton<IWindowingService, WindowingService>()
                    .AddSingleton<INavigationViewService, NavigationViewService>()
                    .AddSingleton<IMapService, MapService>()
                    .AddSingleton<ISkillsService, SkillsService>()
                    .AddSingleton<IInventoryService, InventoryService>()
                    .AddSingleton<IPearlService, PearlService>()
                    .AddSingleton<IAttributeService, AttributeService>()
                    .AddSingleton<ISaveDataService, SaveDataService>()
                    .AddSingleton<ISaveFileInfoService, SaveFileInfoService>()
                    .AddSingleton<IAppActivationService, AppActivationService>()
                    .AddSingleton<MainWindowViewModel>()
                    .AddSingleton<SettingsPageViewModel>()
                    .AddSingleton<MainPageViewModel>()
                    .AddSingleton<CharacterPageViewModel>()
                    .AddSingleton<InventoryPageViewModel>()
                    .AddSingleton<SkillsPageViewModel>()
                    .AddSingleton<RulesPageViewModel>()
                    .AddSingleton<MapPageAleraViewModel>()
                    .AddSingleton<MapPageViewModel>()
                    .AddSingleton<MainWindow>();
            })
            .Build();
    }
}