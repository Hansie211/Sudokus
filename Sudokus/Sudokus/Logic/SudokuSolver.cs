using SudokuData;
using SudokuData.core;
using Sudokus.Extensions;
using Sudokus.Logic.Techniques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudokus.Logic {

    public class SudokuSolver {

        private static readonly int[] DefaultOptions = new int[ Sudoku.BOARDSIZE ] { 1,2,3,4,5,6,7,8,9 };

        public List<int>[,] Options { get; private set; }
        public Sudoku Data { get; private set; }
        private IEnumerable<Technique> Techniques { get; }

        private SudokuSolver( Sudoku data, List<int>[,] options ) : this( data ) {

            for ( int x = 0; x < Sudoku.BOARDSIZE; x++ ) {
                for ( int y = 0; y < Sudoku.BOARDSIZE; y++ ) {

                    Options[ x, y ] = options[ x, y ].Clone();
                }
            }
        }

        private SudokuSolver( Sudoku data ) {

            Options = new List<int>[ Sudoku.BOARDSIZE, Sudoku.BOARDSIZE ];
            Data    = data;

            Techniques = Technique.GetTechniques( this );
        }

        public static SudokuSolver Clone( SudokuSolver subject ) {

            SudokuSolver solver = new SudokuSolver( subject.Data.Clone(), subject.Options );
            return solver;
        }

        public static SudokuSolver Setup( Sudoku data ) {

            SudokuSolver solver = new SudokuSolver( data );

            for ( int x = 0; x < Sudoku.BOARDSIZE; x++ ) {
                for ( int y = 0; y < Sudoku.BOARDSIZE; y++ ) {

                    if ( solver.Data[ x, y ].HasValue ) {

                        solver.Options[ x, y ] = new List<int>( new int[] { solver.Data[ x, y ] } );
                    } else {

                        solver.Options[ x, y ] = new List<int>( DefaultOptions );
                    }
                }
            }

            return solver;
        }

        public bool IsInvalid() {

            for ( int x = 0; x < Sudoku.BOARDSIZE; x++ ) {
                for ( int y = 0; y < Sudoku.BOARDSIZE; y++ ) {

                    if ( Data[ x, y ].HasValue ) {
                        continue;
                    }

                    bool found = Options[ x, y ].Any();
                    if ( !found ) {

                        return true;
                    }
                }
            }

            return false;
        }

        private bool FillValuesWithOneOption() {

            bool result = false;

            for ( int x = 0; x < Sudoku.BOARDSIZE; x++ ) {
                for ( int y = 0; y < Sudoku.BOARDSIZE; y++ ) {

                    if ( Data[ x, y ].HasValue ) {
                        continue;
                    }

                    IEnumerable<int> optionsList = Options[ x, y ];
                    if ( optionsList.Count() != 1 ) {

                        continue;
                    }

                    int value           = optionsList.First();
                    Data[ x, y ].Value  = value;

                    result = true;
                }
            }

            return result;
        }

        private Point GetSmallestOptionPosition() {

            Point result = new Point( -1, -1 );
            int lowest = Sudoku.BOARDSIZE;

            for ( int x = 0; x < Sudoku.BOARDSIZE; x++ ) {
                for ( int y = 0; y < Sudoku.BOARDSIZE; y++ ) {

                    if ( Data[ x, y ].HasValue ) {
                        continue;
                    }

                    int count = Options[ x, y ].Count();
                    if ( count < lowest ) {

                        lowest = count;
                        result = new Point( x, y );

                        if ( count == 2 ) {
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        private SudokuSolver GuessFork() {

            Point position = GetSmallestOptionPosition();
            if ( position.X < 0 || position.Y < 0 ) {

                return null;
            }

            foreach ( var opt in Options[ position.X, position.Y ] ) {

                SudokuSolver fork = SudokuSolver.Clone( this );

                // Fill the guess
                fork.Data[ position.X, position.Y ].Value = opt;

                // Try to solve
                if ( fork.Solve( true ) ) {

                    return fork;
                }
            }

            return null;
        }

        private int GetOptionsCount() {

            int result = 0;
            foreach( var optionList in Options ) {

                result += optionList.Count();
            }

            return result;
        }

        public bool Solve( bool canFork = true ) {

            int count;
            do {

                count = GetOptionsCount();

                foreach ( Technique technique in Techniques ) {

                    technique.ReduceOptions();
                    FillValuesWithOneOption();
                }

            } while ( count != GetOptionsCount() );

            if ( Data.IsCorrect() ) {

                return true;
            }

            if ( IsInvalid() ) {

                return false;
            }

            if ( !canFork ) {

                return false;
            }

            SudokuSolver solution = GuessFork();
            if ( solution == null ) {

                return false;
            }

            this.Data       = solution.Data;
            this.Options    = solution.Options;

            return true;
        }

        public List<int> GetOptions( Cell cell ) { 

            return Options[ cell.X, cell.Y ];
        }

    }
}
