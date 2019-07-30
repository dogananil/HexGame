using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DotTouch : MonoBehaviour
{
    private Vector2 touchPosition;
    private Vector2 finalposition;
    public int index = 0;
    public float swipeAngel = 0;

   
    private string[] split;
    
    // Start is called before the first frame update
    void Start()
    { 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        split = this.transform.name.Split('.');
        
        touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       
        GameManager.instance.SelectHexagons(split,this.index);
    }

    private void OnMouseUp()
    {
        finalposition =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        swipeAngel = Mathf.Atan2(finalposition.y - touchPosition.y, finalposition.x - touchPosition.x)*180*Mathf.PI;
    }
}
