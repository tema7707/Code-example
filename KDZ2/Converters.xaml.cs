using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace KDZ2
{

    public class CellHighlighterConverter : IMultiValueConverter
    {
        public object Convert(
          object[] values,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            if (values[1] is DataRow)
            {
                //Change the background of any
                //cell with 1.0 to light red.
                var cell = (DataGridCell)values[0];
                var row = (DataRow)values[1];
                var columnName = cell.Column.SortMemberPath;


                return new SolidColorBrush(Colors.LightSalmon);
            }
            return SystemColors.AppWorkspaceColor;
        }

        public object[] ConvertBack(
            object value,
            Type[] targetTypes,
            object parameter,
            CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}