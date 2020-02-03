using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VITV.Data.Repositories;
using VITV.WebApi.Entities;

namespace VITV.WebApi.Controllers
{
    public class CategoryController : ApiController
    {
         private readonly IVideoCategoryRepository _videocategoryRepository;
         public CategoryController()
        {
            _videocategoryRepository = new VideoCategoryRepository();
        }
        [HttpGet]
        public IEnumerable<VideoCategoryEntity> GetVideoCategoryByGroupId(int id)
        {
            var lstvideo = _videocategoryRepository.GetMany(v => v.GroupId == id, v=>v.Order);
            var lstvideocategory = Mapper.Map<List<VideoCategoryEntity>>(lstvideo);
            return lstvideocategory;
        }
    }
}
