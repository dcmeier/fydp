﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Regis.Plugins;
using Regis.Services;
using System.Windows.Input;
using Regis.Plugins.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Regis.Base.ViewModels;

namespace Regis.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : BaseViewModel, IPartImportsSatisfiedNotification
    {
        [Import(typeof(IPluginService))]
        private IPluginService _pluginService;
        private IPlugin _currentPlugin;

        public void OnImportsSatisfied()
        {
            LoadPluginCommand = new Regis.Commands.LoadPluginCommand(_pluginService);
            _pluginService.PluginLoaded += new EventHandler<PluginLoadedEventArgs>(_pluginService_PluginLoaded);
            NotifyPropertyChanged(_PluginsChangedArgs);
        }

        public ICommand LoadPluginCommand
        {
            get;
            private set;
        }

        private PropertyChangedEventArgs _currentPluginChangedArgs = new PropertyChangedEventArgs("CurrentPlugin");
        public IPlugin CurrentPlugin
        {
            get
            {
                return _currentPlugin;
            }

            private set
            {
                _currentPlugin = value;
                NotifyPropertyChanged(_currentPluginChangedArgs);
            }
        }

        private PropertyChangedEventArgs _PluginsChangedArgs = new PropertyChangedEventArgs("Plugins");
        public ObservableCollection<IPlugin> Plugins
        {
            get
            {
                return _pluginService.Plugins;
            }
        }

        void _pluginService_PluginLoaded(object sender, PluginLoadedEventArgs e)
        {
            CurrentPlugin = e.Plugin;
        }
    }
}