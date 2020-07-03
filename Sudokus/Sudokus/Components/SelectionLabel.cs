using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sudokus.Components {

    public class SelectionLabel : TapLabel {

        public static readonly BindableProperty ValueProperty = BindableProperty.Create( nameof( Value ), typeof( int ), typeof( SelectionLabel ), default( int ) );

        public int Value {
            get => (int)GetValue( ValueProperty );
            set {
                SetValue( ValueProperty, value );
            }
        }

        protected override void ExecuteTapCommand( object parameter ) {

            base.ExecuteTapCommand( Value );
        }

    }
}
