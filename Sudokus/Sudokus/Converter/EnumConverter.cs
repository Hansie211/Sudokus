using Sudokus.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Sudokus.Converter {

    public class EnumConverter : IValueConverter {

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            switch ( (Difficulty)value ) {
                case Difficulty.Easy:
                    
                    return "Easy";
                case Difficulty.Medium:
                    
                    return "Medium";

                case Difficulty.Hard:

                    return "Hard";

                default:
                    return string.Empty;
            }
        }

        public static object ConvertBack( string value, Type enumType ) {

            switch ( value ) {
                case "Easy":

                    return Difficulty.Easy;
                case "Medium":

                    return Difficulty.Medium;

                case "Hard":

                    return Difficulty.Hard;

                default:
                    return Difficulty.Easy;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
    
            return ConvertBack( (string)value, null );
        }
    }
}
