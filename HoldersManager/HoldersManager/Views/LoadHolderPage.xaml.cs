﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HoldersManager.ViewModels;

namespace HoldersManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadHolderPage : ContentPage
    {
        public LoadHolderPage()
        {
            InitializeComponent();
            BindingContext = new LoadHolderViewModel();
        }
    }
}