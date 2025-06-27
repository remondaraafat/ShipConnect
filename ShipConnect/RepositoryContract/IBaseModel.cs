using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipConnect.RepositoryContract
{
    public interface IBaseModel
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
