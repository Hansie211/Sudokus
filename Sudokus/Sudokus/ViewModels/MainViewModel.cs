using SudokuData;
using Sudokus.Logic;
using Sudokus.Models;
using Sudokus.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sudokus.ViewModels {

    public class MainViewModel : ViewModel {

        public SudokuModel Sudoku { get; }
        private int _ActiveSelection = 1;
        public int ActiveSelection { 
            get => _ActiveSelection;
            set { 
                SetProperty( ref _ActiveSelection, value ); 
            }
        }

        public ICommand OnChangeSelection { get; }

        #region outcommented
        //static readonly RawSudoku[] RawSudokus = new RawSudoku[50];

        //static MainViewModel() {

        //    string[] textData = Resources.sudokus.Split( '\n' );

        //    for ( int i = 0; i < RawSudokus.Length; i++ ) {
        //        RawSudokus[ i ] = new RawSudoku( Sudoku.BOARDSIZE );
        //    }

        //    int index = 0;
        //    for ( int i = 1; i < textData.Length; i++ ) {

        //        if ( i % 10 == 0 ) {

        //            index++;
        //            continue;
        //        }

        //        int y = (i % 10) - 1;

        //        for ( int x = 0; x < 9; x++ ) {

        //            char c  = textData[i][x];
        //            int v   = int.Parse( c.ToString() );

        //            RawSudokus[ index ][ x, y ] = v;
        //        }
        //    }
        //}

        //public void TestMethod1() {

        //    int count = 0;

        //    for ( int i = 0; i < RawSudokus.Length; i++ ) {

        //        try {
        //            Sudoku data = new Sudoku( RawSudokus[i] );
        //            SudokuSolver solver = SudokuSolver.Setup( data );

        //            try {

        //                bool x = solver.Solve( false );
        //                if ( !x ) {
        //                    count++;
        //                }

        //            } catch ( Exception exp ) {

        //                string s = exp.ToString();
        //                Console.WriteLine( s );
        //                throw;
        //            }

        //        } catch ( Exception exp ) {

        //            string s = exp.ToString();
        //            Console.WriteLine( s );
        //            throw;
        //        }
        //    }

        //    Debug.Assert( count == 0, $"Count should be '0', but is { count }." );
        //}
        #endregion

        public MainViewModel() {

            Sudoku = new SudokuModel();
            OnChangeSelection = new Command( ChangeSelection );
        }

        private void ChangeSelection( object parameter ) {

            ActiveSelection = int.Parse((string)parameter);
        }
    }
}
