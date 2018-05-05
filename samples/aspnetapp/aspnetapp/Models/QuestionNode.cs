using System.Collections.Generic;
using LiteDB;

namespace aspnetapp.Models
{
    public class QuestionNode
    {
        public int Id { get; set; }
        public long LastUsed { get; set; }
        public byte[] Image { get; set; }
        public string Header { get; set; }
        public string Question { get; set; }
        public List<AnswerNode> Answers { get; set; }
    }
}