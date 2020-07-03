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

        public List<int> Notes { get; }

        public IEnumerable<BoardStructure> Structures {
            get {
                yield return Row;
                yield return Column;
                yield return Square;
            }
        }

        public Cell( int value ) {

            Value = value;
            Notes = new List<int>( Sudoku.BOARDSIZE );
        }

        public static implicit operator int( Cell cell ) {

            return cell.Value;
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
