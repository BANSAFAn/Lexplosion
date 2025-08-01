using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Lexplosion.WPF.NewInterface.Core.Converters
{
    public sealed class IsLastItemConverter : ConverterBase<IsLastItemConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DependencyObject border)
            {
                if (ItemsControl.ItemsControlFromItemContainer(border) is ItemsControl itemsControl)
                {
                    var item = itemsControl.ItemContainerGenerator.ItemFromContainer(border);
                    var index = itemsControl.Items.IndexOf(item);
                    return index == itemsControl.Items.Count - 1;
                }
            }
            return false;
        }
    }
}
