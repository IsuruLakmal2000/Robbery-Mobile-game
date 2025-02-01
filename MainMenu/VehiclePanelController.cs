using UnityEngine;

public class VehiclePanelController : MonoBehaviour
{
    private Animator animator;
    private bool isPanelVisible = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TogglePanel()
    {
        if (isPanelVisible)
        {
            animator.SetBool("isVehiclePanelOpen", false);
        }
        else
        {
            animator.SetBool("isVehiclePanelOpen", true);
        }
        isPanelVisible = !isPanelVisible;
    }


}