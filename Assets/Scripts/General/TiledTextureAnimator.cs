using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledTextureAnimator : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // the speed at which to scroll the texture
    public LineRenderer rend; // the renderer component of the object with the tiled texture

    void Start()
    {
        rend = GetComponent<LineRenderer>(); // grab the renderer component
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed; // calculate the offset based on current time and scroll speed
        Vector2 newOffset = new Vector2(offset, 0); // create a new offset vector
        rend.material.SetTextureOffset("_MainTex", newOffset); // set the new offset on the material
    }
}
