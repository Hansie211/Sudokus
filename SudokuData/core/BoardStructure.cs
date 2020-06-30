using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuData.Core {

    public abstract class BoardStructure {

        public Cell[] Cells { get; }
        public Sudoku Sudoku { get; }
        public int Index { get; }

        public BoardStructure( Sudoku sudoku, int index ) {

            Cells  = new Cell[ Sudoku.BOARDSIZE ];
            Sudoku = sudoku;
            Index  = index;

            InitCells();
        }

        protected abstract void InitCells();

        public bool Contains( int num ) {

            foreach ( var cell in Cells ) {

                if ( cell.Value != num ) {
                    continue;
                }

                return true;
            }

            return false;
        }

        public bool IsCorrect() {

            if ( Contains( 0 ) ) {
                return false;
            }

            for ( int num = 0 + 1; num < Sudoku.BOARDSIZE + 1; num++ ) {

                if ( !Contains( num ) ) {
                    return false;
                }
            }

            return true;
        }
    }

    public class Row : BoardStructure {

        public Row( Sudoku sudoku, int index ) : base( sudoku, index ) {
        }

        protected override void InitCells() {

            for ( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                Cells[ i ] = Sudoku[ i, Index ];
                Cells[ i ].Row = this;
            }
        }
    }

    public class Column : BoardStructure {

        public Column( Sudoku sudoku, int index ) : base( sudoku, index ) {
        }

        protected override void InitCells() {

            for ( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                Cells[ i ] = Sudoku[ Index, i ];
                Cells[ i ].Column = this;
            }
        }
    }

    public class Square : BoardStructure {

        public Square( Sudoku sudoku, int index ) : base( sudoku, index ) {
        }

        protected override void InitCells() {

            int squareX = Index % 3;
            int squareY = Index / 3;

            for ( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                int cellX = i % 3 + squareX * 3;
                int cellY = i / 3 + squareY * 3;

                Cells[ i ] = Sudoku[ cellX, cellY ];
                Cells[ i ].Square = this;
            }
        }
    }
}
