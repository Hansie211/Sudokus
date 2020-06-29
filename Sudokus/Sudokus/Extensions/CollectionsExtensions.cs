using System;
using System.Collections.Generic;
using System.Text;

namespace Sudokus.Extensions {

    public static class CollectionsExtensions {

        public static int Count<T>( this ReadOnlySpan<T> span, T item ) {

            int count = 0;
            foreach ( T entry in span ) {

                if ( EqualityComparer<T>.Default.Equals( entry, item ) ) {

                    count++;
                }
            }

            return count;
        }

        public static int Count<T, K>( this ReadOnlySpan<T> span, K item, Func<T, K> convert ) {

            int count = 0;
            foreach ( T entry in span ) {

                K conv = convert(entry);
                if ( EqualityComparer<K>.Default.Equals( conv, item ) ) {

                    count++;
                }
            }

            return count;
        }

        public static bool Contains<T>( this ReadOnlySpan<T> span, T item ) {

            foreach ( T entry in span ) {

                if ( EqualityComparer<T>.Default.Equals( entry, item ) ) {

                    return true;
                }
            }

            return false;
        }

        public static bool Contains<T, K>( this ReadOnlySpan<T> span, K item, Func<T, K> convert ) {

            foreach ( T entry in span ) {

                K conv = convert(entry);
                if ( EqualityComparer<K>.Default.Equals( conv, item ) ) {

                    return true;
                }
            }

            return false;
        }

        public static List<T> Clone<T>( this List<T> list ) {

            return new List<T>( list );
        }
    }
}
