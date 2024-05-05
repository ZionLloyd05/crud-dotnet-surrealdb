using SurrealDb.Net.Models;

namespace SurrealCrud.Models;

public class User : Record
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Age { get; set; } = 0; 
    public bool IsConfirmed { get; set; }
}

public class CreateUserDTO
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Age { get; set; } = 0;
}

public class UpdateUserDTO
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
}