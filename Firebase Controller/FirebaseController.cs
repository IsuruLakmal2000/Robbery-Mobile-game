using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseController : MonoBehaviour
{
    private DatabaseReference databaseReference;
    public static FirebaseController instance;

    void Awake()
    {
        instance = this;
    }
    private async void Start()
    {
        // Check and fix Firebase dependencies
        var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            // Firebase is ready to use
            var app = Firebase.FirebaseApp.DefaultInstance;

            // Initialize the database reference
            FirebaseDatabase database = FirebaseDatabase.GetInstance(app, "https://getawaytycoon-default-rtdb.asia-southeast1.firebasedatabase.app");
            databaseReference = database.RootReference;

            Debug.Log("Firebase initialized successfully.");
        }
        else
        {
            // Log an error if Firebase dependencies are not resolved
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }
    }

    public async Task SaveNewUser(string username)
    {
        try
        {
            if (databaseReference == null)
            {
                Debug.LogError("Database reference is null. Ensure Firebase is initialized.");
                return;
            }

            // Create a unique ID for the user
            string userId = databaseReference.Child("users").Push().Key;
            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("Failed to generate a unique user ID.");
                return;
            }

            // Create a new user model
            UserModel newUser = new UserModel(username, 0, 0);

            // Convert the user model to JSON and save it under the unique ID
            string json = JsonUtility.ToJson(newUser);
            Debug.Log("Saving user data: " + json);

            await databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
            PlayerPrefs.SetInt("IsFirstTime", 0);
            PlayerPrefs.SetString("UserId", userId); // Save the user ID in PlayerPrefs
            PlayerPrefs.SetString("UserName", username); // Save the username in PlayerPrefs
            Debug.Log("User saved successfully with ID: " + userId);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving user: " + ex.Message);
        }
    }
}