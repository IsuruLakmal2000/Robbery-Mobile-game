using UnityEngine;

public class GunsPanelController : MonoBehaviour
{
    private Animator animator;
    private bool isPanelVisible = false;

    public static GunsPanelController instance;

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