using Google.Cloud.Firestore;

namespace sabidos.Application.DTOs;

[FirestoreData]
public class UserAchievementDto
{
    [FirestoreProperty]
    public string Id { get; set; }

    [FirestoreProperty]
    public string Title { get; set; }

    [FirestoreProperty]
    public string Description { get; set; }

    [FirestoreProperty]
    public bool Unlocked { get; set; }

    [FirestoreProperty]
    public int Progress { get; set; }

    [FirestoreProperty]
    public int Goal { get; set; }

    [FirestoreProperty]
    public int XpReward { get; set; }
    }