using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuData.Core {

    public class Cell {

        private byte RawValue;

        public int Value { 
            get => (int)RawValue; 
            set { RawValue = (byte)value; } 
        }

        public bool HasValue { get => Value > 0; }

        public Row Row { get; set; }
        public Column Column { get; set; }
        public Square Square { get; set; }

        public int X { get => Column.Index; }
        public int Y { get => Row.Index; }

        public List<byte> Notes { get; }

        public IEnumerable<BoardStructure> Structures {
            get {
                yield return Row;
                yield return Column;
                yield return Square;
            }
        }

        public Cell( int value ) {

            Value = value;
            Notes = new List<byte>( Sudoku.BOARDSIZE );
        }

        public static implicit operator int( Cell cell ) {

            return cell.Value;
        }

        public static implicit operator byte( Cell cell ) {

            return cell.RawValue;
        }

        public Cell Clone() {

            Cell result = new Cell( this.Value );
            foreach( var note in Notes ) {

                result.Notes.Add( note );
            }

            return result;
        }
    }
}
