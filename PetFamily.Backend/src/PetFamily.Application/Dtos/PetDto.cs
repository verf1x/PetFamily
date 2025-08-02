namespace PetFamily.Application.Dtos;

public class PetDto
{
    public Guid Id { get; set; }
    
    public Guid VolunteerId { get; set; }
    
    public string Nickname { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public int Position { get; set; }
    
    public string Color { get; set; } = null!;

    public PetFileDto[] Photos { get; set; } = null!;
}

public class PetFileDto
{
    public string FilePath { get; init; } = string.Empty;
}