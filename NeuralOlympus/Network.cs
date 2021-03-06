﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralOlympus
{
    public partial class Network : coreclass.NetworkNucleusEntity
    {
        private int _Inputs;
        private int _HiddenLayers;
        private int _NeuronsByLayer;
        private int _Outputs;
        private bool _Initialized;
        private Activator _ActivationFunction;
        private EjecutionMode _EjecMode;
        private Layer InputLayer;
        private List<Layer> HiddenLayers = new List<Layer>();
        private Layer OutputLayer;
        private List<Synapse> Synapses = new List<Synapse>();

        public Activator ACTIVATION_FUNCTION { get { return _ActivationFunction; } }

        public enum Activator { TAN, RELU, SIG };
        public enum EjecutionMode { SYNC, ASYNC };

        public List<Synapse> GetSynapses { get { return Synapses; } }

        public Network(int Inputs, int HiddenLayers, int NeuronsByLayer, int Outputs, bool Initialize = true, bool InMemmoryLog = false, Activator ActivationFunction = Activator.RELU, EjecutionMode EjecMode = EjecutionMode.SYNC)
        {
            _Inputs = Inputs;
            _HiddenLayers = HiddenLayers;
            _NeuronsByLayer = NeuronsByLayer;
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

        public void SetInputs(float[] Inputs)
        {
            if (Inputs.Length != InputLayer.NEURONS.Count)
            {
                throw new Exception("Array incompatible con la cantidad de entradas");
            }
            else
            {
                for (int a = 0; a < InputLayer.NEURONS.Count; a++)
                {
                    InputLayer.NEURONS[a].setInput(Inputs[a]);
                }
            }
        }

        public NetworkResult Calculate(float[] ExpectedValues)
        {
            List<float> results = new List<float>();
            for (int a = 0; a < HiddenLayers.Count; a++)
            {
                HiddenLayers[a].Calculate();
            }

            OutputLayer.Calculate();

            

            float ResultError = 0f;

            for (int a=0; a<ExpectedValues.Length; a++)
            {
                results.Add(OutputLayer.NEURONS[a].RESULT);
                float SubResultThershold = ExpectedValues[a] - OutputLayer.NEURONS[a].RESULT;
                ResultError += SubResultThershold;
            }
            
            return new NetworkResult(ResultError,results.ToArray());

        }

        public void AdjustNetwork(float Error)
        {
            foreach (Layer L in HiddenLayers)
            {
                L.AdjustLayer(Error);
            }
            for ( int a=0;a<Synapses.Count;a++)
            {
                Synapse S = Synapses[a];
                S.AdjustWeight(Error);
            }
        }

        private void CreateConnections()
        {
            //Para unicamente Inputs a primer capa
            foreach (Neuron Nb in InputLayer.NEURONS) { 
                foreach (Neuron Ne in HiddenLayers[0].NEURONS)
                {
                    Synapses.Add(new Synapse(this, Nb, Ne));
                }
            }

            //Para Conexion Entre capas menos capa final
            for (int a = 0; a < _HiddenLayers - 2; a++)
            {
                Layer Lb = HiddenLayers[a];
                Layer Le = HiddenLayers[a + 1];
                foreach (Neuron Nb in Lb.NEURONS)
                {
                    foreach (Neuron Ne in Le.NEURONS)
                    {
                        Synapses.Add(new Synapse(this, Nb, Ne));
                    }
                }
            }

            //Para conexion Final
            foreach (Neuron Nb in HiddenLayers[HiddenLayers.Count-1].NEURONS)
            {
                foreach (Neuron Ne in OutputLayer.NEURONS)
                {
                    Synapses.Add(new Synapse(this, Nb, Ne));
                }
            }

        }


    }
}
