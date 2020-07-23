using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using System;

using Random = UnityEngine.Random;

public class NNet : MonoBehaviour
{
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 28);

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
                Matrix<float> inputToH1 = Matrix<float>.Build.Dense(28, hiddenNeuronCount);
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
    public (float, float, float, float, float) RunNetwork (float mace1X, float mace1Y, float mace2X, float mace2Y, float mace3X, float mace3Y, float saw1X, float saw1Y, float saw2X, float saw2Y, float saw3X, float saw3Y, float water1X, float water1Y, float water2X, float water2Y, float water3X, float water3Y, float tile1X, float tile1Y, float tile2X, float tile2Y, float longTile1X, float longTile1Y, float coin1X, float coin1Y, float coin2X, float coin2Y)
    {
        inputLayer[0, 0] = mace1X;
        inputLayer[0, 1] = mace1Y;
        inputLayer[0, 2] = mace2X;
        inputLayer[0, 3] = mace2Y;
        inputLayer[0, 4] = mace3X;
        inputLayer[0, 5] = mace3Y;
        inputLayer[0, 6] = saw1X;
        inputLayer[0, 7] = saw1Y;
        inputLayer[0, 8] = saw2X;
        inputLayer[0, 9] = saw2Y;
        inputLayer[0, 10] = saw3X;
        inputLayer[0, 11] = saw3Y;
        inputLayer[0, 12] = water1X;
        inputLayer[0, 13] = water1Y;
        inputLayer[0, 14] = water2X;
        inputLayer[0, 15] = water2Y;
        inputLayer[0, 16] = water3X;
        inputLayer[0, 17] = water3Y;
        inputLayer[0, 18] = tile1X;
        inputLayer[0, 19] = tile1Y;
        inputLayer[0, 20] = tile2X;
        inputLayer[0, 21] = tile2Y;
        inputLayer[0, 22] = longTile1X;
        inputLayer[0, 23] = longTile1Y;
        inputLayer[0, 24] = coin1X;
        inputLayer[0, 25] = coin1Y;
        inputLayer[0, 26] = coin2X;
        inputLayer[0, 27] = coin2Y;
        
        hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();

        for (int i = 1; i < hiddenLayers.Count; i++)
        {
            hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }

        outputLayer = ((hiddenLayers[hiddenLayers.Count-1]*weights[weights.Count-1])+biases[biases.Count-1]).PointwiseTanh();
        return (Sigmoid(outputLayer[0,0]), Sigmoid(outputLayer[0,1]), Sigmoid(outputLayer[0,2]), Sigmoid(outputLayer[0,3]), Sigmoid(outputLayer[0,4]));
    }
    // testing save network
    public void saveBestNetwork(float fitnss) // save network to file
    {
        System.IO.File.WriteAllText(@"C:\Users\Khoi Tran\test.txt",string.Empty);
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Khoi Tran\test.txt", true))
        {
            file.WriteLine(fitnss.ToString());
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
    public void saveNetwork(int genome)
    {
        if (genome == 0)
            System.IO.File.WriteAllText(@"C:\Users\Khoi Tran\population.txt",string.Empty);
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Khoi Tran\population.txt", true))
        {
            file.WriteLine(genome.ToString());
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