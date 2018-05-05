using LiteDB;

namespace aspnetapp.Models
{
    public class AnswerNode
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public decimal Score { get; set; }
        public string Conclusion { get; set; }
    }
}
