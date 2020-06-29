using Sudokus.ViewModels;
using Sudokus.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sudokus {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            MainPage = ViewModel.CreatePage<MainPage>();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
