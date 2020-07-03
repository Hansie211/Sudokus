using Sudokus.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Sudokus.Components {

    public class CellLabel : TapLabel {

        public static readonly BindableProperty HasValueProperty    = BindableProperty.Create( nameof(HasValue), typeof(bool), typeof(CellLabel), default(ICommand), BindingMode.Default, null, null, HasValuePropertyChanged );

        public static readonly BindableProperty ValueTextProperty   = BindableProperty.Create( nameof(ValueText), typeof(string), typeof(CellLabel), default(string) );
        public static readonly BindableProperty NoteTextProperty    = BindableProperty.Create( nameof(NoteText), typeof(string), typeof(CellLabel), default(string) );

        public bool HasValue {
            get => (bool)GetValue( HasValueProperty );
            set {
                SetValue( HasValueProperty, value );
            }
        }

        public string ValueText {
            get => (string)GetValue( ValueTextProperty );
            set {
                SetValue( ValueTextProperty, value );
            }
        }

        public string NoteText {
            get => (string)GetValue( NoteTextProperty );
            set {
                SetValue( NoteTextProperty, value );
            }
        }

        private static void HasValuePropertyChanged( BindableObject bindable, object oldValue, object newValue ) {

            CellLabel self = (CellLabel)bindable;
            Binding binding;

            if ( (bool)newValue ) {

                binding = self.GetBinding( ValueTextProperty );
            } else {

                binding = self.GetBinding( NoteTextProperty );
            }

            if ( binding == null ) {
                return;
            }

            self.SetBinding( TextProperty, binding );
        }

        protected override void OnBindingContextChanged() {

            HasValuePropertyChanged( this, !HasValue, HasValue );

            base.OnBindingContextChanged();
        }

    }
}
