using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VITV.Data.Repositories;
using VITV.DataService.Contract;
using VITV.DataService.Entity;

namespace VITV.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VideoService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select VideoService.svc or VideoService.svc.cs at the Solution Explorer and start debugging.
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;
        public VideoService()
        {
            _videoRepository = new VideoRepository();
        }
        public IEnumerable<VideoEntity> GetVideoByCategoryId(string id, string from = "", string no = "1")
        {
            List<VideoEntity> lstvideocategory = null;
            if(string.IsNullOrEmpty(id))
                return lstvideocategory;
            
            if(string.IsNullOrEmpty(from) || string.IsNullOrEmpty(no))
            {
                int idQuery = Convert.ToInt32(id);
                int noQuery = Convert.ToInt32(no);
                var lstvideo = _videoRepository.GetMany(v => v.CategoryId == idQuery).Skip(0).Take(noQuery);
                lstvideocategory = Mapper.Map<List<VideoEntity>>(lstvideo);
            }else
            {
                int idQuery = Convert.ToInt32(id);
                int fromQuery = Convert.ToInt32(from);
                int noQuery = Convert.ToInt32(no);
                var lstvideo = _videoRepository.GetMany(v => v.CategoryId == idQuery).Skip(fromQuery).Take(noQuery);
                lstvideocategory = Mapper.Map<List<VideoEntity>>(lstvideo);
            }
            return lstvideocategory;
        }

        public int CountVideoByCategoryId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return 0;
            int idQuery = Convert.ToInt32(id);
            return _videoRepository.GetMany(v => v.CategoryId == idQuery).Count();
        }

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
