namespace SPR_411_Team_1.DAL.Data.Entities
{
    public interface IBaseEntity
    {
        int Id { get; set; }
    }

    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}
