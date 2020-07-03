using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudokus.Extensions {

    public static class CollectionsExtensions {

        private static readonly Random Random = new Random();

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

        public static void Shuffle<T>( this List<T> list ) {

            for ( int i = 0; i < list.Count; i++ ) {

                int j = Random.Next( 0, list.Count );

                T temp = list[i];
                list[ i ] = list[ j ];
                list[ j ] = temp;
            }
        }

        public static bool EqualRange<T>( this IList<T> list, IEnumerable<T> range ) {

            if ( list.Count() != range.Count() ) {
                return false;
            }

            foreach( T entry in range ) {

                if ( list.Contains(entry) ) {

                    continue;
                }

                return false;
            }

            return true;
        }
    }
}
