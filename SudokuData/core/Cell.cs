using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuData.Core {

    public class Cell {

        public int Value { get; set; }
        public bool HasValue { get => Value > 0; }

        public Row Row { get; set; }
        public Column Column { get; set; }
        public Square Square { get; set; }

        public int X { get => Column.Index; }
        public int Y { get => Row.Index; }

        public IEnumerable<BoardStructure> Structures {
            get {
                yield return Row;
                yield return Column;
                yield return Square;
            }
        }

        public Cell( int value ) {

            Value = value;
        }

        public static implicit operator int( Cell cell ) {

            return cell.Value;
        }

        public static int AsInt( Cell cell ) {

            return (int)cell;
        }

        public Cell Clone() {

            return new Cell( this.Value );
        }

        //public static bool IsValueInSpan( ReadOnlySpan<Cell> span, int value ) {

        //    foreach ( var cell in span ) {

        //        if ( cell.Get() == value ) {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}
