using SudokuData;
using Sudokus.Models;
using Sudokus.Extensions;
using Sudokus.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Sudokus.Converter;
using System.Linq;

namespace Sudokus.ViewModels {

    public partial class MainViewModel : ViewModel {

        public SudokuModel Sudoku { get; }

        private List<Move> Moves { get; }

        private int _ActiveSelection = 1;
        public int ActiveSelection {
            get => _ActiveSelection;
            set {
                SetProperty( ref _ActiveSelection, value );
            }
        }

        private bool _NotesMode = false;
        public bool NotesMode {
            get => _NotesMode;
            set {
                SetProperty( ref _NotesMode, value );
            }
        }

        public IEnumerable<string> Difficulties {
            get => new string[] { "Easy", "Medium", "Hard" };
        }

        private Difficulty _Difficulty = Difficulty.Easy;
        public Difficulty Difficulty {
            get => _Difficulty;
            set { SetProperty( ref _Difficulty, value ); }
        }

        private int _ErrorCount = 0;
        public int ErrorCount {
            get => _ErrorCount;
            set {  SetProperty( ref _ErrorCount, value); }
        }

        private int _HintCount = 0;
        public int HintCount {
            get => _HintCount;
            set { SetProperty( ref _HintCount, value ); }
        }

        public ICommand ChangeSelectionCommand { get; }
        public ICommand CellTapCommand { get; }
        public ICommand ChangeNotesModeCommand { get; }

        public ICommand GenerateCommand { get; }
        public ICommand HintCommand { get; }
        public ICommand FillNotesCommand { get; }
        public ICommand ResetCommand { get; }

        public ICommand UndoCommand { get; }

        public MainViewModel() {

            Sudoku = new SudokuModel();
            Moves  = new List<Move>();

            ChangeSelectionCommand  = new Command( OnChangeSelection );
            CellTapCommand          = new Command( OnCellTap );
            ChangeNotesModeCommand  = new Command( OnChangeNotesMode );

            GenerateCommand         = new Command( OnGenerate );
            HintCommand             = new Command( OnHint );
            FillNotesCommand        = new Command( OnFillNotes );
            ResetCommand            = new Command( OnReset );

            UndoCommand             = new Command( OnUndo );
        }

        private void ResetModel() {

            ActiveSelection = 1;
            Moves.Clear();
            ErrorCount = 0;
            HintCount  = 0;
        }

        private void OnChangeSelection( object parameter ) {

            ActiveSelection = (int)parameter;
        }

        private void OnChangeNotesMode( object parameter ) {

            NotesMode = !NotesMode;
        }

        private void OnCellTap( object parameter ) {

            CellModel cell = (CellModel)parameter;

            if ( cell.IsConstant ) {
                return;
            }

            Move move;

            if ( !NotesMode ) {
                
                move = new CellMove( cell );
                Moves.Add( move );

                if ( cell.Value == ActiveSelection ) {

                    cell.Value = 0;
                } else {

                    cell.Value = ActiveSelection;

                    if ( cell.IsError ) {

                        ErrorCount++;
                    }
                }

                Validate();

                return;
            }

            move = new NoteMove( cell );
            Moves.Add( move );

            if ( ActiveSelection == 0 ) {

                cell.Notes.Clear();
            } else {

                if ( cell.Notes.Contains( ActiveSelection ) ) {

                    cell.Notes.Remove( ActiveSelection );
                } else {

                    cell.Notes.Add( ActiveSelection );
                }
            }

            cell.RaisePropertyChanged( "Notes" );
        }

        private async void OnGenerate( object parameter ) {

            string result = await Application.Current.MainPage.DisplayActionSheet( "Difficulty", "Cancel", null, "Easy", "Medium", "Hard" );
            if ( result == "Cancel" ) {
                return;
            }

            Sudoku.GenerateRandom( (Difficulty)EnumConverter.ConvertBack( result, typeof( Difficulty ) ) );
            ResetModel();
        }

        private void OnHint( object parameter ) {

            foreach ( var cell in Sudoku.Cells ) {

                if ( cell.HasValue ) {
                    continue;
                }

                Moves.Add( new CellMove( cell ) );
                cell.Value = cell.SolutionCell.Value;
                HintCount++;

                Validate();

                return;
            }
        }

        private IEnumerable<int> GetNotesForCell( CellModel cell ) {

            for ( int num = 0 + 1; num < 9 + 1; num++ ) {

                if ( cell.Square.Contains( num ) ) {

                    continue;
                }

                if ( cell.Row.Contains( num ) ) {

                    continue;
                }

                if ( cell.Column.Contains( num ) ) {

                    continue;
                }

                yield return num;
            }
        }

        private void OnFillNotes( object parameter ) {

            foreach ( var cell in Sudoku.Cells ) {

                if ( cell.HasValue ) {
                    continue;
                }

                var cellNotes = GetNotesForCell( cell );
                if ( cell.Notes.EqualRange( cellNotes ) ) {

                    continue;
                }

                Moves.Add( new NoteMove(cell) );

                cell.Notes.Clear();
                foreach ( var note in cellNotes ) {

                    cell.Notes.Add( note );
                }

                cell.RaisePropertyChanged( "Notes" );
            }
        }

        private void OnReset( object parameter ) {

            foreach ( var cell in Sudoku.Cells ) {

                if ( cell.IsConstant ) {

                    continue;
                }

                cell.Reset( 0 );
            }

            ResetModel();
        }

        private void OnUndo() {

            if ( Moves.Count() == 0 ) {
                return;
            }

            Move last = Moves.Last();

            if ( last is CellMove ) {

                if ( ((CellMove)last).Cell.IsError ) {
                    ErrorCount--;
                }
            }
            
            last.Undo();

            Moves.Remove( last );
        }

        private async void Validate() {

            foreach( var cell in Sudoku.Cells ) {

                if ( cell.IsConstant ) {

                    continue;
                }

                if ( !cell.IsValid ) {

                    return;
                }
            }

            await Application.Current.MainPage.DisplayAlert("Solved!", "You have solved this sudoku!", "Generate next" );
            OnGenerate( null );
        }
    }
}
