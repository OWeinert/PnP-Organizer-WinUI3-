using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Windows.UI;

namespace PnPOrganizer.Core.Character
{
    public sealed partial class Pearl : ObservableObject
    {
        [ObservableProperty]
        private string _displayName = string.Empty;

        [ObservableProperty]
        private int _amount;

        [ObservableProperty]
        private byte _form;

        [ObservableProperty]
        private Color _color;

        [ObservableProperty]
        private bool _isUsed;

        public Pearl(string displayName, Color color = default)
        {
            DisplayName = displayName;
            Color = color;
        }

        partial void OnFormChanged(byte value)
        {
            // Form can only be 1 or 2
            if(value < 1 || value > 2)
                Form = Math.Clamp(value, (byte)1, (byte)2);
        }

        partial void OnAmountChanged(int value) => IsUsed = Amount > 0;
    }

    public enum PearlType
    {
        Fire,
        Earth,
        Metal,
        Air,
        Water,
        Wood
    }
}
