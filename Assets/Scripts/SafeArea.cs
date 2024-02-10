using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents the parts of the game board that are safe.
/// This class is not thread safe - current access to its properties and methods must not happen
/// </summary>
public class SafeArea
{
    private const int AreaXResolution = 1000;
    private const int AreaZResolution = 1000;

    public Vector3 SizeOfBlock { get; private set; }
    private bool[,] BlockStates = new bool[AreaXResolution, AreaZResolution];

    private readonly Vector3 borderSize;

    private readonly List<Vector3> vectors = new();
    private readonly List<int> triangles = new();

    public SafeArea(Bounds bounds, Vector3 borderSize)
    {
        this.borderSize = borderSize;
        var blockSizeX = bounds.size.x / AreaXResolution;
        var blockSizeZ = bounds.size.z / AreaZResolution;
        SizeOfBlock = new Vector3(blockSizeX, 1, blockSizeZ);

        for (int z = 0; z < AreaZResolution; z++)
        {
            for (int x = 0; x < AreaXResolution; x++)
            {
                BlockStates[x, z] = IsBorderBlock(x, z);
            }
        }
    }

    // TODO: Feel like this should be something passed in - used once at start up (create an interface IInitialSafeBlocks,,,)
    private bool IsBorderBlock(int x, int z)
    {
        var xBorderBlocks = AreaXResolution * borderSize.x;
        var zBorderBlocks = AreaZResolution * borderSize.z;
        var safe = (x < xBorderBlocks || x >= AreaXResolution - xBorderBlocks) || (z < zBorderBlocks || z >= AreaZResolution - zBorderBlocks);
        return safe;
    }

    public void CreateSafeAreaOverlay()
    {
        vectors.Clear();
        triangles.Clear();

        // Create Vectors for 2D plane (Lower faces)
        float lowY = 0;
        for (var z= 0; z <= AreaZResolution; z++)
        {
            for(var x = 0;x <= AreaXResolution; x++)
            {
                if(IsVector(x, z))
                {
                    vectors.Add(CreateVector(x, lowY, z));
                }
            }
        }

        AddVerticalFaces(vectors.OrderBy(v => (v.x * AreaXResolution) + v.z), (v1, v2) => (v2.x - v1.x) < float.Epsilon);
        AddVerticalFaces(vectors.Where( v=> v.y == lowY).OrderBy(v => (v.z * AreaZResolution) + v.x), (v1, v2) => (v2.z - v1.z) < float.Epsilon);
    }

    public Vector3[] GetVectorsAsArray() { return vectors.ToArray(); }
    public int[] GetTrianglesAsArray() { return triangles.ToArray(); }

    private bool IsVector(int x, int z)
    {
        /*
         * Does the x,y intersect have a block at its corner that is not sharing and edge with a block of the same time (safe or not safe)
         *
         *  NW|NE
         *  --o--
         *  SW|SE
         */
        bool NW = IsSafeBlock(x - 1, z - 1);
        bool NE = IsSafeBlock(x, z - 1);
        bool SW = IsSafeBlock(x - 1, z);
        bool SE = IsSafeBlock(x, z);

        bool nwSharing = NW == NE | NW == SW;
        bool neSharing = NE == NW | NE == SE;
        bool seSharing = SE == NE | SE == SW;
        bool swSharing = SW == SE | SW == NW;
        return !nwSharing | !neSharing | !seSharing | !swSharing;
    }

    // I think it is right to consider a block outside the game area as not a safe block (only safe blocks get drawn)
    private bool IsSafeBlock(int x, int z)
    {
        return x >= 0 && z >= 0 && x < AreaXResolution && z < AreaZResolution && BlockStates[x,z];
    }

    private Vector3 CreateVector(int x, float y, int z)
    {
        return new Vector3(x * SizeOfBlock.x, y, z * SizeOfBlock.z);
    }

    private void AddVerticalFaces(IEnumerable<Vector3> query, Func<Vector3, Vector3, bool> IsOnSameLine)
    {
        Vector3 first = Vector3.negativeInfinity;
        foreach (var vector in query)
        {
            if (first == Vector3.negativeInfinity || !IsOnSameLine(first, vector))
            {
                first = vector;
            }
            else
            {
                AddVerticalFace(vectors.IndexOf(first), vectors.IndexOf(vector));
                first = Vector3.negativeInfinity;
            }
        }
    }

    private void AddVerticalFace(int indexOfLowFirst, int indexOfLowSecond)
    {
        float highY = 1;
        // TODO: We get the index - then get the vector! best to pass the vector and then get the index?
        var lowFirst = vectors[indexOfLowFirst];
        var lowSecond = vectors[indexOfLowSecond];

        // Add two new vectors to create a rectangle
        var highFirst = new Vector3(lowFirst.x, highY, lowFirst.z);
        var highSecond = new Vector3(lowSecond.x, highY, lowSecond.z);

        if (!vectors.Contains(highFirst))
        {
            vectors.Add(highFirst);
        }

        var indexOfHighFirst = vectors.IndexOf(highFirst);

        if(!vectors.Contains(highSecond))
        {
            vectors.Add(highSecond);
        }

        var indexOfHighSecond = vectors.IndexOf(highSecond);

        // Add Trianges for the new face
        triangles.AddRange(new int[] { indexOfLowFirst, indexOfHighSecond, indexOfLowSecond });
        triangles.AddRange(new int[] { indexOfLowFirst, indexOfHighFirst, indexOfHighSecond });
    }
}

