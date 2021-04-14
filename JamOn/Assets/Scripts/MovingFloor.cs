using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Transform> destinationPoints;
    [SerializeField] private Transform platformTransform;
    [SerializeField] private float speed;
    [SerializeField] private Collider2D myCollider;
    private Transform playerTransform;
    private Transform ballTransform;
    private int last=0, next=1;
    private float middlePos=0;
    private bool calculated = false;
 
    private void LateUpdate()
    {
        calculated = false;
        playerTransform = null;
        ballTransform = null;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckObjects();
        Vector2 actualpos = platformTransform.position;
        Vector2 position = Vector2.Lerp(destinationPoints[last].position, destinationPoints[next].position,middlePos);
        float distance = Vector3.Distance(destinationPoints[last].position, destinationPoints[next].position);
        middlePos += (Time.fixedDeltaTime/ distance) *speed;
        if (middlePos > 1)
        {
            middlePos = 0;
            last = (last + 1) % destinationPoints.Count;
            next = (next + 1) % destinationPoints.Count;
        }
        Vector2 positionDif = position - actualpos;
 
        if(playerTransform!=null)
            playerTransform.position = new Vector3(playerTransform.position.x + positionDif.x, playerTransform.position.y + positionDif.y, playerTransform.position.z);
       
        if (ballTransform != null)
            ballTransform.position = new Vector3(ballTransform.position.x + positionDif.x, ballTransform.position.y + positionDif.y, ballTransform.position.z);

        platformTransform.position = position;
    }

    private void CheckObjects()
    {
        if (calculated) return;

        calculated = true;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0.0f);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                playerTransform = colliders[i].transform;
            }
            else if (colliders[i].gameObject.layer == LayerMask.NameToLayer("Egg"))
            {
                ballTransform = colliders[i].transform;
            }
        }
        
    }
    public void ForceCalculate()
    {
        calculated = false;
    }

}
