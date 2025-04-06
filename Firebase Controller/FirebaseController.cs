using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseController : MonoBehaviour
{
    private DatabaseReference databaseReference;
    public static FirebaseController instance;

    void Awake()
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
            UserModel newUser = new UserModel(username, 0, 0, "defaultAvatar", "defaultFrame");

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

    public async Task UpdateCurrentNetworth(string userId, int newNetworth)
    {
        try
        {
            if (databaseReference == null)
            {
                Debug.LogError("Database reference is null. Ensure Firebase is initialized.");
                return;
            }

            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("User ID is null or empty. Cannot update net worth.");
                return;
            }

            // Create a dictionary to update the CurrentNetworth field
            var updates = new Dictionary<string, object>
        {
            { "currentNetworth", newNetworth }
        };

            // Update the user's CurrentNetworth in the database
            await databaseReference.Child("users").Child(userId).UpdateChildrenAsync(updates);
            Debug.Log($"Successfully updated CurrentNetworth to {newNetworth} for user ID: {userId}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error updating CurrentNetworth: " + ex.Message);
        }
    }
    public async Task UpdateXPLevel(string userId, int newLevel)
    {
        try
        {
            if (databaseReference == null)
            {
                Debug.LogError("Database reference is null. Ensure Firebase is initialized.");
                return;
            }

            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("User ID is null or empty. Cannot update XP Level.");
                return;
            }

            // Create a dictionary to update the CurrentLevel field
            var updates = new Dictionary<string, object>
        {
            { "xpLevel", newLevel }
        };

            // Update the user's CurrentLevel in the database
            await databaseReference.Child("users").Child(userId).UpdateChildrenAsync(updates);
            Debug.Log($"Successfully updated CurrentLevel to {newLevel} for user ID: {userId}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error updating CurrentLevel: " + ex.Message);
        }
    }

    public async Task<List<LeaderboardPlayerDetails>> GetTopUsersByNetworth()
    {
        try
        {
            if (databaseReference == null)
            {
                Debug.LogError("Database reference is null. Ensure Firebase is initialized.");
                return null;
            }

            // Query to get top 50 users sorted by totalNetworth
            var snapshot = await databaseReference.Child("users")
                .OrderByChild("currentNetworth")
                .LimitToLast(50) // Get the top 50 users
                .GetValueAsync();

            if (snapshot.Exists)
            {
                List<LeaderboardPlayerDetails> users = new List<LeaderboardPlayerDetails>();
                foreach (var childSnapshot in snapshot.Children)
                {
                    string json = childSnapshot.GetRawJsonValue();
                    LeaderboardPlayerDetails user = JsonUtility.FromJson<LeaderboardPlayerDetails>(json);
                    users.Add(user);
                }

                // Sort in descending order (Firebase returns ascending order by default)
                users.Sort((a, b) => b.currentNetworth.CompareTo(a.currentNetworth));
                return users;
            }
            else
            {
                Debug.LogWarning("No users found in the database.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error retrieving top users by networth: " + ex.Message);
            return null;
        }
    }

    public async Task<List<LeaderboardPlayerDetails>> GetTopUsersByXPLevel()
    {
        try
        {
            if (databaseReference == null)
            {
                Debug.LogError("Database reference is null. Ensure Firebase is initialized.");
                return null;
            }

            // Query to get top 50 users sorted by xpLevel
            var snapshot = await databaseReference.Child("users")
                .OrderByChild("xpLevel")
                .LimitToLast(50) // Get the top 50 users
                .GetValueAsync();

            if (snapshot.Exists)
            {
                List<LeaderboardPlayerDetails> users = new List<LeaderboardPlayerDetails>();
                foreach (var childSnapshot in snapshot.Children)
                {
                    string json = childSnapshot.GetRawJsonValue();
                    LeaderboardPlayerDetails user = JsonUtility.FromJson<LeaderboardPlayerDetails>(json);
                    users.Add(user);
                }

                // Sort in descending order (Firebase returns ascending order by default)
                users.Sort((a, b) => b.xpLevel.CompareTo(a.xpLevel));
                return users;
            }
            else
            {
                Debug.LogWarning("No users found in the database.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error retrieving top users by XP Level: " + ex.Message);
            return null;
        }
    }
}