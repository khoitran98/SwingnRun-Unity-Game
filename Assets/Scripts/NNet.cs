using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.IO;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using SimpleJSON;
using Newtonsoft.Json;
public class NNet : MonoBehaviour
{
    // public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 28);

    // public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();

    // public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 5);

    // public List<Matrix<float>> weights = new List<Matrix<float>>();

    // public List<float> biases = new List<float>();

    // public float fitness;

    // public void Initialise (int hiddenLayerCount, int hiddenNeuronCount)
    // {
    //     inputLayer.Clear();
    //     hiddenLayers.Clear();
    //     outputLayer.Clear();
    //     weights.Clear();
    //     biases.Clear();
    //     for (int i = 0; i < hiddenLayerCount; i++)
    //     {
    //         Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
    //         hiddenLayers.Add(f);
    //         biases.Add(Random.Range(0f, 1f));
    //         // Debug.Log("biases: " + biases[i]);
    //         //WEIGHTS
    //         if (i == 0)
    //         {
    //             Matrix<float> inputToH1 = Matrix<float>.Build.Dense(28, hiddenNeuronCount);
    //             weights.Add(inputToH1);
    //         }
    //         else 
    //         {
    //             Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
    //             weights.Add(HiddenToHidden);
    //         }

    //     }
    //     Matrix<float> OutputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, 5);
    //     weights.Add(OutputWeight);
    //     biases.Add(Random.Range(0f, 1f));
    //     // Debug.Log("biases: " + biases[biases.Count - 1]);
    //     RandomiseWeights();
    // }
    // public NNet InitialiseCopy (int hiddenLayerCount, int hiddenNeuronCount)
    // {
    //     NNet n = new NNet();

    //     List<Matrix<float>> newWeights = new List<Matrix<float>>();

    //     for (int i = 0; i < this.weights.Count; i++)
    //     {
    //         Matrix<float> currentWeight = Matrix<float>.Build.Dense(weights[i].RowCount, weights[i].ColumnCount);

    //         for (int x = 0; x < currentWeight.RowCount; x++)
    //         {
    //             for (int y = 0; y < currentWeight.ColumnCount; y++)
    //             {
    //                 currentWeight[x, y] = weights[i][x, y];
    //             }
    //         }

    //         newWeights.Add(currentWeight);
    //     }

    //     List<float> newBiases = new List<float>();

    //     newBiases.AddRange(biases);

    //     n.weights = newWeights;
    //     n.biases = newBiases;

    //     n.InitialiseHidden(hiddenLayerCount, hiddenNeuronCount);

    //     return n;
    // }
    // public void InitialiseHidden (int hiddenLayerCount, int hiddenNeuronCount)
    // {
    //     inputLayer.Clear();
    //     hiddenLayers.Clear();
    //     outputLayer.Clear();

    //     for (int i = 0; i < hiddenLayerCount + 1; i ++)
    //     {
    //         Matrix<float> newHiddenLayer = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
    //         hiddenLayers.Add(newHiddenLayer);
    //     }
    // }
    // public void RandomiseWeights()
    // {

    //     for (int i = 0; i < weights.Count; i++)
    //     {

    //         for (int x = 0; x < weights[i].RowCount; x++)
    //         {

    //             for (int y = 0; y < weights[i].ColumnCount; y++)
    //             {

    //                 weights[i][x, y] = Random.Range(0f, 1f);
    //             }
    //         }
    //     }
    // }
    // public (float, float, float, float, float) RunNetwork (float mace1X, float mace1Y, float mace2X, float mace2Y, float mace3X, float mace3Y, float saw1X, float saw1Y, float saw2X, float saw2Y, float saw3X, float saw3Y, float water1X, float water1Y, float water2X, float water2Y, float water3X, float water3Y, float tile1X, float tile1Y, float tile2X, float tile2Y, float longTile1X, float longTile1Y, float coin1X, float coin1Y, float coin2X, float coin2Y)
    // {
    //     inputLayer[0, 0] = mace1X;
    //     inputLayer[0, 1] = mace1Y;
    //     inputLayer[0, 2] = mace2X;
    //     inputLayer[0, 3] = mace2Y;
    //     inputLayer[0, 4] = mace3X;
    //     inputLayer[0, 5] = mace3Y;
    //     inputLayer[0, 6] = saw1X;
    //     inputLayer[0, 7] = saw1Y;
    //     inputLayer[0, 8] = saw2X;
    //     inputLayer[0, 9] = saw2Y;
    //     inputLayer[0, 10] = saw3X;
    //     inputLayer[0, 11] = saw3Y;
    //     inputLayer[0, 12] = water1X;
    //     inputLayer[0, 13] = water1Y;
    //     inputLayer[0, 14] = water2X;
    //     inputLayer[0, 15] = water2Y;
    //     inputLayer[0, 16] = water3X;
    //     inputLayer[0, 17] = water3Y;
    //     inputLayer[0, 18] = tile1X;
    //     inputLayer[0, 19] = tile1Y;
    //     inputLayer[0, 20] = tile2X;
    //     inputLayer[0, 21] = tile2Y;
    //     inputLayer[0, 22] = longTile1X;
    //     inputLayer[0, 23] = longTile1Y;
    //     inputLayer[0, 24] = coin1X;
    //     inputLayer[0, 25] = coin1Y;
    //     inputLayer[0, 26] = coin2X;
    //     inputLayer[0, 27] = coin2Y;
        
    //     hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();

    //     for (int i = 1; i < hiddenLayers.Count; i++)
    //     {
    //         hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]);
    //     }
    //     // sigmoid the entire hiddenlayer
    //     for (int i = 0; i < hiddenLayers.Count; i++)
    //     {
    //         for (int k = 0; k < hiddenLayers[i].ColumnCount; k++)
    //         {
    //             hiddenLayers[i][0, k] = Sigmoid(hiddenLayers[i][0, k]);
    //             Debug.Log(k);
    //         }
    //     }
    //     Debug.Log("done");
    //     outputLayer = ((hiddenLayers[hiddenLayers.Count-1]*weights[weights.Count-1])+biases[biases.Count-1]);
    //     return (Sigmoid(outputLayer[0,0]), Sigmoid(outputLayer[0,1]), Sigmoid(outputLayer[0,2]), Sigmoid(outputLayer[0,3]), Sigmoid(outputLayer[0,4]));
    // }
    // // testing save network
    // public void saveBestNetwork(float fitnss) // save network to file
    // {
    //     System.IO.File.WriteAllText(@"C:\Users\Khoi Tran\test.txt",string.Empty);
    //     using (System.IO.StreamWriter file =
    //         new System.IO.StreamWriter(@"C:\Users\Khoi Tran\test.txt", true))
    //     {
    //         file.WriteLine(fitnss.ToString());
    //         for (int i = 0; i < weights.Count; i++)
    //         {
    //             for (int x = 0; x < weights[i].RowCount; x++)
    //             {
    //                 for (int y = 0; y < weights[i].ColumnCount; y++)
    //                 {
    //                     file.WriteLine(weights[i][x, y].ToString());
    //                 }
    //             }
    //         }   
    //         for (int i = 0; i < biases.Count; i++)
    //         {
    //             file.WriteLine(biases[i]);
    //         }    
    //     }
    // }
    // public void saveNetwork(int genome)
    // {
    //     if (genome == 0)
    //         System.IO.File.WriteAllText(@"C:\Users\Khoi Tran\population.txt",string.Empty);
    //     using (System.IO.StreamWriter file =
    //         new System.IO.StreamWriter(@"C:\Users\Khoi Tran\population.txt", true))
    //     {
    //         file.WriteLine(genome.ToString());
    //         for (int i = 0; i < weights.Count; i++)
    //         {
    //             for (int x = 0; x < weights[i].RowCount; x++)
    //             {
    //                 for (int y = 0; y < weights[i].ColumnCount; y++)
    //                 {
    //                     file.WriteLine(weights[i][x, y].ToString());
    //                 }
    //             }
    //         }   
    //         for (int i = 0; i < biases.Count; i++)
    //         {
    //             file.WriteLine(biases[i]);
    //         }    
    //     }
    // }

    // private float Sigmoid (float s)
    // {
    //     return (1 / (1 + Mathf.Exp(-s)));
    // }
    // public void BackPropagate(float[] outputs)
    // {
    //     return;
    // }
    // private static float SigmoidDerivative(float x) => x * (1 - x);
    private static readonly System.Random Random = new System.Random();
    public float[][] values;
    public float[][] biases;
    public float[][][] weights;

    public float[][] desiredValues;
    public float[][] biasesSmudge;
    public float[][][] weightsSmudge;
    public float [] inputs;
    
    // private const float WeightDecay = 0.001f;
    private const float LearningRate = 0.015f;
    //StreamReader sr = new StreamReader(@"C:\Users\Khoi Tran\training_data.txt");
    public float[] previousOutputs = {0f,1f,0f,0f};
    float[] matrix = new float[1424];
    public void Initialise()
    {
        List<int> structure = new List<int>()
        {
            28,
            24,
            24,
            4
        };
        values = new float[structure.Count][];
        desiredValues = new float[structure.Count][];
        biases = new float[structure.Count][];
        biasesSmudge = new float[structure.Count][];
        weights = new float[structure.Count - 1][][];
        weightsSmudge = new float[structure.Count - 1][][];
        for (var i = 0; i < structure.Count; i++)
        {
            values[i] = new float[structure[i]];
            desiredValues[i] = new float[structure[i]];
            biases[i] = new float[structure[i]];
            biasesSmudge[i] = new float[structure[i]];
        }
        for (var i = 0; i < structure.Count - 1; i++)
        {
            weights[i] = new float[values[i + 1].Length][];
            weightsSmudge[i] = new float[values[i + 1].Length][];
            for (var j = 0; j < weights[i].Length; j++)
            {
                weights[i][j] = new float[values[i].Length];
                weightsSmudge[i][j] = new float[values[i].Length];
                for (var k = 0; k < weights[i][j].Length; k++)
                    weights[i][j][k] = (float) Random.NextDouble() * Mathf.Sqrt(2f / weights[i][j].Length);
            }
        }
        inputs = new float [28];
    }
    public void InitialiseAndLoad()
    {
        List<int> structure = new List<int>()
        {
            28,
            24,
            24,
            4
        };
        values = new float[structure.Count][];
        desiredValues = new float[structure.Count][];
        biases = new float[structure.Count][];
        biasesSmudge = new float[structure.Count][];
        weights = new float[structure.Count - 1][][];
        weightsSmudge = new float[structure.Count - 1][][];
        for (var i = 0; i < structure.Count; i++)
        {
            values[i] = new float[structure[i]];
            desiredValues[i] = new float[structure[i]];
            biases[i] = new float[structure[i]];
            biasesSmudge[i] = new float[structure[i]];
        }
        for (var i = 0; i < structure.Count - 1; i++)
        {
            weights[i] = new float[values[i + 1].Length][];
            weightsSmudge[i] = new float[values[i + 1].Length][];
            for (var j = 0; j < weights[i].Length; j++)
            {
                weights[i][j] = new float[values[i].Length];
                weightsSmudge[i][j] = new float[values[i].Length];
            }
        }
        inputs = new float [28];
        loadNetwork();
    }

    public (float, float, float, float) RunNetwork(float mace1X, float mace1Y, float mace2X, float mace2Y, float mace3X, float mace3Y, float saw1X, float saw1Y, float saw2X, float saw2Y, float saw3X, float saw3Y, float water1X, float water1Y, float water2X, float water2Y, float water3X, float water3Y, float tile1X, float tile1Y, float tile2X, float tile2Y, float longTile1X, float longTile1Y, float coin1X, float coin1Y, float coin2X, float coin2Y)
    //public (float, float, float, float) RunNetwork()
    {
        inputs[0] = values[0][0] = mace1X; // commented out for training purpose
        inputs[1] = values[0][1] = mace1Y;
        inputs[2] = values[0][2] = mace2X;
        inputs[3] = values[0][3] = mace2Y;
        inputs[4] = values[0][4] = mace3X;
        inputs[5] = values[0][5] = mace3Y;
        inputs[6] = values[0][6] = saw1X;
        inputs[7] = values[0][7] = saw1Y;
        inputs[8] = values[0][8] = saw2X;
        inputs[9] = values[0][9] = saw2Y;
        inputs[10] = values[0][10] = saw3X;
        inputs[11] = values[0][11] = saw3Y;
        inputs[12] = values[0][12] = water1X;
        inputs[13] = values[0][13] = water1Y;
        inputs[14] = values[0][14] = water2X;
        inputs[15] = values[0][15] = water2Y;
        inputs[16] = values[0][16] = water3X;
        inputs[17] = values[0][17] = water3Y;
        inputs[18] = values[0][18] = tile1X;
        inputs[19] = values[0][19] = tile1Y;
        inputs[20] = values[0][20] = tile2X;
        inputs[21] = values[0][21] = tile2Y;
        inputs[22] = values[0][22] = longTile1X;
        inputs[23] = values[0][23] = longTile1Y;
        inputs[24] = values[0][24] = coin1X;
        inputs[25] = values[0][25] = coin1Y;
        inputs[26] = values[0][26] = coin2X;
        inputs[27] = values[0][27] = coin2Y;
        for (var i = 1; i < values.Length; i++)
        {
            for (var j = 0; j < values[i].Length; j++)
            {
                values[i][j] = Sigmoid(Sum(values[i - 1], weights[i - 1][j]) + biases[i][j]);
                desiredValues[i][j] = values[i][j];
            }
        }

        return (values[values.Length - 1][0], values[values.Length - 1][1], values[values.Length - 1][2], values[values.Length - 1][3]);
    }

    private static float Sum(IEnumerable<float> values, IReadOnlyList<float> weights) =>
        values.Select((v, i) => v * weights[i]).Sum();

    private static float Sigmoid(float x) => 1f / (1f + (float) Math.Exp(-x));

    private static float HardSigmoid(float x)
    {
        if (x < -2.5f)
            return 0;
        if (x > 2.5f)
            return 1;
        return 0.2f * x + 0.5f;
    }
    void Awake() // This is for training data purpose only
    {
        //InitialiseAndLoad();
    }
    // void Update() // This is for training purposes only
    // {
    //     if (sr.Peek() < 0)
    //         sr.BaseStream.Position = 0;
    //     for (int i = 0; i < 28; i++)
    //     {
    //         try
    //          {   values[0][i] = float.Parse(sr.ReadLine()); }
    //          catch
    //          { Debug.Log(sr.ReadLine());}
    //     }
    //     float[] desiredOutputs = new float[4];
    //     for (int i = 0; i < 4; i++)
    //     {
    //         desiredOutputs[i] = float.Parse(sr.ReadLine());
    //     }
    //     RunNetwork();
    //     float temp = 0; // temp variable for comparison
    //     float index = 0; // variable to store the index of highest output
    //     for (int i = 0; i < 4; i++) // compare outputs to select the highest output
    //     {
    //         if (i == 2) continue;
    //         if (values[values.Length - 1][i] > temp)
    //         {   
    //             temp = values[values.Length - 1][i];
    //             index = i;
    //         }
    //     }
    //     temp = 0; // temp variable for comparison
    //     float index2 = 0; // variable to store the index of highest output
    //     for (int i = 0; i < 4; i++) // compare outputs to select the highest output
    //     {
    //         if (i == 2) continue;
    //         if (desiredOutputs[i] > temp)
    //         {   
    //             temp = desiredOutputs[i];
    //             index2 = i;
    //         }
    //     }
    //     if (desiredOutputs[2] == 1)
    //     {
    //         for (int i = 0; i < 4; i++)
    //         {
    //             desiredOutputs[i] = previousOutputs[i];
    //         }
    //         Train(desiredOutputs);
    //     }
    //     else
    //     {
    //         for (int i = 0; i < 4; i++)
    //         {
    //             previousOutputs[i] = desiredOutputs[i];
    //         }
    //         if (index == index2 && index != 2)
    //         {
    //             Debug.Log("correct prediction");
    //         }
    //         if (index != index2 && index2 != 2)
    //         {
    //             Debug.Log("wrong prediction");
    //             Train(desiredOutputs);
    //         }
    //     }
    //     if (sr.Peek() < 0)
    //         sr.BaseStream.Position = 0;
    //     for (int i = 0; i < 28; i++)
    //     {
    //         try
    //          {   values[0][i] = float.Parse(sr.ReadLine()); }
    //          catch
    //          { Debug.Log(sr.ReadLine());}
    //     }
    //     float[] desiredOutputs2 = new float[4];
    //     for (int i = 0; i < 4; i++)
    //     {
    //         desiredOutputs2[i] = float.Parse(sr.ReadLine());
    //     }
    //     RunNetwork();
    //     temp = 0;
    //     index = 0;
    //     for (int i = 0; i < 4; i++) // compare outputs to select the highest output
    //     {
    //         if (i == 2) continue;
    //         if (values[values.Length - 1][i] > temp)
    //         {   
    //             temp = values[values.Length - 1][i];
    //             index = i;
    //         }
    //     }
    //     temp = 0; // temp variable for comparison
    //     index2 = 0; // variable to store the index of highest output
    //     for (int i = 0; i < 4; i++) // compare outputs to select the highest output
    //     {
    //         if (i == 2) continue;
    //         if (desiredOutputs2[i] > temp)
    //         {   
    //             temp = desiredOutputs2[i];
    //             index2 = i;
    //         }
    //     }
    //     if (desiredOutputs2[2] == 1)
    //     {
    //         for (int i = 0; i < 4; i++)
    //         {
    //             desiredOutputs2[i] = previousOutputs[i];
    //         }
    //         Train(desiredOutputs2);
    //     }
    //     else
    //     {
    //         for (int i = 0; i < 4; i++)
    //         {
    //             previousOutputs[i] = desiredOutputs2[i];
    //         }
    //         if (index == index2 && index != 2)
    //         {
    //             Debug.Log("correct prediction");
    //         }
    //         if (index != index2 && index2 != 2)
    //         {
    //             Debug.Log("wrong prediction");
    //             Train(desiredOutputs2);
    //         }
    //     }
    //     if (sr.Peek() < 0)
    //         sr.BaseStream.Position = 0;
    //     for (int i = 0; i < 28; i++)
    //     {
    //         try
    //          {   values[0][i] = float.Parse(sr.ReadLine()); }
    //          catch
    //          { Debug.Log(sr.ReadLine());}
    //     }
    //     float[] desiredOutputs3 = new float[4];
    //     for (int i = 0; i < 4; i++)
    //     {
    //         desiredOutputs3[i] = float.Parse(sr.ReadLine());
    //     }
    //     RunNetwork();
    //     temp = 0;
    //     index = 0;
    //     for (int i = 0; i < 4; i++) // compare outputs to select the highest output
    //     {
    //         if (i == 2) continue;
    //         if (values[values.Length - 1][i] > temp)
    //         {   
    //             temp = values[values.Length - 1][i];
    //             index = i;
    //         }
    //     }
    //     temp = 0; // temp variable for comparison
    //     index2 = 0; // variable to store the index of highest output
    //     for (int i = 0; i < 4; i++) // compare outputs to select the highest output
    //     {
    //         if (i == 2) continue;
    //         if (desiredOutputs3[i] > temp)
    //         {   
    //             temp = desiredOutputs3[i];
    //             index2 = i;
    //         }
    //     }
    //     if (desiredOutputs3[2] == 1)
    //     {
    //         for (int i = 0; i < 4; i++)
    //         {
    //             desiredOutputs3[i] = previousOutputs[i];
    //         }
    //         Train(desiredOutputs3);
    //     }
    //     else
    //     {
    //         for (int i = 0; i < 4; i++)
    //         {
    //             previousOutputs[i] = desiredOutputs3[i];
    //         }
    //         if (index == index2 && index != 2)
    //         {
    //             Debug.Log("correct prediction");
    //         }
    //         if (index != index2 && index2 != 2)
    //         {
    //             Debug.Log("wrong prediction");
    //             Train(desiredOutputs3);
    //         }
    //     }
    // }
    public void Train(float[] trainingOutputs)
    {  
        // This code is for collecting data
        // {   
        //     using (System.IO.StreamWriter file =
        //         new System.IO.StreamWriter(@"C:\Users\Khoi Tran\training_data.txt", true))
        //     {
        //         for (int i = 0; i < inputs.Length; i++) // save inputs and outputs for training data
        //         {
        //             file.WriteLine(inputs[i].ToString());
        //         }
        //         for (int i = 0; i < trainingOutputs.Length; i++) // save inputs and outputs for training data
        //         {
        //             file.WriteLine(trainingOutputs[i].ToString());
        //         }      
        //     }  
        // }
        for (var j = 0; j < desiredValues[desiredValues.Length - 1].Length; j++)
            desiredValues[desiredValues.Length - 1][j] = trainingOutputs[j];

        for (var j = values.Length - 1; j >= 1; j--)
        {
            for (var k = 0; k < values[j].Length; k++)
            {
                var biasSmudge = SigmoidDerivative(values[j][k]) *
                                    (desiredValues[j][k] - values[j][k]);
                biasesSmudge[j][k] += biasSmudge;
                
                for (var l = 0; l < values[j - 1].Length; l++)
                {
                    var weightSmudge = values[j - 1][l] * biasSmudge;
                    weightsSmudge[j - 1][k][l] += weightSmudge;
                    
                    var valueSmudge = weights[j - 1][k][l] * biasSmudge;
                    desiredValues[j - 1][l] += valueSmudge;
                }
            }
        }
        for (var i = values.Length - 1; i >= 1; i--)
        {
            for (var j = 0; j < values[i].Length; j++)
            {
                biases[i][j] += biasesSmudge[i][j] * LearningRate;
                // biases[i][j] *= 1 - WeightDecay;
                biasesSmudge[i][j] = 0;

                for (var k = 0; k < values[i - 1].Length; k++)
                {
                    weights[i - 1][j][k] += weightsSmudge[i - 1][j][k] * LearningRate;
                    // weights[i - 1][j][k] *= 1 - WeightDecay;
                    weightsSmudge[i - 1][j][k] = 0;
                }
                desiredValues[i][j] = 0;
            }
        }
    }
    private static float SigmoidDerivative(float x) => x * (1 - x);
    public void saveBestNetwork() // save network to file
    {
        System.IO.File.WriteAllText(@"C:\Users\Khoi Tran\finalnetwork2.txt",string.Empty);
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Khoi Tran\finalnetwork2.txt", true))
        {
            for (var i = 0; i < weights.Length; i++)
            {
                for (var j = 0; j < weights[i].Length; j++)
                {
                    for (var k = 0; k < weights[i][j].Length; k++)
                        file.WriteLine(weights[i][j][k].ToString());
                }
            }
            for (var i = 0; i < biases.Length; i++)
            {
                for (var j = 0; j < biases[i].Length; j++)
                {
                    // Debug.Log(biases[i][j]);
                    file.WriteLine(biases[i][j].ToString());
                }
            }
        }
    }
    public void loadNetwork()
    {
        using (System.IO.StreamReader file =
            new System.IO.StreamReader(@"C:\Users\Khoi Tran\finalnetwork2.txt", true))
        {
            for (var i = 0; i < weights.Length; i++)
            {
                for (var j = 0; j < weights[i].Length; j++)
                {
                    for (var k = 0; k < weights[i][j].Length; k++)
                    {
                        if (file.Peek() < 0)
                            return;
                        weights[i][j][k] = float.Parse(file.ReadLine());
                    }
                }
            }
            for (var i = 0; i < biases.Length; i++)
            {
                for (var j = 0; j < biases[i].Length; j++)
                {
                    if (file.Peek() < 0)
                        return;
                    biases[i][j] = float.Parse(file.ReadLine());
                }
            }
        }
    }
    public void saveNetworkToServer()
    {
        StartCoroutine("SaveNet");
    }
    class NetMatrix {
        public float[] values;
    }
    IEnumerator SaveNet()
    {
        int counter = 0;
        for (var i = 0; i < weights.Length; i++)
        {
            for (var j = 0; j < weights[i].Length; j++)
            {
                for (var k = 0; k < weights[i][j].Length; k++)
                {
                    matrix[counter] = weights[i][j][k];
                    counter++;
                }
            }
        }
        for (var i = 0; i < biases.Length; i++)
        {
            for (var j = 0; j < biases[i].Length; j++)
            {
                matrix[counter] = biases[i][j];
                counter++;
            }
        }
        NetMatrix net = new NetMatrix{
            values = matrix
        };
        string json = JsonConvert.SerializeObject(net);
        var uwr = new UnityWebRequest("", "PUT"); // using POST request with json
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        //Send the request then wait here until it returns 
        yield return uwr.SendWebRequest();
    }
}

