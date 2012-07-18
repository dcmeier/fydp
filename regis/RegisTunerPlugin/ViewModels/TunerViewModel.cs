﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Regis.Base.ViewModels;
using System.IO;
using RegisTunerPlugin.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace RegisTunerPlugin.ViewModels
{
    [Export]
    public class TunerViewModel : BaseViewModel
    {
        public TunerViewModel()
        {
            Tunings = new ObservableCollection<Tuning>();
            LoadTunings();
        }

        private ObservableCollection<Tuning> _tunings;
        public ObservableCollection<Tuning> Tunings
        {
            get { return _tunings; }
            private set { _tunings = value; }
        }


        private void LoadTunings()
        {
            try
            {
                StreamReader readFile = new StreamReader(Environment.CurrentDirectory + "\\tunings.cfg");
                while (true)
                {
                    string line = readFile.ReadLine();

                    if (line == "#end")
                        break;

                    Tuning tuning = new Tuning();
                    ObservableCollection<GuitarString> guitarStrings = new ObservableCollection<GuitarString>();
                    tuning.Name = line;

                    for (int i = 0; i < 6; i++)
                    {
                        GuitarString guitarString = new GuitarString();
                        line = readFile.ReadLine();
                        string[] lineParts = line.Split(',');
                        guitarString.StringName = lineParts[0];
                        guitarString.Frequency = Convert.ToDouble(lineParts[1]);
                        guitarString.StringNum = i;
                        guitarStrings.Add(guitarString);
                    }

                    line = readFile.ReadLine(); // blank line

                    tuning.GuitarStrings = guitarStrings;
                    Tunings.Add(tuning);
                }
            }
            catch { }
        }


    }
}
