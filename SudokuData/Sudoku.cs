using SudokuData.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        private void InitStructures() {

            for ( int i = 0; i < BOARDSIZE; i++ ) {

                Rows[ i ]    = new Row( this, i );
                Columns[ i ] = new Column( this, i );
                Squares[ i ] = new Square( this, i );
            }
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

        public void ResetZero() {

            for ( int x = 0; x < Data.GetLength( 0 ); x++ ) {
                for ( int y = 0; y < Data.GetLength( 1 ); y++ ) {

                    Data[ x, y ].Value = 0;
               }
            }
        }

        public void GenerateRandom() {

            ResetZero();

            Random random = new Random();
            Action<Square> FillRandom = ( square ) => {

                List<int> values = new List<int>( Enumerable.Range( 1, BOARDSIZE ) );

                // Shuffle
                for( int i = 0; i < values.Count; i++ ){

                    int j = random.Next( 0, values.Count );

                    int temp = values[i];
                    values[i] = values[j];
                    values[j] = temp;
                }

                // Fill
                for( int i = 0; i < square.Cells.Length; i++ ){

                    square.Cells[i].Value = values[i];
                }
            };

            FillRandom( Squares[ 0 ] );
            FillRandom( Squares[ 4 ] );
            FillRandom( Squares[ 8 ] );
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
