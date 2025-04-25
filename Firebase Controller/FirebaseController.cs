using Firebase.Database;
using Firebase;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Firebase.Auth;

public class FirebaseController : MonoBehaviour
{
    private DatabaseReference databaseReference;
    public static FirebaseController instance;

    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        try
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
#if UNITY_EDITOR
                FirebaseDatabase database = FirebaseDatabase.GetInstance(app, "https://getawaytycoon-default-rtdb.asia-southeast1.firebasedatabase.app/");
                databaseReference = database.RootReference;
#endif

#if UNITY_ANDROID
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
#endif
                Debug.Log("App name: " + FirebaseApp.DefaultInstance.Name);
                Debug.Log("Database URL: " + FirebaseApp.DefaultInstance.Options.DatabaseUrl);
                Debug.Log("Database reference: " + databaseReference.ToString());
                Debug.Log("Firebase Initialized Successfully");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error initializing Firebase: {ex.Message}");
        }
    }
    private void Start()
    {

    }
    // private void ShowloadingPanelInstanceInstance()
    // {

    // }
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
            UserModel newUser = new UserModel(username, 0, 0, "defaultAvatar", "none");

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
            { "currentNetWorth", newNetworth }
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
        string currentUserId = PlayerPrefs.GetString("UserId");
        try
        {
            if (databaseReference == null)
            {
                Debug.LogError("Database reference is null. Ensure Firebase is initialized.");
                return null;
            }

            // Query to get top 50 users sorted by currentNetWorth
            var snapshot = await databaseReference.Child("users")
                .OrderByChild("currentNetWorth")
                .LimitToLast(50) // Get the top 50 users
                .GetValueAsync();

            List<LeaderboardPlayerDetails> users = new List<LeaderboardPlayerDetails>();
            LeaderboardPlayerDetails currentUserDetails = null;

            if (snapshot.Exists)
            {
                foreach (var childSnapshot in snapshot.Children)
                {
                    string json = childSnapshot.GetRawJsonValue();
                    LeaderboardPlayerDetails user = JsonUtility.FromJson<LeaderboardPlayerDetails>(json);
                    user.userId = childSnapshot.Key;
                    users.Add(user);
                }

                // Sort in descending order (Firebase returns ascending order by default)
                users.Sort((a, b) => b.currentNetWorth.CompareTo(a.currentNetWorth));

                // Assign ranks to the top 50 users
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].rank = i + 1; // Rank is 1-based
                }
            }

            // Fetch current user details if not already in the top 50
            if (users.Find(u => u.userId == currentUserId) == null)
            {
                var userSnapshot = await databaseReference.Child("users").Child(currentUserId).GetValueAsync();
                if (userSnapshot.Exists)
                {
                    string json = userSnapshot.GetRawJsonValue();
                    currentUserDetails = JsonUtility.FromJson<LeaderboardPlayerDetails>(json);
                    currentUserDetails.userId = currentUserId;

                    // Query to calculate the rank of the current user
                    var rankSnapshot = await databaseReference.Child("users")
                        .OrderByChild("currentNetWorth")
                        .StartAt(currentUserDetails.currentNetWorth)
                        .GetValueAsync();

                    if (rankSnapshot.Exists)
                    {
                        currentUserDetails.rank = (int)rankSnapshot.ChildrenCount; // Rank is the count of users with equal or greater net worth
                    }

                    // Add the current user as the 51st element
                    users.Add(currentUserDetails);
                }
            }

            return users;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error retrieving top users by networth: " + ex.Message);
            return null;
        }
    }
    public async Task<List<LeaderboardPlayerDetails>> GetTopUsersByXPLevel()
    {
        string currentUserId = PlayerPrefs.GetString("UserId");
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

            List<LeaderboardPlayerDetails> users = new List<LeaderboardPlayerDetails>();
            LeaderboardPlayerDetails currentUserDetails = null;

            if (snapshot.Exists)
            {
                foreach (var childSnapshot in snapshot.Children)
                {
                    string json = childSnapshot.GetRawJsonValue();
                    LeaderboardPlayerDetails user = JsonUtility.FromJson<LeaderboardPlayerDetails>(json);
                    user.userId = childSnapshot.Key; // Assign the user ID from the snapshot key
                    users.Add(user);
                }

                // Sort in descending order (Firebase returns ascending order by default)
                users.Sort((a, b) => b.xpLevel.CompareTo(a.xpLevel));

                // Assign ranks to the top 50 users
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].rank = i + 1; // Rank is 1-based
                }
            }

            // Fetch current user details if not already in the top 50
            if (users.Find(u => u.userId == currentUserId) == null)
            {
                var userSnapshot = await databaseReference.Child("users").Child(currentUserId).GetValueAsync();
                if (userSnapshot.Exists)
                {
                    string json = userSnapshot.GetRawJsonValue();
                    currentUserDetails = JsonUtility.FromJson<LeaderboardPlayerDetails>(json);
                    currentUserDetails.userId = currentUserId;

                    // Query to calculate the rank of the current user
                    var rankSnapshot = await databaseReference.Child("users")
                        .OrderByChild("xpLevel")
                        .StartAt(currentUserDetails.xpLevel)
                        .GetValueAsync();

                    if (rankSnapshot.Exists)
                    {
                        currentUserDetails.rank = (int)rankSnapshot.ChildrenCount; // Rank is the count of users with equal or greater XP level
                    }

                    // Add the current user as the 51st element
                    users.Add(currentUserDetails);
                }
            }

            return users;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error retrieving top users by XP Level: " + ex.Message);
            return null;
        }
    }

    public async Task UpdateProfile(string userId, string newAvatar, string newFrame)
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
                Debug.LogError("User ID is null or empty. Cannot update profile.");
                return;
            }

            // Create a dictionary to update the avatar and frame fields
            var updates = new Dictionary<string, object>
        {
            { "avatarId", newAvatar },
            { "frameId", newFrame }
        };

            // Update the user's avatar and frame in the database
            await databaseReference.Child("users").Child(userId).UpdateChildrenAsync(updates);
            Debug.Log($"Successfully updated profile for user ID: {userId}");

        }
        catch (System.Exception ex)
        {

            Debug.LogError("Error updating profile: " + ex.Message);
        }
    }
}