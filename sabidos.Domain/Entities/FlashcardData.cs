using System.Text.Json.Serialization;

public class FlashcardData
{
    [JsonPropertyName("score")]
    public int Score { get; set; }
    
}
