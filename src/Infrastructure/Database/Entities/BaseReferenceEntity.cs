using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public abstract class BaseReferenceEntity
    {
        [Key, Required]
        public int Id { get; set; }
        
        [Required, MaxLength(20)]
        public string Name { get; set; }
        
        [Required]
        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset? Updated { get; set; }
    }
}