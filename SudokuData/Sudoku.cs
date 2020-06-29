using SudokuData.core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SudokuData {

    public class Sudoku : IEnumerable<Cell> {

        public const int BOARDSIZE  = 9;
        public const int SQUARESIZE = 3;

        public Cell[,] Data { get; }

        public Row[] Rows { get; }
        public Column[] Columns { get; }
        public Square[] Squares { get; }

        public Cell this[ int x, int y ] {

            get => Data[ x, y ];
        }

        public Cell this[ Point point ] {

            get => this[ point.X, point.Y ];
        }

        private Sudoku() {

            Data    = new Cell[ BOARDSIZE, BOARDSIZE ];

            Rows    = new Row[ BOARDSIZE ];
            Columns = new Column[ BOARDSIZE ];
            Squares = new Square[ BOARDSIZE ];
        }

        private void InitStructures() {

            for ( int i = 0; i < BOARDSIZE; i++ ) {

                Rows[ i ]    = new Row( this, i );
                Columns[ i ] = new Column( this, i );
                Squares[ i ] = new Square( this, i );
            }
        }

        public Sudoku( RawSudoku rawData ) : this() {

            for ( int x = 0; x < Data.GetLength( 0 ); x++ ) {
                for ( int y = 0; y < Data.GetLength( 1 ); y++ ) {

                    Data[ x, y ] = new Cell( rawData[ x, y ] );
                }
            }

            InitStructures();
        }

        public static Sudoku Clone( Sudoku subject ) {

            Sudoku result = new Sudoku();

            for ( int x = 0; x < result.Data.GetLength( 0 ); x++ ) {
                for ( int y = 0; y < result.Data.GetLength( 1 ); y++ ) {

                    result.Data[ x, y ] = subject.Data[ x, y ].Clone();
                }
            }

            result.InitStructures();

            return result;
        }

        public static Sudoku GenerateEmpty() {

            Sudoku result = new Sudoku();
            for ( int x = 0; x < result.Data.GetLength( 0 ); x++ ) {
                for ( int y = 0; y < result.Data.GetLength( 1 ); y++ ) {

                    result.Data[ x, y ] = new Cell( 0 );
                }
            }

            result.InitStructures();

            return result;
        }

        public Sudoku Clone() {

            return Clone( this );
        }

        public bool IsCorrect() {

            for ( int i = 0; i < BOARDSIZE; i++ ) {

                if ( !Rows[ i ].IsCorrect() ) {
                    return false;
                }

                if ( !Columns[ i ].IsCorrect() ) {
                    return false;
                }

                if ( !Squares[ i ].IsCorrect() ) {
                    return false;
                }
            }

            return true;
        }

        public IEnumerator<Cell> GetEnumerator() {

            foreach( var cell in Data ) {
                yield return cell;
            }

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();
        }
    }
}
