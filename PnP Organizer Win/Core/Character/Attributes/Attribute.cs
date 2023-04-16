using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;

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

        public Attribute(string displayName)
        {
            DisplayName = displayName;
        }

        partial void OnValueChanged(int value)
        {
            var normalizedValue = value * 0.5f - 5;
            Bonus = (int)Math.Floor(normalizedValue);
        }
    }
}
