using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.ViewModels;

namespace VITV.Data.Repositories
{
    public class VideoTranscriptRepository : IVideoTranscriptRepository
    {
        private readonly VITVContext _context;

        public VideoTranscriptRepository(VITVContext context)
        {
            _context = context;
        }

        public VideoTranscriptRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<VideoTranscript> All
        {
            get { return _context.VideoTranscripts.OrderBy(vt => vt.ConvertedToSeconds); }
        }

        public IQueryable<VideoTranscript> AllIncluding(params Expression<Func<VideoTranscript, object>>[] includeProperties)
        {
            IQueryable<VideoTranscript> query = _context.VideoTranscripts.OrderBy(vt => vt.ConvertedToSeconds);
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(VideoTranscript videoTranscript, Expression<Func<VideoTranscript, ICollection<TElement>>> includeProperty) where TElement : class
        {
            var oldVT = _context.VideoTranscripts.Find(videoTranscript.Id);
            if (oldVT != null)
                _context.Entry(oldVT).State = EntityState.Detached;

            _context.VideoTranscripts.Attach(videoTranscript);
            _context.Entry(videoTranscript).Collection(includeProperty).Load();

        }

        public DbPropertyEntry GetProperty<TProperty>(VideoTranscript videoTranscript, Expression<Func<VideoTranscript, TProperty>> property)
        {
            return _context.Entry(videoTranscript).Property(property);
        }

        public VideoTranscript Find(int id)
        {
            return _context.VideoTranscripts.Find(id);
        }

        public IQueryable<VideoTranscript> GetMany(Expression<Func<VideoTranscript, bool>> where)
        {
            return _context.VideoTranscripts.Where(where).OrderBy(v => v.ConvertedToSeconds);
        }

        public void InsertOrUpdate(VideoTranscript videoTranscript)
        {
            if (videoTranscript.Id == default(int))
            {
                // New entity
                _context.VideoTranscripts.Add(videoTranscript);
            }
            else
            {
                // Existing entity
                _context.Entry(videoTranscript).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var videoTranscript = _context.VideoTranscripts.Find(id);
            _context.VideoTranscripts.Remove(videoTranscript);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public interface IVideoTranscriptRepository : IDisposable
    {
        IQueryable<VideoTranscript> All { get; }
        IQueryable<VideoTranscript> AllIncluding(params Expression<Func<VideoTranscript, object>>[] includeProperties);
        void Load<TElement>(VideoTranscript videoTranscript, Expression<Func<VideoTranscript, ICollection<TElement>>> includeProperty) where TElement : class;
        VideoTranscript Find(int id);
        DbPropertyEntry GetProperty<TProperty>(VideoTranscript videoTranscript, Expression<Func<VideoTranscript, TProperty>> property);
        IQueryable<VideoTranscript> GetMany(Expression<Func<VideoTranscript, bool>> where);
        void InsertOrUpdate(VideoTranscript videoTranscript);
        void Delete(int id);
        void Save();
    }
}
