using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore;

namespace SecretSipsServer.Models;

public class CreateGameRequest
{
    [Required, Range(1, int.MaxValue)]
    public int Rounds { get; set; }
    
    [Required, GreaterThan(nameof(Rounds))]
    public int MinSecrets { get; set; }
    
    [Required]
    public int TimerLength { get; set; } 
    
    public string UserName {get; set; }
}