using System;

[Serializable]
public class LevelProgress
{
    public int user_id;
    public int level_number;
    public int stars;
    public bool completed;

    // Constructor for creating a new progress instance
    public LevelProgress(int userId, int levelNumber, int stars, bool completed = true)
    {
        this.user_id = userId;
        this.level_number = levelNumber;
        this.stars = stars;
        this.completed = completed;
    }

    // Default constructor for serialization
    public LevelProgress() { }
} 