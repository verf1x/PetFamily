namespace PetFamily.Domain.Shared;

public abstract class SoftDeletableEntity<TId> : 
    Entity<TId>,
    ISoftDeletableEntity
    where TId : IEquatable<TId>
{
    public bool IsDeleted { get; private set; }

    public DateTime? DeletionDate { get; private set; } 

    protected SoftDeletableEntity(TId id) : base(id) { }
    
    public void SoftDelete()
    {
        if (IsDeleted)
            return;
        
        IsDeleted = true;
        DeletionDate = DateTime.UtcNow;
    }
    
    public void Restore()
    {
        if (IsDeleted is false)
            return;
        
        IsDeleted = false;
        DeletionDate = null;
    }
}