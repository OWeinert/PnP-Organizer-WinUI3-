﻿using Microsoft.UI.Xaml.Data;
using System;

namespace PnPOrganizer.Converters
{
    /// <summary>
    /// Used in the InventoryPage. Subtracts the input value by 30
    /// </summary>
    public class InventoryHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.GetType() != typeof(double))
                throw new ArgumentException("value is not double!", nameof(value));

            var dValue = (double)value;
            var result = dValue - 30.0;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("TODO InventoryHeightConverter::ConvertBack");
        }
    }
}
