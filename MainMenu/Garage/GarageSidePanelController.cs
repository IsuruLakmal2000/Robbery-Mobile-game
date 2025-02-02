using UnityEngine;
using UnityEngine.UI;

public class GarageSidePanelController : MonoBehaviour
{
    private Animator animator;
    private bool isPanelVisible = false;
    [SerializeField] private Button vehiclesSelectBtn;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Button gunsBtn;
    [SerializeField] private GameObject vehiclePanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject gunsPanel;


    void Start()
    {
        animator = GetComponent<Animator>();
        vehiclesSelectBtn.onClick.AddListener(OnVehiclesSelectBtnClick);
        upgradeBtn.onClick.AddListener(OnUpgradeBtnClick);
        gunsBtn.onClick.AddListener(OnGunsBtnClick);
    }
    private void OnVehiclesSelectBtnClick()
    {
        Debug.Log("on vehicles select clicked ");
        vehiclePanel.GetComponent<VehiclePanelController>().OpenPanel();
        upgradePanel.GetComponent<UpgradePanelController>().ClosePanel();
        gunsPanel.GetComponent<GunsPanelController>().ClosePanel();
    }
    private void OnUpgradeBtnClick()
    {
        Debug.Log("on upgrade clicked ");
        vehiclePanel.GetComponent<VehiclePanelController>().ClosePanel();
        upgradePanel.GetComponent<UpgradePanelController>().OpenPanel();
        gunsPanel.GetComponent<GunsPanelController>().ClosePanel();
    }
    private void OnGunsBtnClick()
    {
        Debug.Log("on guns clicked ");
        vehiclePanel.GetComponent<VehiclePanelController>().ClosePanel();
        upgradePanel.GetComponent<UpgradePanelController>().ClosePanel();
        gunsPanel.GetComponent<GunsPanelController>().OpenPanel();
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