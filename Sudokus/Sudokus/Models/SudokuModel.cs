using SudokuData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sudokus.Models {

    public class SudokuModel {

        public Sudoku Data { get; }
        public List<int>[,] Notes { get; }

        public SudokuModel() {

            Data    = Sudoku.GenerateEmpty();
            Notes   = new List<int>[ Sudoku.BOARDSIZE, Sudoku.BOARDSIZE ];

            for ( int x = 0; x < Sudoku.BOARDSIZE; x++ ) {
                for ( int y = 0; y < Sudoku.BOARDSIZE; y++ ) {

                    Notes[ x, y ] = new List<int>( Sudoku.BOARDSIZE );
                }
            }
        }

    }
}
