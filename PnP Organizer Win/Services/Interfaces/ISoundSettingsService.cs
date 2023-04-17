namespace PnPOrganizer.Services.Interfaces
{
    public interface ISoundSettingsService
    {
        public bool SoundsEnabled { get; set; }

        void Initialize();

        bool LoadSoundSettings();

        bool SaveSoundSettings();
    }
}