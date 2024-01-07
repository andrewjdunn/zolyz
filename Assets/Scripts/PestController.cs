using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestController : MonoBehaviour
{
    public float speed = 7.0f;
    private Bounds areaBounds;    
    private PlayerController playerController;

    void Start()
    {
        // TODO: Fragile... if this cannot be found all the other setup fails.. can we throw excetions?
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        var areaGameObject = GameObject.Find("Area");
        var collider = areaGameObject.GetComponent<Collider>();
        areaBounds = collider.bounds;

        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        StartCoroutine(ChangeDirection());
    }

    private IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds( Random.Range(10, 60));

        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        StartCoroutine(ChangeDirection());
    }

    // Update is called once per frame
    void Update()
    {        
        if(transform.position.z > areaBounds.extents.z)
        {
            BounceOffAngle(90);
        }
        else if(transform.position.x >  areaBounds.extents.x)
        {
            BounceOffAngle(180);
        }
        else if (transform.position.z < -areaBounds.extents.z)
        {
            BounceOffAngle(270);

        }
        else if (transform.position.x < -areaBounds.extents.x)
        {
            BounceOffAngle(0);
        }
        transform.Translate(speed * Time.deltaTime * transform.forward);
    }

    private void BounceOffAngle(float wallAngle)
    {        
        var oldAngle = transform.rotation.eulerAngles.y;
        var newAngle = wallAngle - oldAngle;
        //Debug.Log($"(Old Angle {oldAngle} New Angle {newAngle} Wall Angle {wallAngle}");
        transform.rotation = Quaternion.Euler(0, newAngle, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Other {other.transform.root.gameObject.name}");
        if(other.gameObject.CompareTag("Line"))
        {
            // No longer makes sense - this "Line" is now the wall - need to check for collision with the line renderer if pos?
            //playerController.DestroyPlayerLine();
        }
        else if (other.transform.root.CompareTag("Player"))
        {
            playerController.TakePlayerLife();
        }
        else if(other.transform.CompareTag("Wall"))
        {
            // TODO: Calculate the angle (or should we go back to using rigid boy and a nav agent??)
            // yeah needs a change - the animals just kinda bounce through the wall.. which is too small and floats a bit.. but still gett closer.
            BounceOffAngle(180);
        }
    }
}
