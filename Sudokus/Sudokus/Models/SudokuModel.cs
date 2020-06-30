using SudokuData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sudokus.Models {


    public class SquareModel {

        public CellModel[] Cells { get; }

        public SquareModel() {

            Cells = new CellModel[9];
        }
    }

    public class SudokuModel {

        public Sudoku Solution { get; }
        public SquareModel[] Squares { get; }

        public SudokuModel() {

            Solution = Sudoku.GenerateRandom();
            Squares  = new SquareModel[9];

            for( int i = 0; i < Squares.Length; i++ ) {

                Squares[i] = new SquareModel();

                for( int j = 0; j < Squares[i].Cells.Length; j++ ) {

                    Squares[i].Cells[j] = new CellModel( Solution.Squares[i].Cells[j].Value );
                }

            }
        }

    }
}
