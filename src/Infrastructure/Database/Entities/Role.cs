using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Database.Entities
{
    [Table("Role", Schema="ref")]
    public class Role : BaseReferenceEntity
    {
        
    }
}