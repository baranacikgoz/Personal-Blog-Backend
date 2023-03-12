
namespace Application.Exceptions;

#pragma warning disable RCS1194
public sealed class EntityNotFoundException : Exception
{
    //public EntityNotFoundException(T entity) : base(String.Join("{typeof(T).Name} with id {id} not found", typeof(T).Name, entity.Id))
    //{
    //}

    public string EntityHashedId { get; }
    public string EntityType { get; }

    public EntityNotFoundException(string entityHashedId, string entityType) : base($"{entityType} with Id {entityHashedId} is not found.")
    {
        EntityHashedId = entityHashedId;
        EntityType = entityType;
    }
}