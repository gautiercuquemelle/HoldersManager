﻿using HoldersManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoldersManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HolderFilmDetailPage : ContentPage
    {
        HolderFilmDetailViewModel _viewModel;

        public HolderFilmDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HolderFilmDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.RefreshPage();
        }
    }
}