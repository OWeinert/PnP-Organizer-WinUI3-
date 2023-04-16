using PnPOrganizer.Core.Character;
using System.Collections.Generic;

namespace PnPOrganizer.Services.Interfaces
{
    public interface IPearlService : ISaveData
    {
        public IReadOnlyDictionary<PearlType, Pearl> Pearls { get; }
    }
}
