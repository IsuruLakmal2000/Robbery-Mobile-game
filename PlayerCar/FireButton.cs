using UnityEngine;


using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool isFireButtonPressed = false;



    void Start()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        isFireButtonPressed = true; // Set button pressed status
        Debug.Log("Button Pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        isFireButtonPressed = false; // Reset button pressed status
        Debug.Log("Button Released");
    }



    void Update()
    {
        // Optionally call OnPointerHold in Update if needed
        if (isFireButtonPressed)
        {
            GunController.instance.FireBullet();
        }

    }
}