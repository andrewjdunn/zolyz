using NUnit.Framework;
using UnityEngine;

public class TestSafeArea
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestSafeAreaStartsWithBorder()
    {
        // Setup
        var bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(100, 0, 100));
        var borderSize = new Vector3 (0.05f, 0, 0.05f);
        SafeArea safeArea = new(bounds, borderSize);

        // TODO: Bordersize is the number of blocks in the border * the size of a block - 10 is 1%...should blocks in border be exposed? or passed in..? 
        var BorderSizeX = borderSize.x / safeArea.SizeOfBlock.x * 10;
        var BorderSizeZ = borderSize.z / safeArea.SizeOfBlock.z * 10;

        var height = 1;

        var expetedOverlayVector = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(100, 0, 0),
            new Vector3(BorderSizeX, 0, BorderSizeZ),
            new Vector3(100 - BorderSizeX, 0, BorderSizeZ),

            new Vector3(BorderSizeX, 0, 100 - BorderSizeZ),
            new Vector3(100 - BorderSizeX, 0, 100 - BorderSizeZ),
            new Vector3(0, 0, 100),
            new Vector3(100, 0, 100),

            new Vector3(0, height, 0),
            new Vector3(0, height, 100),
            new Vector3(BorderSizeX, height, BorderSizeZ),
            new Vector3(BorderSizeX, height, 100 - BorderSizeZ),

            new Vector3(100 - BorderSizeX, height, BorderSizeZ),
            new Vector3(100 - BorderSizeX, height, 100 - BorderSizeZ),
            new Vector3(100, height, 0),
            new Vector3(100, height, 100),
        };

        var expetedOverlayTriangles = new[] {
            // face 1
            0, 9, 6, 0, 8, 9,
            // face 2
            2, 11, 4, 2, 10, 11,
            // face 3
            3, 13, 5, 3, 12, 13,
            // face 4
            1, 15, 7, 1, 14, 15,

            // Face 5
            0, 14, 1, 0, 8, 14,
            // face 6
            2, 12, 3, 2, 10, 12,
            // face 7
            4, 13, 5, 4, 11, 13,
            // face 8
            6, 15, 7, 6, 9, 15
        };

        // Test
        safeArea.CreateSafeAreaOverlay();
        var overlayVectors = safeArea.GetVectorsAsArray();
        var overlayTriangles = safeArea.GetTrianglesAsArray();

        // Verify
        Assert.AreEqual(expetedOverlayVector, overlayVectors);
        Assert.AreEqual(expetedOverlayTriangles, overlayTriangles);

    }


    // TestSafeAreaStartsWith???
    // Add tests for No safe area - one where there is only one block left, and whatever you can thing off fuzz test!


    /*
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestSafeAreaWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
