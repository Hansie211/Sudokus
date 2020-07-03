using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuData {

    public struct RawSudoku {

        public byte[,] Data { get; }

        public byte this[ int x, int y ] {

            get => Data[x,y];
            set {
                Data[x,y] = value;
            }
        }

        public RawSudoku( int size ) {

            Data = new byte[ size, size ];
        }
    }
}
