using SudokuData;
using SudokuData.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuData.Logic.Techniques {

    public class NakedSubsetTechnique : Technique {

        public NakedSubsetTechnique( SudokuSolver solver ) : base( solver ) {
        }

        private void RemovePair( Cell A, Cell B, BoardStructure structure ) {

            IEnumerable<int> pairValues = A.Notes;

            foreach( var cell in structure.Cells ) {

                if ( cell == A || cell == B ) {
                    continue;
                }

                foreach( var option in pairValues ) {

                     cell .Notes.Remove( option );
                }
            }

            foreach( Cell cell in new Cell[] { A, B } ) {

                 cell .Notes.Clear();
                 cell .Notes.AddRange( pairValues );
            }
        }

        private void ScanPairs( BoardStructure structure ) {

            foreach( var cell in structure.Cells ) {

                if ( cell.HasValue ) {

                    continue;
                }

                if ( cell.Notes.Count != 2 ) {

                    continue;
                }


                for( int i = 0 + 1; i < ( Sudoku.BOARDSIZE + 1 ) - 1; i++ ) {

                    int count = 0;
                    Cell A = null, B = null;

                    for ( int j = i + 1; j < Sudoku.BOARDSIZE + 1; j++ ) {

                        if ( !cell.Notes.Contains(i) || !cell.Notes.Contains(j) ) {
                            continue;
                        }

                        count++;

                        if ( count == 1 ) {

                            A = cell;
                        } else if ( count == 2 ) {

                            B = cell;
                        } else {

                            break;
                        }
                    }

                    if ( count == 2 ) {

                        RemovePair( A, B, structure);
                    }
                }
            }
        }

        public override void ReduceOptions() {

            for( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                ScanPairs( Solver.Data.Rows[ i ] );
                ScanPairs( Solver.Data.Columns[ i ] );
                ScanPairs( Solver.Data.Squares[ i ] );
            }
        }
    }
}
