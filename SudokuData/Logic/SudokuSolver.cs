using SudokuData;
using SudokuData.Core;
using SudokuData.Logic.Techniques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuData.Logic {

    public class SudokuSolver {

        private static readonly IEnumerable<int> DefaultOptions = Enumerable.Range(1, Sudoku.BOARDSIZE );

        public Sudoku Data { get; private set; }
        private IEnumerable<Technique> Techniques { get; }

        private SudokuSolver( Sudoku data ) {

            Data        = data;
            Techniques  = Technique.GetTechniques( this );
        }

        public static SudokuSolver Clone( SudokuSolver subject ) {

            return new SudokuSolver( subject.Data.Clone() );
        }

        public static SudokuSolver Setup( Sudoku data ) {

            SudokuSolver solver = new SudokuSolver( data );

            for ( int x = 0; x < Sudoku.BOARDSIZE; x++ ) {
                for ( int y = 0; y < Sudoku.BOARDSIZE; y++ ) {

                    if ( solver.Data[ x, y ].HasValue ) {

                        solver.Data[ x, y ].Notes.Add( solver.Data[ x, y ] );
                    } else {

                        solver.Data[ x, y ].Notes.AddRange( DefaultOptions );
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

                    if ( !Data[ x, y ].Notes.Any() ) {

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

                    IEnumerable<int> optionsList = Data[x,y].Notes;
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

                    int count = Data[ x, y ].Notes.Count();
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

            foreach ( var opt in Data[ position.X, position.Y ].Notes ) {

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
            foreach ( var cell in Data ) {

                result += cell.Notes.Count();
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

            this.Data = solution.Data;

            return true;
        }

    }
}
