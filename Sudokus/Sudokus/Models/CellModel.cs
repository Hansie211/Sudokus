using System;
using System.Collections.Generic;
using System.Text;

namespace Sudokus.Models {
    public class CellModel {

        public List<int> Notes { get; }
        public int Value { get; set; }
        public bool HasValue { get => Value > 0; }
        public bool Constant { get; }

        public CellModel( int value = 0 ) {

            Notes = new List<int>( 9 );
            Value = value;

            Constant = HasValue;
        }
    }
}
