using Google.Cloud.Firestore;

namespace sabidos.Domain.Entities;

[FirestoreData]
public class UserDailyState
{
    [FirestoreProperty]
    public string AssignedDate { get; set; } // Formato YYYY-MM-DD

    [FirestoreProperty]
    public List<string> ActiveMissionIds { get; set; }

    [FirestoreProperty]
    public List<string> CompletedMissionIds { get; set; }

    [FirestoreProperty]
    public int RerollsUsed { get; set; }
}
