﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralOlympus
{
    public partial class Network
    {
        private int _Inputs;
        private int _HiddenLayers;
        private int _NeuronsByLayer;
        private int _Outputs;
        private bool _Initialized;
        private Activator _ActivationFunction;
        private EjecutionMode _EjecMode;
        private Layer InputLayer;
        private List<Layer> HiddenLayers;
        private Layer OutputLayer;
        private List<Synapse> Synapses;

        public enum Activator { TAN, RELU, SIG };
        public enum EjecutionMode { SYNC, ASYNC };
        


        public Network(int Inputs, int HiddenLayers, int NeuronByLayer, int Outputs, bool Initialize = true, bool InMemmoryLog = false, Activator ActivationFunction = Activator.RELU, EjecutionMode EjecMode = EjecutionMode.SYNC)
        {
            _Inputs = Inputs;
            _HiddenLayers = HiddenLayers;
            _NeuronsByLayer = NeuronByLayer;
            _Outputs = Outputs;
            _ActivationFunction = ActivationFunction;
            if (Initialize == true)
            {
                InitMe();
            }

        }

        public void Init()
        {
            if (_Initialized == true)
            {
                throw new Exception("No se puede iniciar una red que ya esta establecida campeón, o haces una nueva o te vas a la concha de tu madre!");
            }
            InitMe();
        }

        private void InitMe()
        {
            CreateLayer();
            CreateConnections();
        }


        private void CreateLayer()
        {
            if (_Initialized == false)
            {
                _Initialized = true;
                InputLayer = new Layer(_Inputs, Layer.LayerType.Input, this);
                OutputLayer = new Layer(_Outputs, Layer.LayerType.Output, this);
                for (int a = 0; a < _HiddenLayers; a++)
                {
                    HiddenLayers.Add(new Layer(_NeuronsByLayer, Layer.LayerType.Hidden, this));
                }
            }
        }

        private void CreateConnections()
        {

        }


    }
}