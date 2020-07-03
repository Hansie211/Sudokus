using Sudokus.Converter;
using Sudokus.Extensions;
using Sudokus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sudokus.Views {

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible( false )]
    public partial class MainPage : ContentPage {

        private IEnumerable<T> GetChildrenOfType<T>( Layout<View> parent ) where T: class{

            List<T> result = new List<T>();

            foreach( var child in parent.Children ) {

                if ( child is ContentView ) {

                    ContentView content = (ContentView)child;

                    if ( content.Content is T ) {

                        result.Add( content.Content as T );
                    } else if ( content.Content is Layout<View> ) {

                        result.AddRange( GetChildrenOfType<T>( content.Content as Layout<View> ) );
                    }
                }

                if ( child is ContentView && ((ContentView)child).Content is T ) {

                    result.Add(( (ContentView)child ).Content as T );
                } else if ( child is Layout<View> ) {

                    var temp = GetChildrenOfType<T>( child as Layout<View> );
                    result.AddRange( temp );
                } else if ( child is T ) {

                    result.Add( child as T );
                }
            }

            return result;
        }

        public MainPage() {
            InitializeComponent();

            //var labels = GetChildrenOfType<Label>( SudokuBoard );

            //foreach( var label in labels ) {

            //    var cell    = (CellModel)label.BindingContext;
            //    var binding = label.GetBinding( Label.TextProperty );

            //    label.RemoveBinding( Label.TextProperty );

            //    binding.Source = cell;
            //    //binding.Path = null;

            //    label.SetBinding( Label.TextProperty, binding );
            //}
        }
        
    }
}
