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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReporterService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReporterService.svc or ReporterService.svc.cs at the Solution Explorer and start debugging.
    public class ReporterService : IReporterService
    {
        private readonly IReporterRepository _reporterRepository;
        private readonly IPositionLevelRepository _positionLevelRepository;
        public ReporterService()
        {
            _reporterRepository = new ReporterRepository();
            _positionLevelRepository = new PositionLevelRepository();
        }
        public ReporterToListEntity GetReporterById(string id)
        {
            var reporter = _reporterRepository.Find(Convert.ToInt32(id));
            var reporterEntity = Mapper.Map<ReporterToListEntity>(reporter);
            return reporterEntity;
        }

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
