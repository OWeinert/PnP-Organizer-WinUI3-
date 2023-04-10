using Microsoft.UI.Xaml;
using PnPOrganizer.Interfaces;

namespace PnPOrganizer.ViewServices
{
    public class SoundSettingsService : ISoundSettingsService
    {
        public bool SoundsEnabled { get; set; }
        public string SettingsName { get; set; }
        public string SoundsEnabledSettingsKey { get; } = "SoundsEnabled";

        private readonly ISettingsService _settingsService;

        public SoundSettingsService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            SettingsName = $"{GetType().Namespace}.{GetType().Name}";
        }

        public void Initialize()
        {
            ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
        }

        public bool SaveSoundSettings() => _settingsService.TrySave(SettingsName, SoundsEnabledSettingsKey, SoundsEnabled);

        public bool LoadSoundSettings() 
        {
            if (_settingsService.TryLoad(SettingsName, SoundsEnabledSettingsKey, out bool soundsEnabled) is true)
                SoundsEnabled = soundsEnabled;
            return SoundsEnabled;
        }
    }
}
