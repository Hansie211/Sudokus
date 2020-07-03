using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;
using Sudokus.Extensions;

namespace Sudokus.ViewModels {

    public abstract class ViewModel : INotifyPropertyChanged {

        private static readonly string Namespace = typeof( ViewModel ).Assembly.GetName().Name;
        private static INavigation AppNavigation { get => Application.Current?.MainPage?.Navigation; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected ViewModel() {

        }

        protected void RaisePropertyChanged( [CallerMemberName] string propertyName = "" ) {

            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        protected bool SetProperty<T>( ref T store, T value, [CallerMemberName] string propertyName = "" ) {

            if ( EqualityComparer<T>.Default.Equals( store, value ) ) {
                return false;
            }

            store = value;
            RaisePropertyChanged( propertyName );
            return true;
        }

        protected static Type GetViewModelType( Type page ) {

            string typeName = $"{ Namespace }.ViewModels.{ page.Name.Substring( 0, -4, true ) }ViewModel";
            return Type.GetType( typeName );
        }

        protected static Type GetPageType( Type VM ) {

            string typeName = $"{ Namespace }.Views.{ VM.Name.Substring( 0, -9, true ) }Page";
            return Type.GetType( typeName );
        }

        protected Page CreatePageInstance( Type pageType, params object[] args ) {

            return (Page)Activator.CreateInstance( pageType, args );
        }

        protected static NavigationPage CreatePage( Type pageType, params object[] args ) {

            Type VMType     = GetViewModelType(pageType);
            ViewModel VM    = (ViewModel)Activator.CreateInstance( VMType, args );

            Page Page = VM.CreatePageInstance( pageType );
            Page.BindingContext = VM;

            return new NavigationPage( Page );
        }

        public static NavigationPage CreatePage<T>( params object[] args ) where T : Page {

            return CreatePage( typeof( T ), args );
        }

        protected static async Task<ViewModel> RunModalAsync( Type type, params object[] args ) {

            NavigationPage page = CreatePage( type, args );
            await AppNavigation?.PushModalAsync( page );

            return page.RootPage.BindingContext as ViewModel;
        }

        public static async Task<ViewModel> RunModalAsync<T>( params object[] args ) where T : Page {

            return await RunModalAsync( typeof( T ), args );
        }

        protected async Task PopModalAsync() {

            await AppNavigation?.PopModalAsync();
        }
    }
}
