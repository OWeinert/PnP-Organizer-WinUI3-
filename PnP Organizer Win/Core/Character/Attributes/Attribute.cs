using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Windows.UI;

namespace PnPOrganizer.Core.Attributes
{
    /// <summary>
    /// Character Attribute consisting of a Value and a Bonus calculated from the Value.
    /// Also contains Name and Color information for the UI.
    /// Has nothing to do with C# Attributes
    /// </summary>
    public partial class Attribute : ObservableObject
    {
        /// <summary>
        /// Name shown in the UI
        /// </summary>
        [ObservableProperty]
        private string _displayName = string.Empty;

        /// <summary>
        /// Current Value
        /// </summary>
        [ObservableProperty]
        private int _value = 10;

        /// <summary>
        /// Current Bonus depending on the Value
        /// </summary>
        [ObservableProperty]
        private int _bonus;

        /// <summary>
        /// Color for the Foreground
        /// </summary>
        [ObservableProperty]
        private Color _color;

        /// <summary>
        /// 2 character long abbreviation of the DisplayName
        /// </summary>
        public string ShortName { get; }

        public Attribute(string displayName, Color color = default)
        {
            DisplayName = displayName;
            Color = color;
            ShortName = DisplayName.Substring(0, 2);
        }

        partial void OnValueChanged(int value)
        {
            var normalizedValue = value * 0.5f - 5;
            Bonus = (int)Math.Floor(normalizedValue);
        }
    }
}
