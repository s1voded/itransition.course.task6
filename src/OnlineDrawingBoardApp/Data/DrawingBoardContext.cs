using Microsoft.EntityFrameworkCore;
using OnlineDrawingBoardApp.Models;

namespace OnlineDrawingBoardApp.Data
{
    public class DrawingBoardContext : DbContext
    {
        public DrawingBoardContext(DbContextOptions<DrawingBoardContext> options) : base(options)
        {
        }

        public DbSet<DrawingBoard> DrawingBoards { get; set; }
    }
}
