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
    public AudioClip popSoundEffect;
    public AudioClip firingSound;
    public AudioClip levelComplete;
    public AudioClip walkieTalkieSound;
    public AudioClip levelFailed
    ;
    [SerializeField] private AudioClip[] policeCarDestroyedSound;

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

    public void PlayPopSound()
    {
        audioSource.PlayOneShot(popSoundEffect);
    }

    public void PlayLevelFailedSound()
    {
        audioSource.PlayOneShot(alertWarnSound);
        audioSource.PlayOneShot(walkieTalkieSound);
    }

    public void PlayLevelCompleteSound()
    {
        audioSource.PlayOneShot(levelComplete);
    }
    public void PlayVehicleDestroyedSound()
    {
        int randomIndex = Random.Range(0, policeCarDestroyedSound.Length);
        audioSource.PlayOneShot(policeCarDestroyedSound[randomIndex]);
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
        audioSource.clip = moneyPickupSound;
        audioSource.volume = 0.3f;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayMoneyIncreaseSound()
    {
        audioSource.clip = moneyIncreaseSound;
        audioSource.volume = 1f;
        audioSource.loop = false;
        audioSource.Play();
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