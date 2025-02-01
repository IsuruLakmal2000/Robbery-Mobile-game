using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CarController carController; // Reference to your CarController script
    public float inputValue; // Set this to -1 for left and 1 for right in the Inspector

    public void OnPointerDown(PointerEventData eventData)
    {
        carController.SetInput(inputValue); // Set the input based on button pressed
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        carController.SetInput(0f); // Reset input to 0 on release
    }
}