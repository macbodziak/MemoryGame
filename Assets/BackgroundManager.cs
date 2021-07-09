using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private void Start() 
    {
        AdjustSize();    
    }

    public void AdjustSize()
    {
        Vector3 prevScale = transform.localScale;
        float newSize = Camera.main.orthographicSize * 2.0f;
        
        if (Camera.main.aspect > 1)
        {
            newSize *= Camera.main.aspect;
        }
        transform.localScale = new Vector3(newSize, newSize, 1.0f); 
    }
}
