using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudokus;
using Sudokus.Logic;
using UnitTestProject.Properties;

namespace UnitTestProject {

    [TestClass]
    public class SudokuTests {

        private static readonly int[][,] Sudokus = new int[50][,];

        static SudokuTests() {

            string[] textData = Resources.sudokus.Split( '\n' );

            for( int i = 0; i < Sudokus.Length; i++ ) {
                Sudokus[i] = new int[9,9];
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

                    Sudokus[ index ][ x, y ] = v;
                }
            }
        }

        [TestMethod]
        public void TestMethod1() {

            int count = 0;

            for( int i = 0; i < Sudokus.Length; i++ ) {

                if ( i == 1 ) {
                    Console.WriteLine();
                }

                SudokuData data = new SudokuData( Sudokus[i] );
                SudokuSolver solver = SudokuSolver.Setup( data );
                bool x = solver.Solve();

                if ( !x ) {
                    count++;
                }
            }

            Debug.Assert( count == 0, $"Count should be '0', but is { count }." );
        }
    }
}
