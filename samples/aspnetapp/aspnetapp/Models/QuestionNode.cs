using System.Collections.Generic;

namespace aspnetapp.Models
{
    public class QuestionNode
    {
        public int Id { get; set; }
        public int LastUsed { get; set; }
        public byte[] Image { get; set; }
        public string Header { get; set; }
        public string Question { get; set; }
        public List<AnswerNode> Answers { get; set; }
    }
}