using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sudokus.Components {

    public class TapLabel : Label {

        public static readonly BindableProperty TapCommandProperty          = BindableProperty.Create( nameof(TapCommand), typeof(ICommand), typeof(TapLabel), default(ICommand) );
        public static readonly BindableProperty TapCommandParameterProperty = BindableProperty.Create( nameof(TapCommandParameter), typeof(object), typeof(TapLabel), default(object) );

        public ICommand TapCommand {
            get => (ICommand)GetValue( TapCommandProperty );
            set {
                SetValue( TapCommandProperty, value );
            }
        }

        public object TapCommandParameter {

            get => GetValue( TapCommandParameterProperty );
            set {
                SetValue( TapCommandParameterProperty, value );
            }
        }

        public TapLabel() {

            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnTapEvent;

            this.GestureRecognizers.Add( tapGesture );
        }

        protected virtual void ExecuteTapCommand( object parameter ) {

            TapCommand.Execute( parameter );
        }

        private void OnTapEvent( object sender, EventArgs e ) {

            if ( TapCommand == null ) {
                return;
            }

            if ( !TapCommand.CanExecute( TapCommandParameter ) ) {
                return;
            }

            ExecuteTapCommand( TapCommandParameter );
        }
    }
}
