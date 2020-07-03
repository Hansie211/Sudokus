using Sudokus.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sudokus.ViewModels {

    public partial class MainViewModel {

        private abstract class Move {

            public CellModel Cell { get; }

            public Move( CellModel cell ) {
                    
                Cell = cell;
            }

            public abstract void Undo();
        }

        private class CellMove : Move {

            public int Value { get; }

            public CellMove( CellModel cell ) : base( cell ) {

                Value = cell.Value;
            }

            public override void Undo() {

                Cell.Value = Value;
            }
        }

        private class NoteMove : Move {

            public IEnumerable<int> Value { get; }

            public NoteMove( CellModel cell ) : base( cell ) {

                Value = cell.Notes.ToArray();
            }

            public override void Undo() {

                Cell.Notes.Clear();
                foreach ( var note in Value ) {
                    Cell.Notes.Add( note );
                }

                Cell.RaisePropertyChanged( "Notes" );
            }
        }


    }
}
