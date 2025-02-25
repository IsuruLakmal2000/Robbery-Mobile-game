using UnityEngine;

public class UpgradePanelController : MonoBehaviour
{
    private Animator animator;
    private bool isPanelVisible = false;
    public static UpgradePanelController instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void OpenPanel()
    {
        animator.SetBool("isVehiclePanelOpen", true);
        isPanelVisible = true;
    }
    public void ClosePanel()
    {
        animator.SetBool("isVehiclePanelOpen", false);
        isPanelVisible = false;
    }


}