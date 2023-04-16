using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using Windows.UI;

namespace PnPOrganizer.Core.Attributes
{
    public partial class Attribute : ObservableObject
    {
        [ObservableProperty]
        private string _displayName = string.Empty;

        [ObservableProperty]
        private int _value = 10;

        [ObservableProperty]
        private int _bonus;

        [ObservableProperty]
        private Color _color;

        public Attribute(string displayName, Color color = default)
        {
            DisplayName = displayName;
            Color = color;
        }

        partial void OnValueChanged(int value)
        {
            var normalizedValue = value * 0.5f - 5;
            Bonus = (int)Math.Floor(normalizedValue);
        }
    }
}
