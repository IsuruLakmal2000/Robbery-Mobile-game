[System.Serializable]
public class UserModel
{
    public string username;
    public int score;
    public int level;

    public UserModel(string username, int score, int level)
    {
        this.username = username;
        this.score = score;
        this.level = level;
    }
}