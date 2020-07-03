using SudokuData;
using SudokuData.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuData.Logic.Techniques {

    public class SingleOptionTechnique : Technique {

        public SingleOptionTechnique( SudokuSolver solver ) : base( solver ) {
        }

        private void ScanSingleOptions( BoardStructure structure ) {

            for ( int num = 0 + 1; num < Sudoku.BOARDSIZE + 1; num++ ) {

                Cell possible = null;

                if ( structure.Contains( num ) ) {
                    continue;
                }

                foreach ( var cell in structure.Cells ) {

                    if ( cell.HasValue ) {
                        continue;
                    }

                    if ( !cell.Notes.Contains( num ) ) {

                        continue;
                    }

                    if ( possible != null ) {

                        possible = null;
                        break;
                    }

                    possible = cell;
                }

                if ( possible == null ) {
                    continue;
                }

                possible.Notes.Clear();
                possible.Notes.Add( num );
            }
        }

        public override void ReduceOptions() {

            for ( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                ScanSingleOptions( Solver.Data.Rows[ i ] );
                ScanSingleOptions( Solver.Data.Columns[ i ] );
                ScanSingleOptions( Solver.Data.Squares[ i ] );
            }
        }
    }
}
