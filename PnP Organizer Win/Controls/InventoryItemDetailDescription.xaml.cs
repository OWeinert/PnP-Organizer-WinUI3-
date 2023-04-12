using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PnPOrganizer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PnPOrganizer.Controls
{
    public sealed partial class InventoryItemDetailDescription : UserControl
    {

        public static readonly DependencyProperty ItemViewModelProperty = DependencyProperty.Register(
            nameof(ItemViewModel),
            typeof(InventoryItemViewModel),
            typeof(InventoryItemHeader),
            new PropertyMetadata(null)
        );

        public InventoryItemViewModel ItemViewModel
        {
            get => (InventoryItemViewModel)GetValue(ItemViewModelProperty);
            set => SetValue(ItemViewModelProperty, value);
        }

        public InventoryItemDetailDescription()
        {
            InitializeComponent();
        }
    }
}
