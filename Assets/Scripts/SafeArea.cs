using System.Collections.Generic;
using UnityEngine;

public class SafeArea
{

    private const int AreaXResolution = 1000;
    private const int AreaZResolution = 1000;

    public Vector3 SizeOfBlock { get; private set; }
    private bool[,] SafeAreaSwitches = new bool[AreaXResolution, AreaZResolution];
    private readonly Bounds gameBounds;

    public SafeArea(Bounds bounds)
    {
        gameBounds = bounds;

        var blockSizeX = bounds.size.x / AreaXResolution;
        var blockSizeZ = bounds.size.z / AreaZResolution;
        SizeOfBlock = new Vector3(blockSizeX, 1, blockSizeZ);

        // TOOO: Do we need to set all to false? propably alreayd initialise to false... check
        for (int x = 0; x < AreaXResolution; x++)
        {
            for (int z = 0; z < AreaZResolution; z++)
            {

                // Set a border of safe blocks - for now using a fixed with - later calculate that thickness using area bounds / res + a constant?
                SafeAreaSwitches[x, z] = IsBorderBlock(x, z);
            }
        }
    }

    private bool IsBorderBlock(int x, int z)
    {
        return false;
    }

    public IList<Vector3> CreateSafeAreaOverlay()
    {
        int x = 0;
        int z = 0;

        // Find starting safe area block
        while ( z < AreaZResolution && !SafeAreaSwitches[x, z])
        {
            ++x;
            if(x >= AreaXResolution)
            {
                x = 0;
                z++;
            }
        }

        if(z < AreaZResolution)
        {
            Debug.Log($"First safe area block is {x}:{z}");
        }
        else
        {
            Debug.Log("No safe area found");
        }

        return new List<Vector3>();
    }
}

