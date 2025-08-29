using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBehavior : MonoBehaviour
{
    public GameObject BoundingBox;
    public GameObject Cat;
    public GameObject Dog;

    public float MinDistance;
    public float VerticalOffset;
    private BoxCollider2D BoundBoxCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        BoundBoxCollider = BoundingBox.GetComponent<BoxCollider2D>();
        BoundBoxCollider.size = new Vector2(MinDistance, MinDistance);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 midpoint = (Cat.transform.position + Dog.transform.position) / 2;
        float separationDistance = Vector2.Distance(Cat.transform.position, Dog.transform.position);
        BoundingBox.transform.position = new Vector2(midpoint.x, midpoint.y + VerticalOffset);
        if (separationDistance > MinDistance)
        {
            BoundBoxCollider.size = new Vector2(separationDistance, separationDistance);

        }

    }
}
