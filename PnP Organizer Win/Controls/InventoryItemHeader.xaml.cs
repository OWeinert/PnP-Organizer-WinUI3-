using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PnPOrganizer.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PnPOrganizer.Controls
{
    public sealed partial class InventoryItemHeader : UserControl
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

        public InventoryItemHeader()
        {
            InitializeComponent();
        }
    }
}
