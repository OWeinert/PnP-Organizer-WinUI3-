using Microsoft.UI;
using PnPOrganizer.Core.Character;
using PnPOrganizer.Services.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using Windows.ApplicationModel.Resources;

namespace PnPOrganizer.Services
{
    public class PearlService : IPearlService
    {

        private readonly IReadOnlyDictionary<PearlType, Pearl> _pearls;
        public IReadOnlyDictionary<PearlType, Pearl> Pearls => _pearls;

        public PearlService()
        {
            var resourceLoader = ResourceLoader.GetForViewIndependentUse();
            _pearls = new Dictionary<PearlType, Pearl>()
            {
                { PearlType.Fire, new Pearl(resourceLoader.GetString("Pearls_Fire"), Colors.OrangeRed) },
                { PearlType.Earth, new Pearl(resourceLoader.GetString("Pearls_Earth"), Colors.SaddleBrown) },
                { PearlType.Metal, new Pearl(resourceLoader.GetString("Pearls_Metal"), Colors.LightSlateGray) },
                { PearlType.Air, new Pearl(resourceLoader.GetString("Pearls_Air"), Colors.White) },
                { PearlType.Water, new Pearl(resourceLoader.GetString("Pearls_Water"), Colors.CornflowerBlue) },
                { PearlType.Wood, new Pearl(resourceLoader.GetString("Pearls_Wood"), Colors.LimeGreen) }
            }.ToImmutableDictionary();
        }

        // HACK to support deprecated save data. Implement save data fixer in the future
        public void LoadFromCharacter(CharacterData data)
        {
            Pearls[PearlType.Fire].Amount = data.Pearls.Fire;
            Pearls[PearlType.Fire].Form = data.Forms[0];
            Pearls[PearlType.Earth].Amount = data.Pearls.Earth;
            Pearls[PearlType.Earth].Form = data.Forms[1];
            Pearls[PearlType.Metal].Amount = data.Pearls.Metal;
            Pearls[PearlType.Metal].Form = data.Forms[2];
            Pearls[PearlType.Air].Amount = data.Pearls.Air;
            Pearls[PearlType.Air].Form = data.Forms[3];
            Pearls[PearlType.Water].Amount = data.Pearls.Water;
            Pearls[PearlType.Water].Form = data.Forms[4];
            Pearls[PearlType.Wood].Amount = data.Pearls.Wood;
            Pearls[PearlType.Wood].Form = data.Forms[5];
        }
        // HACK to support deprecated save data. Implement save data fixer in the future
        public void SaveToCharacter(ref CharacterData data)
        {
            data.Pearls = new CharacterPearls()
            {
                Fire = Pearls[PearlType.Fire].Amount,
                Earth = Pearls[PearlType.Earth].Amount,
                Metal = Pearls[PearlType.Metal].Amount,
                Air = Pearls[PearlType.Air].Amount,
                Water = Pearls[PearlType.Water].Amount,
                Wood = Pearls[PearlType.Wood].Amount
            };

            data.Forms[0] = Pearls[PearlType.Fire].Form;
            data.Forms[1] = Pearls[PearlType.Earth].Form;
            data.Forms[2] = Pearls[PearlType.Metal].Form;
            data.Forms[3] = Pearls[PearlType.Air].Form;
            data.Forms[4] = Pearls[PearlType.Water].Form;
            data.Forms[5] = Pearls[PearlType.Wood].Form;
        }
    }
}
