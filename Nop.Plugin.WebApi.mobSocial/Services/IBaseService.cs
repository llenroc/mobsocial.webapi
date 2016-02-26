using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Media;
using Mob.Core.Domain;

namespace Nop.Plugin.WebApi.MobSocial.Services
{
    /// <summary>
    /// Generic service to standardize Service APIs
    /// </summary>
    public interface IBaseService<T, P> where T : BaseMobEntity where P : BaseMobEntity
    {

        void Insert(T entity);
        void Delete(T entity);
        void LogicalDelete(int id);
        void Update(T entity);
        T GetById(int id);
        List<T> GetAll();
        List<T> GetAll(string term, int count);

        void InsertPicture(P entity);
        void UpdatePicture(P entity);
        void DeletePicture(P entity);

        P GetPictureById(int id);


        List<P> GetAllPictures(int entityId);
        P GetFirstEntityPicture(int entityId);

        Picture GetFirstPicture(int entityId);


        void UpdateDisplayOrder(int id, int newDisplayOrder);


    }

}
