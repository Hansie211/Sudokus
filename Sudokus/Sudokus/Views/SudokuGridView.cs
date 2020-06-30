using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Sudokus.Views {

    public class SudokuGridView : ContentView {
    
        public SudokuGridView() {

            Content = new StackLayout {
                Children = {
                    new Label { Text = "Welcome to Xamarin.Forms!" }
                }
            };
        }
    }
}