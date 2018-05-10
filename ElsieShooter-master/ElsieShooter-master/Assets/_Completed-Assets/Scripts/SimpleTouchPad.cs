using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTouchPad : MonoBehaviour,IPointerDownHandler, IDragHandler, IPointerUpHandler {


    public float smoothing;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;

    private bool touched;
    private int pointerID;



    void Awake()
    {
        direction = Vector2.zero;
        touched = false;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(! touched )
        {
            touched = true;
            pointerID = eventData.pointerId;
            origin = eventData.position;

            Debug.Log(eventData.position.normalized);

        }
            
    }
    public void OnDrag(PointerEventData eventData)
    {
        if( eventData.pointerId == pointerID )
        {
            Vector2 currentPosition = eventData.position;
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;
        }


        Debug.Log(eventData.position.normalized);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if( eventData.pointerId == pointerID )
        {
            direction = Vector3.zero;
            touched = false;
        }
    }
        

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector2 GetDirection()
    {
        smoothDirection = Vector2.MoveTowards(smoothDirection, direction,smoothing );
        return smoothDirection;
    }
}
