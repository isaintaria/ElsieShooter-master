using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler
{
    public delegate void ButtonDown();
    public event ButtonDown OnButtonDown;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnButtonDown != null)
            OnButtonDown();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
