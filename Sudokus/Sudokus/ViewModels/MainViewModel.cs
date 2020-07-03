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

        public ICommand ChangeSelectionCommand { get; }
        public ICommand CellTapCommand { get; }
        public ICommand ChangeNotesModeCommand { get; }

        public ICommand GenerateCommand { get; }
        public ICommand HintCommand { get; }
        public ICommand FillNotesCommand { get; }
        public ICommand ResetCommand { get; }

        public MainViewModel() {

            Sudoku = new SudokuModel();

            ChangeSelectionCommand  = new Command( OnChangeSelection );
            CellTapCommand          = new Command( OnCellTap );
            ChangeNotesModeCommand  = new Command( OnChangeNotesMode );

            GenerateCommand         = new Command( OnGenerate );
            HintCommand             = new Command( OnHint );
            FillNotesCommand        = new Command( OnFillNotes );
            ResetCommand            = new Command( OnReset );
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

            if ( !NotesMode ) {

                if ( cell.Value == ActiveSelection ) {

                    cell.Value = 0;
                } else {

                    cell.Value = ActiveSelection;
                }

                return;
            }

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
        }

        private void OnHint( object parameter ) {

            foreach ( var cell in Sudoku.Cells ) {

                if ( cell.HasValue ) {
                    continue;
                }

                cell.Value = cell.SolutionCell.Value;

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

                if ( cell.Square.Contains( num ) ) {

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

                cell.Notes.Clear();

                foreach ( var note in GetNotesForCell( cell ) ) {

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
        }

    }
}
