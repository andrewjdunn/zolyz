using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject area;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject wallsParent;

    private const float speed = 3f;
    private Bounds areaBounds;
    private double wallBlockSize;

    private int linePositionTarget = int.MinValue;
    private bool _walkingToLine;
    private ILineForWall lineToBuild;
    private Vector3 lastWallBlockPosition;

    void Start()
    {
        areaBounds = area.GetComponent<BoxCollider>().bounds;
        wallBlockSize = 0.8;//wallPrefab.GetComponent<BoxCollider>().size.normalized.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayerAloneLine();
        //ConstrainPlayerInAreaBounds();
    }
    
    public void DestroyPlayerLine()
    {
        linePositionTarget = int.MinValue;
        lineToBuild.DestroyLine();
    }

    internal interface ILineForWall
    {
        IList<Vector3> Line { get; }
        void DestroyLine();
    }

    internal void StartBuildingWall(ILineForWall lineToBuild)
    {
        this.lineToBuild = lineToBuild;
    }

    // Allow a border around the area for the player to go
    private void ConstrainPlayerInAreaBounds()
    {
        if (transform.position.x < areaBounds.center.x - areaBounds.extents.x)
        {
            transform.position = new Vector3(areaBounds.center.x - areaBounds.extents.x, transform.position.y, transform.position.z);
        }

        if (transform.position.x > areaBounds.center.x + areaBounds.extents.x)
        {
            transform.position = new Vector3(areaBounds.center.x + areaBounds.extents.x, transform.position.y, transform.position.z);
        }

        if (transform.position.z < areaBounds.center.z - areaBounds.extents.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, areaBounds.center.z - areaBounds.extents.z);
        }

        if (transform.position.z > areaBounds.center.z + areaBounds.extents.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, areaBounds.center.z + areaBounds.extents.z);
        }
    }
    private void MovePlayerAloneLine()
    {
        if(lineToBuild.Line?.Count > 0)
        {
            if(linePositionTarget == int.MinValue)
            {
                linePositionTarget = 0;
                _walkingToLine = true;
            }
            else
            {
                // TODO: This check is not quite right - trying to make sure that no blocks get made until we get to the line
                // so we need a walking to line state?

                if(_walkingToLine)
                {
                    _walkingToLine = (transform.position - lineToBuild.Line[linePositionTarget]).magnitude > 1.1;

                }
                
                if((transform.position - lastWallBlockPosition).magnitude > wallBlockSize && !_walkingToLine)
                {
                    CreateAWallSegment(transform.position);
                }

                var targetPoint = lineToBuild.Line[linePositionTarget];
                var diff = (targetPoint - transform.position);
                diff.y = 0;
                var distance = diff.magnitude;
                Debug.Log($"Distance {distance}");
                if (distance < 0.1)
                {
                    Debug.Log("Reached a line point - move to next");

                    ++linePositionTarget;
                    if(linePositionTarget < lineToBuild.Line.Count)
                    {
                        Debug.Log($"Next target is {lineToBuild.Line[linePositionTarget]}");
                    }
                    else
                    {
                        Debug.Log("Done");
                        DestroyPlayerLine();
                    }
                }
            }

            if (linePositionTarget != int.MinValue && lineToBuild.Line?.Count > linePositionTarget)
            {
                var point = lineToBuild.Line[linePositionTarget];
                var seekDirection = point - transform.position;
                seekDirection.y = 0;
                transform.transform.Translate(seekDirection.normalized * Time.deltaTime * speed);
            }
        }
    }

    void CreateAWallSegment(Vector3 position)
    {
        lastWallBlockPosition = new Vector3(position.x, wallPrefab.transform.position.y, position.z);
        Instantiate(wallPrefab, lastWallBlockPosition, /*Orient between line positions?*/wallPrefab.transform.rotation, wallsParent.transform);
    }
}
