using System.Collections.Generic;
using UnityEngine;

public class SafeArea
{

    private const int AreaXResolution = 1000;
    private const int AreaZResolution = 1000;

    public Vector3 SizeOfBlock { get; private set; }
    private bool[,] BlockStates = new bool[AreaXResolution, AreaZResolution];
    private readonly Bounds gameBounds;
    private readonly Vector3 borderSize;

    public SafeArea(Bounds bounds, Vector3 borderSize)
    {
        gameBounds = bounds;
        this.borderSize = borderSize;
        var blockSizeX = bounds.size.x / AreaXResolution;
        var blockSizeZ = bounds.size.z / AreaZResolution;
        SizeOfBlock = new Vector3(blockSizeX, 1, blockSizeZ);

        for (int x = 0; x < AreaXResolution; x++)
        {
            for (int z = 0; z < AreaZResolution; z++)
            {
                BlockStates[x, z] = IsBorderBlock(x, z);
            }
        }
    }

    private bool IsBorderBlock(int x, int z)
    {
        var xBorderBlocks = AreaXResolution * borderSize.x;
        var zBorderBlocks = AreaZResolution * borderSize.z;
        return (x <= xBorderBlocks || x >= AreaXResolution - xBorderBlocks) && (z <= zBorderBlocks || z >= AreaZResolution - zBorderBlocks);
    }

    public IList<Vector3> CreateSafeAreaOverlay()
    {
        return new List<Vector3>();
    }
}

