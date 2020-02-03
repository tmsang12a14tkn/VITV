using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.DataService.Contract;
using VITV.DataService.Entity;

namespace VITV.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CategoryService : ICategoryService
    {
        private readonly IVideoCategoryRepository _videocategoryRepository;
        public CategoryService()
        {
            _videocategoryRepository = new VideoCategoryRepository();
        }
        public IEnumerable<VideoCategoryEntity> GetVideoCategoryByGroupId(string id)
        {
            
            int catid = Convert.ToInt32(id);
            var lstvideo = _videocategoryRepository.GetMany(v => v.GroupId == catid, v=>v.Order);
            var lstvideocategory = Mapper.Map<List<VideoCategoryEntity>>(lstvideo);
            return lstvideocategory;
        }
    }
}
