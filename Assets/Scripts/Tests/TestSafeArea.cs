using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSafeArea
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestSafeAreaStartsWithBorder()
    {
        // Setup
        var bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(100, 100));
        SafeArea safeArea = new SafeArea(bounds);

        // TODO: Bordersize is the number of blocks in the border * the size of a block - 10 is 1%...should blocks in border be exposed? or passed in..? 
        var BorderSizeX = 10 * safeArea.SizeOfBlock.x;
        var BorderSizeY = 10 * safeArea.SizeOfBlock.y;
        // Expected overlay vector describes the inner and outer loop of the border - duplicated with a Y offset to make a block
        // TODO: I'm not sure what is needed to make a mesh so this might change a lot :-)
        var height = 1;
        var expetedOverlayVector = new List<Vector3>
        {
            new Vector3(-100 + BorderSizeX, 0, -100 + BorderSizeY),
            new Vector3(100 - BorderSizeX, 0, -100 + BorderSizeY),
            new Vector3(100 - BorderSizeX, 0, 100 - BorderSizeY),
            new Vector3(-100 + BorderSizeX, 0, 100 - BorderSizeY),
            new Vector3(-100 + BorderSizeX, 0, -100 + BorderSizeY),
            new Vector3(-100, 0, -100),
            new Vector3(100, 0, -100),
            new Vector3(100, 0, 100),
            new Vector3(-100, 0, 100),
            new Vector3(-100, 0, -100),
            new Vector3(-100 + BorderSizeX, height, -100 + BorderSizeY),
            new Vector3(100 - BorderSizeX, height, -100 + BorderSizeY),
            new Vector3(100 - BorderSizeX, height, 100 - BorderSizeY),
            new Vector3(-100 + BorderSizeX, height, 100 - BorderSizeY),
            new Vector3(-100 + BorderSizeX, height, -100 + BorderSizeY),
            new Vector3(-100, height, -100),
            new Vector3(100, height, -100),
            new Vector3(100, height, 100),
            new Vector3(-100, height, 100),
            new Vector3(-100, height, -100)
        };

        // Test
        var overlayVectors = safeArea.CreateSafeAreaOverlay();

        // Verify
        Assert.AreEqual(expetedOverlayVector, overlayVectors);

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
