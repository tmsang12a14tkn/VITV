using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly VITVContext _context;

        public PollRepository(VITVContext context)
        {
            _context = context;
        }

        public PollRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<PollQuestion> All
        {
            get { return _context.PollQuestions; }
        }

        public IQueryable<PollQuestion> AllIncluding(params Expression<Func<PollQuestion, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<PollQuestion, object>>, IQueryable<PollQuestion>>(_context.PollQuestions, (current, includeProperty) => current.Include(includeProperty));
        }

        public PollQuestion Find(int id)
        {
            var poll = _context.PollQuestions.Find(id);
            poll.Answers.ToList().ForEach(pa => {
                pa.Count = _context.PollUserAnswers.Count(ua => ua.QuestionId == pa.QuestionId && ua.AnswerId == pa.AnswerId);
            });
            return poll;
        }

        public PollUserAnswer FindUserAnswer(int questionId, int order, string ip)
        {
            return _context.PollUserAnswers.Find(questionId, order, ip);
        }
        public bool CheckUserAnswered(int questionId, string ip)
        {
            return _context.PollUserAnswers.Any(ua => ua.QuestionId == questionId && ua.IP == ip);
        }
        public IEnumerable<PollQuestion> VideoQuestions(int videoId)
        {
            var now = DateTime.Now.Date;
            //pq.FromDate <= now && (pq.EndDate == null || pq.EndDate >= now)
            return _context.PollQuestions.Where(pq => pq.Published && pq.VideoId == videoId).ToList();
        }
        public IEnumerable<PollAnswer> GetAnswers(int questionId)
        {
            return _context.PollAnswers.Where(a => a.QuestionId == questionId).ToList();
        }
        public IEnumerable<PollUserAnswer> GetUserAnswers(int questionId)
        {
            return _context.PollUserAnswers.Where(a => a.QuestionId == questionId).ToList();
        }

        public PollQuestion QuestionAnswers(int questionId)
        {
            var question = _context.PollQuestions.Find(questionId);

            question.Answers.ToList().ForEach(pa => {
                pa.Count = _context.PollUserAnswers.Count(ua => ua.QuestionId == pa.QuestionId && ua.AnswerId == pa.AnswerId);
            });
            return question;
        }

        public void Load<TElement>(PollQuestion PollQuestion, Expression<Func<PollQuestion, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.PollQuestions.Attach(PollQuestion);
            _context.Entry(PollQuestion).Collection(includeProperty).Load();
        }

        public IEnumerable<PollQuestion> GetMany(Expression<Func<PollQuestion, bool>> where)
        {
            return _context.PollQuestions.Where(where).ToList();
        }
        public void InsertOrUpdateAnswer(PollAnswer PollAnswer)
        {
            if (PollAnswer.AnswerId == default(int))
            {
                // New entity
                _context.PollAnswers.Add(PollAnswer);
            }
            else {
                // Existing entity
                _context.Entry(PollAnswer).State = EntityState.Modified;
            }
        }
        public void InsertOrUpdate(PollQuestion PollQuestion)
        {
            if (PollQuestion.Id == default(int))
            {
                // New entity
                _context.PollQuestions.Add(PollQuestion);
            }
            else {
                // Existing entity
                //_context.Entry(PollQuestion).State = EntityState.Modified;

                var existingParent = _context.PollQuestions.Where(p => p.Id == PollQuestion.Id).Include(p => p.Answers).SingleOrDefault();
                if (existingParent != null)
                {
                    // Update parent 
                    _context.Entry(existingParent).CurrentValues.SetValues(PollQuestion);
                    // Delete children 
                    foreach (var existingChild in existingParent.Answers.ToList())
                    {
                        if (!PollQuestion.Answers.Any(c => c.AnswerId == existingChild.AnswerId)) _context.PollAnswers.Remove(existingChild);
                    }
                    // Update and Insert children
                    foreach (var childModel in PollQuestion.Answers)
                    {
                        var existingChild = existingParent.Answers.Where(c => c.AnswerId == childModel.AnswerId).SingleOrDefault();
                        if (existingChild != null)
                            // Update child 
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                        else {
                            // Insert child 

                            existingParent.Answers.Add(childModel);
                        }
                    }
                }
            }

        }
        public void InsertUserAnswer(PollUserAnswer userAnswer)
        {
            _context.PollUserAnswers.Add(userAnswer);
        }

        public void Delete(int id)
        {
            var answers = _context.PollAnswers.Where(a => a.QuestionId == id).ToList();
            _context.PollAnswers.RemoveRange(answers);

            var userAnswers = _context.PollUserAnswers.Where(ua => ua.QuestionId == id).ToList();
            _context.PollUserAnswers.RemoveRange(userAnswers);

            var PollQuestion = _context.PollQuestions.Find(id);
            _context.PollQuestions.Remove(PollQuestion);

        }
        public void DeleleAnswer(int questionId, int answerId)
        {
            var answer = _context.PollAnswers.Find(questionId, answerId);
            var userAnswers = _context.PollUserAnswers.Where(ua => ua.QuestionId == questionId && ua.AnswerId == answerId);
            if(answer != null)
            {
                _context.PollAnswers.Remove(answer);
                _context.PollUserAnswers.RemoveRange(userAnswers);
            }
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

    public interface IPollRepository : IDisposable
    {
        IQueryable<PollQuestion> All { get; }
        IQueryable<PollQuestion> AllIncluding(params Expression<Func<PollQuestion, object>>[] includeProperties);
        PollQuestion Find(int id);
        PollUserAnswer FindUserAnswer(int questionId, int order, string ip);
        bool CheckUserAnswered(int questionId, string ip);

        IEnumerable<PollQuestion> VideoQuestions(int videoId);
        IEnumerable<PollAnswer> GetAnswers(int questionId);
        IEnumerable<PollUserAnswer> GetUserAnswers(int questionId);
        PollQuestion QuestionAnswers(int questionId);
        IEnumerable<PollQuestion> GetMany(Expression<Func<PollQuestion, bool>> where);
        void Load<TElement>(PollQuestion PollQuestion, Expression<Func<PollQuestion, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(PollQuestion PollQuestion);
        void InsertOrUpdateAnswer(PollAnswer PollAnswer);
        void InsertUserAnswer(PollUserAnswer userAnswer);
        void Delete(int id);
        void Save();
        void DeleleAnswer(int questionId, int answerId);
    }
}