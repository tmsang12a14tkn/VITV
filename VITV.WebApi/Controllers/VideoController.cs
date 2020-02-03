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
    public class VideoController : ApiController
    {
        private readonly IVideoRepository _videoRepository;
        public VideoController()
        {
            _videoRepository = new VideoRepository();
        }
        [HttpGet]
        public IEnumerable<VideoEntity> GetVideoByCategoryId(int id, int from = 0, int no = 10)
        {
            List<VideoEntity> lstvideocategory = null;

            var lstvideo = _videoRepository.GetMany(v => v.CategoryId == id).Skip(from).Take(no);
            lstvideocategory = Mapper.Map<List<VideoEntity>>(lstvideo);
            
            return lstvideocategory;
        }
        [HttpGet]
        public int CountVideoByCategoryId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return 0;
            int idQuery = Convert.ToInt32(id);
            return _videoRepository.GetMany(v => v.CategoryId == idQuery).Count();
        }
        [HttpGet]
        public IEnumerable<VideoEntity> GetListVideo(string from, string no)
        {
            int fromQuery = Convert.ToInt32(from);
            int noQuery = Convert.ToInt32(no);
            var lstvideo = _videoRepository.All.Skip(fromQuery).Take(noQuery);
            var lstvideocategory = Mapper.Map<List<VideoEntity>>(lstvideo);
            return lstvideocategory;
        }
    }
}
