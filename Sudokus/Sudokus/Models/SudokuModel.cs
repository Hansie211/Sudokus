using SudokuData;
using SudokuData.Core;
using SudokuData.Logic;
using Sudokus.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sudokus.Models {

    public partial class SudokuModel : INotifyPropertyChanged {

        public Sudoku Solution { get; }

        public IList<Square> Squares { get; }
        public IList<Column> Columns { get; }
        public IList<Row> Rows { get; }

        public CellModel[,] Cells { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SudokuModel() {

            Cells = new CellModel[ Sudoku.BOARDSIZE, Sudoku.BOARDSIZE ];
            Solution = Sudoku.GenerateEmpty();

            foreach ( var cell in Solution.Data ) {

                Cells[ cell.X, cell.Y ] = new CellModel( cell );
            }

            Squares  = new BindingList<Square>();
            Columns  = new BindingList<Column>();
            Rows     = new BindingList<Row>();

            for ( int i = 0; i < Sudoku.BOARDSIZE; i++ ) {

                Squares.Add( new Square( this, i ) );
                Columns.Add( new Column( this, i ) );
                Rows.Add( new Row( this, i ) );
            }

            GenerateRandom( Difficulty.Easy );
        }

        private IEnumerable<CellModel> GetCellsWithoutValue() {

            foreach ( var cell in Cells ) {

                if ( cell.HasValue ) {
                    continue;
                }

                yield return cell;
            }
        }

        private IEnumerable<int> GetCellOptions( CellModel cell ) {

            for ( int num = 0 + 1; num < Sudoku.BOARDSIZE + 1; num++ ) {

                if ( cell.Row.Contains( num ) ) {
                    continue;
                }

                if ( cell.Column.Contains( num ) ) {
                    continue;
                }

                if ( cell.Square.Contains( num ) ) {
                    continue;
                }

                yield return num;
            }

        }

        private Sudoku AsSudoku() {

            Sudoku result = Sudoku.GenerateEmpty();

            foreach ( var cell in Cells ) {

                result.Data[ cell.X, cell.Y ].Value = cell.Value;
            }

            return result;
        }

        private void ZeroCells() {

            foreach ( var cell in Cells ) {

                int valueCopy = cell.Value;

                cell.Reset( 0 );
                IEnumerable<int> cellOptions = GetCellOptions( cell );

                if ( cellOptions.Count() == 1 ) {
                    continue;
                }

                Sudoku temp = AsSudoku();

                int count = 0;
                foreach ( var option in cellOptions ) {

                    if ( option != valueCopy ) {

                        temp.Data[ cell.X, cell.Y ].Value = option;
                        SudokuSolver solver = SudokuSolver.Setup( temp );
                        if ( !solver.Solve() ) {
                            continue;
                        }
                    }

                    if ( ++count > 1 ) {
                        break;
                    }
                }

                if ( count != 1 ) {

                    cell.Reset( valueCopy );
                }
            }
        }

        public void GenerateRandom( Difficulty difficulty ) {

            Solution.GenerateRandom();

            SudokuSolver solver = SudokuSolver.Setup( Solution );
            solver.Solve( true );

            // Copy solution
            for ( int x = 0; x < Sudoku.BOARDSIZE; x++ ) {
                for ( int y = 0; y < Sudoku.BOARDSIZE; y++ ) {

                    Solution.Data[ x, y ].Value = solver.Data[ x, y ].Value;
                    Cells[ x, y ].Reset( Solution.Data[ x, y ].Value );
                }
            }

            ZeroCells();

            Random random = new Random();

            int emptyCellCount = 0;
            switch ( difficulty ) {
                case Difficulty.Easy:
                    emptyCellCount = random.Next( 30, 40 + 1 );
                    break;
                case Difficulty.Medium:
                    emptyCellCount = random.Next( 40, 45 + 1 );
                    break;
                case Difficulty.Hard:
                    emptyCellCount = random.Next( 45, 55 + 1 );
                    break;
            }

            List<CellModel> emptyCellList = new List<CellModel>( GetCellsWithoutValue() );
            emptyCellList.Shuffle();

            while ( emptyCellList.Count() > emptyCellCount ) {

                var cell = emptyCellList[0];
                cell.Reset( Solution.Data[ cell.X, cell.Y ] );

                emptyCellList.RemoveAt( 0 );
            }
        }

    }
}
