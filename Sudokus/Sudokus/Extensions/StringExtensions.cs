using System;
using System.Collections.Generic;
using System.Text;

namespace Sudokus.Extensions {

    public static class StringExtensions {

        public static string Substring( this string that, int startIndex, int length, bool useNegative ) {

            if ( length > -1 || !useNegative ) {

                return that.Substring( startIndex, length );
            }

            return that.Substring( startIndex, Math.Max( ( that.Length - startIndex ) + length, 0 ) );
        }
    }
}
