using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionVision : MonoBehaviour
{

    [SerializeField] public bool nearBuilding = false;
    [SerializeField] private Vector3 detour;
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Wendigo>().target;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            Debug.Log("See building");
            nearBuilding = true;
            detour = GetNearestChild(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            nearBuilding = false;
        }
    }

    public bool isNearBuilding()
    {
        return nearBuilding;
    }

    public Vector3 getDetour()
    {
        return detour;
    }

    private Vector3 GetNearestChild(Transform cTransform)
    {
        if (player.position.x > cTransform.position.x)
        {
            if (player.position.y > cTransform.position.y)
            {
                return cTransform.GetChild(0).position;
            }
            else
            {
                return cTransform.GetChild(1).position;
            }
        }
        else
        {
            if (player.position.y < cTransform.position.y)
            {
                return cTransform.GetChild(2).position;
            }
            else
            {
                return cTransform.GetChild(3).position;
            }
        }
    }
}
