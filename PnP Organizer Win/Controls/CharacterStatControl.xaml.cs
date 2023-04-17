using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PnPOrganizer.Controls
{
    [INotifyPropertyChanged]
    public sealed partial class CharacterStatControl : UserControl
    {
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(CharacterStatControl), new PropertyMetadata(""));
        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int), typeof(CharacterStatControl), new PropertyMetadata(0));
        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(CharacterStatControl), new PropertyMetadata(0));
        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public static readonly DependencyProperty ValueBonusProperty = DependencyProperty.Register(nameof(ValueBonus), typeof(int), typeof(CharacterStatControl), new PropertyMetadata(0));
        public int ValueBonus
        {
            get => (int)GetValue(ValueBonusProperty);
            set => SetValue(ValueBonusProperty, value);
        }

        public static readonly DependencyProperty NumberBoxEnabledProperty = DependencyProperty.Register(nameof(NumberBoxEnabled), typeof(bool), typeof(CharacterStatControl), 
            new PropertyMetadata(true, new PropertyChangedCallback(OnNumberBoxEnabledChanged)));
        public bool NumberBoxEnabled
        {
            get => (bool)GetValue(NumberBoxEnabledProperty);
            set => SetValue(NumberBoxEnabledProperty, value);
        }

        private static event EventHandler? _numberBoxEnabledChanged;

        [ObservableProperty]
        private Thickness _maxValueBoxMargin = new(70, 0, 0, 0);
        [ObservableProperty]
        private int _maxValueBoxWidth = 64;

        public CharacterStatControl()
        {
            this.InitializeComponent();
            _numberBoxEnabledChanged += CharacterStatControl__numberBoxEnabledChanged;
        }

        private static void OnNumberBoxEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _numberBoxEnabledChanged?.Invoke(NumberBoxEnabledProperty, new EventArgs());
        }

        private void CharacterStatControl__numberBoxEnabledChanged(object? sender, EventArgs e)
        {
            if (NumberBoxEnabled)
            {
                MaxValueBoxWidth = 64;
                MaxValueBoxMargin = new Thickness(70, 0, 0, 0);
            }
            else
            {
                MaxValueBoxWidth = 134;
                MaxValueBoxMargin = new Thickness(0, 0, 0, 0);
            }
                
        }
    }
}
