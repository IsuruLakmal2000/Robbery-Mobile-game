using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private CarController carController; // Reference to your CarController script
    public float inputValue; // Set this to -1 for left and 1 for right in the Inspector

    public bool isFireButtonPressed = false;

    public static ButtonControl instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        carController = GameObject.FindWithTag("PlayerCar").GetComponent<CarController>();
    }
    public void LoadScript()
    {
        carController = GameObject.FindWithTag("PlayerCar").GetComponent<CarController>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        carController.SetInput(inputValue); // Set the input based on button pressed
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        carController.SetInput(0f); // Reset input to 0 on release
    }

    public void OnPointerHold(PointerEventData eventData)
    {
        isFireButtonPressed = true;
    }
}