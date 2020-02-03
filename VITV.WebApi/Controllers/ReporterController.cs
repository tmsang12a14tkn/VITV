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
    public class ReporterController : ApiController
    {
        private readonly IReporterRepository _reporterRepository;
        private readonly IPositionLevelRepository _positionLevelRepository;
        public ReporterController()
        {
            _reporterRepository = new ReporterRepository();
            _positionLevelRepository = new PositionLevelRepository();
        }
        [HttpGet]
        public ReporterToListEntity GetReporterById(string id)
        {
            var reporter = _reporterRepository.Find(Convert.ToInt32(id));
            var reporterEntity = Mapper.Map<ReporterToListEntity>(reporter);
            return reporterEntity;
        }
        [HttpGet]
        public IEnumerable<ReporterToListEntity> GetListReporter(string name)
        {
            List<ReporterToListEntity> lstReporterEntity = null;
            
            if(string.IsNullOrEmpty(name))
            {
                var lstReporter = _reporterRepository.All;
                lstReporterEntity = Mapper.Map<List<ReporterToListEntity>>(lstReporter);
            }else
            {
                var lstReporter = _reporterRepository.GetMany(r => r.Name.Contains(name) || r.Position.Name.Contains(name));
                lstReporterEntity = Mapper.Map<List<ReporterToListEntity>>(lstReporter);
            }
            return lstReporterEntity;
        }
        [HttpGet]
        public IEnumerable<ReporterGroupEntity> GetListReporterByGroup()
        {
            List<ReporterGroupEntity> lstReporterGroups = new List<ReporterGroupEntity>();
            foreach (var p in _positionLevelRepository.All)
            {
                var reporters = _reporterRepository.GetMany(r => r.Position.PositionLevelId == p.Id).ToList();
                List<ReporterEntity> lstReporterEntity = Mapper.Map<List<ReporterEntity>>(reporters);
                var rep = new ReporterGroupEntity
                {
                    Id = p.Id,
                    Name = p.Name,
                    Reporters = lstReporterEntity
                };
                lstReporterGroups.Add(rep);
            }
            return lstReporterGroups;
        }
    }
}
