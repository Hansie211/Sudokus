using SudokuData.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sudokus.Models {

    public class CellModel : INotifyPropertyChanged {

        public IList<int> Notes { get; }

        public bool HasValue { get => Value > 0; }
        public bool IsError { get => HasValue && SolutionCell.Value != Value; }
        public bool IsValid { get => HasValue && SolutionCell.Value == Value; }

        private int _Value;
        public int Value { 
            get => _Value; 
            set { 
                SetProperty( ref _Value, value );

                RaisePropertyChanged( nameof(HasValue) );
                RaisePropertyChanged( nameof(IsError) );
            } 
        }
        
        private bool _IsConstant;
        public bool IsConstant { 
            get => _IsConstant; 
            set { SetProperty( ref _IsConstant, value ); }
        }

        public Square   Square  { get; set; }
        public Column   Column  { get; set; }
        public Row      Row     { get; set; }

        public int X { get => Column.Index; }
        public int Y { get => Row.Index; }

        public Cell SolutionCell { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CellModel( Cell solutionCell ) {

            Notes           = new BindingList<int>();
            SolutionCell    = solutionCell;
        }

        public void RaisePropertyChanged( string propName ) {

            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propName ) );
        }

        private void SetProperty<T>( ref T prop, T value, [CallerMemberName] string propName = "" ) {

            if ( EqualityComparer<T>.Default.Equals(prop, value) ) {
                return;
            } 

            prop = value;
            RaisePropertyChanged( propName );
        }

        public void Reset( int value ) {

            Notes.Clear();
            Value = value;

            IsConstant = HasValue;

            RaisePropertyChanged( "Notes" );
        }
    }
}
