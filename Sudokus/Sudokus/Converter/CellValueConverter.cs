using SudokuData;
using Sudokus.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Sudokus.Converter {

    public class CellValueConverter : IValueConverter {

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            if ( value == null ) {
                return "-1";
            }

            try {

                int v = (int)value;
                if ( v == 0 ) {
                    return string.Empty;
                }

                return v.ToString();

            } catch {

                throw;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}
