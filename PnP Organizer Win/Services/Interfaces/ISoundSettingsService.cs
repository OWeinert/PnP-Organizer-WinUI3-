using Microsoft.UI.Xaml;

namespace PnPOrganizer.ViewServices
{
    public interface ISoundSettingsService
    {
        public bool SoundsEnabled { get; set; }

        void Initialize();

        bool LoadSoundSettings();

        bool SaveSoundSettings();
    }
}