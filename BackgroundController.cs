using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Renderer meshRenderer;
    public float speed = 0.5f;
    private float stopDuration = 1f; // Duration to stop smoothly
    private bool isStopping = false; // Flag to indicate if stopping
    private float targetSpeed;
    public  Material[] materials;
    public static BackgroundController instance;

    void Awake()
    {
        //  targetSpeed = speed;
        instance = this;
    }
    void Start()
    {
        speed = GameManager.instance.levelConfig.mapRotationSpeed;
        int levelNumber = PlayerPrefs.GetInt("current_level", 1); // Get the level number from PlayerPrefs, default to 1
      //  Material[] materials = GetComponent<Renderer>().materials;

        // Set the material based on the level number
        if (levelNumber < 11)
        {
            meshRenderer.material = materials[0]; // Access the material corresponding to the level number
        }
        else if (levelNumber < 21)
        {
            meshRenderer.material = materials[1]; // Access the material corresponding to the level number
        }
        else if (levelNumber < 31)
        {
            meshRenderer.material = materials[2]; // Access the material corresponding to the level number
        }
        else if (levelNumber < 41)
        {
            meshRenderer.material = materials[2]; // Access the material corresponding to the level number
        }
        else if (levelNumber < 51)
        {
            meshRenderer.material = materials[2]; // Access the material corresponding to the level number
        }
    }

    private void Update()
    {
        // Move the background downwards
        Vector2 offset = meshRenderer.material.mainTextureOffset;
        offset += new Vector2(0, speed * Time.deltaTime);
        meshRenderer.material.mainTextureOffset = offset;

    }

    public void StopBackground()
    {
        isStopping = true;
        StartCoroutine(ResumeAfterDelay()); // Start coroutine to resume after a delay
    }

    private System.Collections.IEnumerator ResumeAfterDelay()
    {
        // Wait for the stop duration before resuming
        yield return new WaitForSeconds(stopDuration);
        ResumeBackground();
    }

    // Method to resume background movement
    public void ResumeBackground()
    {
        targetSpeed = speed; // Resume original speed
    }
}