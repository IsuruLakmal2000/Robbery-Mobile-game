using UnityEngine;

public class GarageSidePanelController : MonoBehaviour
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
            animator.SetBool("SidePanelOpen", false);
        }
        else
        {
            animator.SetBool("SidePanelOpen", true);
        }
        isPanelVisible = !isPanelVisible;
    }


}