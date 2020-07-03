using SudokuData;
using SudokuData.Core;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sudokus.Models {

    public abstract class CellList : IEnumerable<CellModel> {

        public IList<CellModel> Cells { get; }
        public SudokuModel Parent { get; }
        public int Index { get; }

        public CellList( SudokuModel sudoku, int index ) {
            
            Parent  = sudoku;
            Index   = index;
            Cells   = new BindingList<CellModel>();

            for( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                Cells.Add( null );
            }

            InitCells();
        }

        public IEnumerator<CellModel> GetEnumerator() {

            return Cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();
        }

        public bool Contains( int num ) {

            return Cells.Any( o => o.Value == num );
        }

        protected abstract void InitCells();
    }

    public class Column : CellList {

        public Column( SudokuModel sudoku, int index ) : base( sudoku, index ) {
        }

        protected override void InitCells() {

            for ( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                Cells[ i ] = Parent.Cells[ Index, i ];
                Cells[ i ].Column = this;
            }
        }
    }

    public class Row : CellList {

        public Row( SudokuModel sudoku, int index ) : base( sudoku, index ) {
        }

        protected override void InitCells() {

            for ( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                Cells[ i ] = Parent.Cells[ i, Index ];
                Cells[ i ].Row = this;
            }
        }
    }

    public class Square : CellList {

        public Square( SudokuModel sudoku, int index ) : base( sudoku, index ) {
        }

        protected override void InitCells() {

            int squareX = Index % 3;
            int squareY = Index / 3;

            for ( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                int cellX = i % 3 + squareX * 3;
                int cellY = i / 3 + squareY * 3;

                Cells[ i ] = Parent.Cells[ cellX, cellY ];
                Cells[ i ].Square = this;
            }
        }
    }
}
