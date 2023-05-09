using Entities.Models;
namespace Contracts;

public interface IDataShaperNotHEATOAS<T>
{
    //Not HEATOAS DataShaper
    IEnumerable<Entity> ShapeDataNH(IEnumerable<T> entities, string fieldsString);
    Entity ShapeDataNH(T entity, string fieldsString);
}
