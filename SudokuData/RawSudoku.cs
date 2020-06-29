using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuData {

    public struct RawSudoku {

        public int[,] Data { get; }

        public int this[ int x, int y ] {

            get => Data[x,y];
            set {
                Data[x,y] = value;
            }
        }

        public RawSudoku( int size ) {

            Data = new int[ size, size ];
        }
    }
}
