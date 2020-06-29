using SudokuData;
using Sudokus.Logic;
using Sudokus.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sudokus.ViewModels {

    public class MainViewModel : ViewModel {

        static readonly RawSudoku[] RawSudokus = new RawSudoku[50];

        static MainViewModel() {

            string[] textData = Resources.sudokus.Split( '\n' );

            for ( int i = 0; i < RawSudokus.Length; i++ ) {
                RawSudokus[ i ] = new RawSudoku( Sudoku.BOARDSIZE );
            }

            int index = 0;
            for ( int i = 1; i < textData.Length; i++ ) {

                if ( i % 10 == 0 ) {

                    index++;
                    continue;
                }

                int y = (i % 10) - 1;

                for ( int x = 0; x < 9; x++ ) {

                    char c  = textData[i][x];
                    int v   = int.Parse( c.ToString() );

                    RawSudokus[ index ][ x, y ] = v;
                }
            }
        }

        public void TestMethod1() {

            int count = 0;

            for ( int i = 0; i < RawSudokus.Length; i++ ) {

                try {
                    Sudoku data = new Sudoku( RawSudokus[i] );
                    SudokuSolver solver = SudokuSolver.Setup( data );

                    try {

                        bool x = solver.Solve( true );
                        if ( !x ) {
                            count++;
                        }

                    } catch ( Exception exp ) {

                        string s = exp.ToString();
                        Console.WriteLine( s );
                        throw;
                    }

                } catch ( Exception exp ) {

                    string s = exp.ToString();
                    Console.WriteLine( s );
                    throw;
                }
            }

            Debug.Assert( count == 0, $"Count should be '0', but is { count }." );
        }

        public MainViewModel() {

            TestMethod1();
        }
    }
}
