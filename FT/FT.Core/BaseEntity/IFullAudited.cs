namespace HR.Core.BaseEntity
{
    public interface IFullAudited : ICreatedOn, IHasCreator, IModifiedOn, IHasModifier,
         IHasTenant, ISoftDelete, IDeletedOn, IHasDeleter
    {
    }
}
