using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Sudokus.Converter {

    public class CellNoteConverter : IValueConverter {

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            IList<int> notes = (IList<int>)value;
            if ( notes.Count == 0 ) {

                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();

            for( int num = 1; num < 9 + 1; num++ ) {

                string s;
                if ( notes.Contains(num) ) {
                    s = num.ToString();
                } else {
                    s = " ";
                }

                stringBuilder.Append(s);
                if ( num % 3 == 0 ) {
                    
                    stringBuilder.AppendLine();
                } else {

                    stringBuilder.Append(" ");
                }
            }

            return stringBuilder.ToString();
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException();
        }
    }
}
