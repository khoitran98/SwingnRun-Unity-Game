using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using System;

using Random = UnityEngine.Random;

public class NNet : MonoBehaviour
{
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 4);

    public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();

    public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 5);

    public List<Matrix<float>> weights = new List<Matrix<float>>();

    public List<float> biases = new List<float>();

    public float fitness;

    public void Initialise (int hiddenLayerCount, int hiddenNeuronCount)
    {
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();
        for (int i = 0; i < hiddenLayerCount; i++)
        {
            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
            hiddenLayers.Add(f);
            biases.Add(Random.Range(-1f, 1f));
            // Debug.Log("biases: " + biases[i]);
            //WEIGHTS
            if (i == 0)
            {
                Matrix<float> inputToH1 = Matrix<float>.Build.Dense(4, hiddenNeuronCount);
                weights.Add(inputToH1);
            }
            else 
            {
                Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
                weights.Add(HiddenToHidden);
            }

        }
        Matrix<float> OutputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, 5);
        weights.Add(OutputWeight);
        biases.Add(Random.Range(-1f, 1f));
        // Debug.Log("biases: " + biases[biases.Count - 1]);
        RandomiseWeights();
    }
    public NNet InitialiseCopy (int hiddenLayerCount, int hiddenNeuronCount)
    {
        NNet n = new NNet();

        List<Matrix<float>> newWeights = new List<Matrix<float>>();

        for (int i = 0; i < this.weights.Count; i++)
        {
            Matrix<float> currentWeight = Matrix<float>.Build.Dense(weights[i].RowCount, weights[i].ColumnCount);

            for (int x = 0; x < currentWeight.RowCount; x++)
            {
                for (int y = 0; y < currentWeight.ColumnCount; y++)
                {
                    currentWeight[x, y] = weights[i][x, y];
                }
            }

            newWeights.Add(currentWeight);
        }

        List<float> newBiases = new List<float>();

        newBiases.AddRange(biases);

        n.weights = newWeights;
        n.biases = newBiases;

        n.InitialiseHidden(hiddenLayerCount, hiddenNeuronCount);

        return n;
    }
    public void InitialiseHidden (int hiddenLayerCount, int hiddenNeuronCount)
    {
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();

        for (int i = 0; i < hiddenLayerCount + 1; i ++)
        {
            Matrix<float> newHiddenLayer = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
            hiddenLayers.Add(newHiddenLayer);
        }
    }
    public void RandomiseWeights()
    {

        for (int i = 0; i < weights.Count; i++)
        {

            for (int x = 0; x < weights[i].RowCount; x++)
            {

                for (int y = 0; y < weights[i].ColumnCount; y++)
                {

                    weights[i][x, y] = Random.Range(-1f, 1f);
                }
            }
        }
    }
    public (float, float, float, float, float) RunNetwork (float a, float b, float c, bool d)
    {
        inputLayer[0, 0] = Sigmoid(a);
        inputLayer[0, 1] = Sigmoid(b);
        inputLayer[0, 2] = Sigmoid(c);
        if (d)
            inputLayer[0, 3] = 1;
        else
            inputLayer[0, 3] = 0;
        hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();

        for (int i = 1; i < hiddenLayers.Count; i++)
        {
            hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }

        outputLayer = ((hiddenLayers[hiddenLayers.Count-1]*weights[weights.Count-1])+biases[biases.Count-1]).PointwiseTanh();
        return (Sigmoid(outputLayer[0,0]), Sigmoid(outputLayer[0,1]), Sigmoid(outputLayer[0,2]), Sigmoid(outputLayer[0,3]), Sigmoid(outputLayer[0,4]));
    }
    // testing save network
    public void saveNetwork() // save network to file
    {
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Khoi Tran\test.txt", true))
        {
            for (int i = 0; i < weights.Count; i++)
            {
                for (int x = 0; x < weights[i].RowCount; x++)
                {
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                    {
                        file.WriteLine(weights[i][x, y].ToString());
                    }
                }
            }   
            for (int i = 0; i < biases.Count; i++)
            {
                file.WriteLine(biases[i]);
            }    
        }
    }

    private float Sigmoid (float s)
    {
        return (1 / (1 + Mathf.Exp(-s)));
    }

}