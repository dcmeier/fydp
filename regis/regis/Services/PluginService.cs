﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Regis.Plugins;
using System.ComponentModel.Composition;
using Regis.Plugins.Models;
using System.Collections.ObjectModel;

namespace Regis.Services
{
    public class PluginLoadedEventArgs : EventArgs
    {
        IPlugin _plugin;

        public PluginLoadedEventArgs(IPlugin plugin)
        {
            _plugin = plugin;
        }

        public IPlugin Plugin
        {
            get
            {
                return _plugin;
            }
        }

    }

    public class PluginLoadException : Exception
    {
        public PluginLoadException(string exceptionString) : base(exceptionString)
        {
        }

    }

    [Export(typeof(IPluginService))]
    public class PluginService : IPluginService//, IPartImportsSatisfiedNotification
    {
        [ImportMany(typeof(IPlugin))]
        ObservableCollection<IPlugin> _plugins;

        NoteStream _noteStream;

        [ImportingConstructor]
        public PluginService([Import(Regis.Strings.DefaulNoteStreamString)] NoteStream noteStream)
        {
            _noteStream = noteStream;


        }

        public void LoadPlugin(string pluginName)
        {
            if (string.IsNullOrEmpty(pluginName))
                throw new ArgumentNullException("plugin");

            IPlugin pluginToLoad = _plugins.Where(plugin => plugin.PluginName == pluginName).SingleOrDefault();

            if (pluginToLoad == null)
                throw new PluginLoadException("Cannot find plugin with name: " + pluginName);


            pluginToLoad.Load(_noteStream);
            RaisePluginLoaded(pluginToLoad);
        }

        public ObservableCollection<IPlugin> Plugins
        {
            get
            {
                return _plugins;
            }
        }

        #region Events
        private void RaisePluginLoaded(IPlugin plugin)
        {
            EventHandler<PluginLoadedEventArgs> h = PluginLoaded;
            if (h == null) return;

            h(this, new PluginLoadedEventArgs(plugin));
        }
        public event EventHandler<PluginLoadedEventArgs> PluginLoaded;
        #endregion

        //public void OnImportsSatisfied()
        //{
        //    ObservableCollection<IPlugin> plugins =;
            
        //}
    }
}