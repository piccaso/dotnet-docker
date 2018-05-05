using System;
using System.Collections.Generic;
using System.Linq;
using aspnetapp.Models;
using LiteDB;

namespace aspnetapp.Database
{
    public class Repository
    {
        private const string DbFile = "local.db";
        private readonly LiteRepository _db;

        public Repository()
        {
            _db = new LiteRepository(DbFile);
            var mapper = _db.Database.Mapper;

            mapper.Entity<QuestionNode>()
                .Id(f=>f.Id)
                .DbRef(c => c.Answers);
            mapper.Entity<AnswerNode>()
                .Id(f=>f.Id);
            
            _db.Database.GetCollection<QuestionNode>().EnsureIndex(q => q.LastUsed);
        }

        public IEnumerable<BsonValue> InsertQuestion(QuestionNode questionNode)
        {
            return InsertQuestions(new[] {questionNode}).ToList();
        }

        public IEnumerable<BsonValue> InsertQuestions(IList<QuestionNode> questionNodes)
        {
            foreach (var questionNode in questionNodes)
            {
                foreach (var answer in questionNode.Answers)
                {
                    yield return _db.Insert(answer);
                }

                yield return _db.Insert(questionNode);
            }
            
        }

        public IList<QuestionNode> GetAllQuestions()
        {
            var questions = _db.Query<QuestionNode>().Include(q => q.Answers).ToList();
            return questions;
        }

        public IList<QuestionNode> GetNextQuestions(int ammount)
        {
            var questions = GetAllQuestions()
                .OrderByDescending(q => q.LastUsed)
                .Take(ammount)
                .ToList();

            var now = Environment.TickCount;

            foreach (var question in questions)
            {
                question.LastUsed = now;
                _db.Update(question);
            }

            return questions;
        }

        public void Truncate()
        {
            _db.Delete<QuestionNode>(Query.All());
            _db.Delete<AnswerNode>(Query.All());
        }
    }
}