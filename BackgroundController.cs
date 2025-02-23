using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Renderer meshRenderer;
    private float speed = 0.5f;
    private float stopDuration = 1f; // Duration to stop smoothly
    private bool isStopping = false; // Flag to indicate if stopping
    private float targetSpeed;
    public static BackgroundController instance;

    void Awake()
    {
        //  targetSpeed = speed;
        instance = this;
    }
    void Start()
    {
        speed = GameManager.instance.levelConfig.mapRotationSpeed;

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