namespace Entities.LinkModels
{
    public class LinkResponse<TLinkedEntity, TEntity> 
        where TLinkedEntity : class
        where TEntity : class
    {
        public bool HasLinks { get; set; }
        public List<TEntity> Entities { get; set; }
        public LinkCollectionWrapper<TLinkedEntity> LinkedEntities { get; set; }
        public LinkResponse()
        {
            LinkedEntities = new LinkCollectionWrapper<TLinkedEntity>();
            Entities = new List<TEntity>();
        }
    }
}
