using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    public AudioSource audioSource;
    public AudioClip buttonClick;
    public AudioClip alertInfoSound;
    public AudioClip upgradeSound;
    public AudioClip moneyPickupSound;
    public AudioClip moneyIncreaseSound;
    public AudioClip alertWarnSound;
    public AudioClip firingSound;
    public AudioClip levelComplete;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }
    public void PlayAlertInfoSound()
    {
        audioSource.PlayOneShot(alertInfoSound);
    }
    public void PlayAlertWarnSound()
    {
        audioSource.PlayOneShot(alertWarnSound);
    }
    public void PlayUpgradeSound()
    {
        audioSource.PlayOneShot(upgradeSound);
    }
    public void PlayMoneyPickupSound()
    {
        audioSource.PlayOneShot(moneyPickupSound);
    }

    public void PlayFireBulletSound()
    {
        audioSource.PlayOneShot(firingSound);
    }

    public void ToggleSound(bool isOn)
    {

        audioSource.mute = !isOn;
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
    }
}