using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: Dont like the name anymore.. its a UI thing - swipe.. but ok its fo rthe player... 
public class PlayerLine : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject playerLine;
    [SerializeField] private GameObject player;

    private LineRenderer lineRenderer;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = playerLine.GetComponentInChildren<LineRenderer>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyLine()
    {
        lineRenderer.positionCount = 0;
    }

    private void OnMouseDrag()
    {
        
        //if (gameManager.isGameActive && !gameManager.isPaused)
        {
            var worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.y = 1;

            bool addPoint = true;
            if (lineRenderer.positionCount > 0)
            {
                var diff = worldPoint - lineRenderer.GetPosition(lineRenderer.positionCount-1);
                // TODO: Magic number
                addPoint = diff.magnitude > 0.5;
            }

            if (addPoint)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, worldPoint);
            }

        }
    }

    public class LineForWall : PlayerController.ILineForWall
    {
        private IList<Vector3> _linePoints;
        private Action _destroyLine;
        
        public LineForWall(IList<Vector3> linePoints, Action destroyLine)
        {
            _linePoints = linePoints;
            _destroyLine = destroyLine;
        }

        public IList<Vector3> Line => _linePoints;

        public void DestroyLine()
        {
            _linePoints.Clear();
            _destroyLine();
        }
    }

    private void OnMouseUp()
    {
        var points = new List<Vector3>();
        // Deep copy of points that make the line
        for(int i = 0; i < lineRenderer.positionCount; i++)
        {
            var p = lineRenderer.GetPosition(i);
            points.Add(new Vector3(p.x, p.y, p.z));
        }
        playerController.StartBuildingWall(new LineForWall(points, DestroyLine));
    }
}
