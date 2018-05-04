using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetapp.Models;
using Microsoft.EntityFrameworkCore;

namespace aspnetapp.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<QuestionNode> Questions { get; set; }
        public DbSet<AnswerNode> Answers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions) { }
    }
}
