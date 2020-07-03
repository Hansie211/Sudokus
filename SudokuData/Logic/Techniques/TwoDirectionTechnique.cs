using SudokuData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuData.Logic.Techniques {

    public class TwoDirectionTechnique : Technique {

        public TwoDirectionTechnique( SudokuSolver solver ) : base( solver ) {
        }

        public override void ReduceOptions() {

            foreach ( var cell in Solver.Data ) {

                if ( cell.HasValue ) {

                    continue;
                }

                foreach ( int option in cell.Notes.ToArray() ) {

                    foreach ( var structure in cell.Structures ) {

                        if ( !structure.Contains( option ) ) {
                            continue;
                        }

                        cell.Notes.Remove( option );
                        break;
                    }
                }
            }
        }

    }
}
