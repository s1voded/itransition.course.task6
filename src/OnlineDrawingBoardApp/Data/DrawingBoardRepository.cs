using Microsoft.EntityFrameworkCore;
using OnlineDrawingBoardApp.Models;
using System;

namespace OnlineDrawingBoardApp.Data
{
    public class DrawingBoardRepository : IDrawingBoardRepository
    {
        private readonly DrawingBoardContext _drawingBoardContext;

        public DrawingBoardRepository(DrawingBoardContext drawingBoardContext)
        {
            _drawingBoardContext = drawingBoardContext;
        }

        public async Task<DrawingBoard> GetDrawingBoard(int drawingBoardId)
        {
            return await _drawingBoardContext.DrawingBoards.FirstOrDefaultAsync(d => d.Id == drawingBoardId);
        }

        public async Task<List<DrawingBoard>> GetAllDrawingBoards()
        {
            return await _drawingBoardContext.DrawingBoards.ToListAsync();
        }

        public async Task<DrawingBoard> AddNewDrawingBoard(DrawingBoard newDrawingBoard)
        {
            var _drawingBoardEntity =  await _drawingBoardContext.DrawingBoards.AddAsync(newDrawingBoard);
            await _drawingBoardContext.SaveChangesAsync();
            return _drawingBoardEntity.Entity;
        }

        public async Task<DrawingBoard> UpdateDrawingBoardContent(int drawingBoardId, string content)
        {
            var drawingBoard = await _drawingBoardContext.DrawingBoards.FirstOrDefaultAsync(d => d.Id == drawingBoardId);
            drawingBoard.Content = content;
            await _drawingBoardContext.SaveChangesAsync();
            return drawingBoard;
        }
    }

    public interface IDrawingBoardRepository
    {
        Task<DrawingBoard> GetDrawingBoard(int drawingBoardId);
        Task<List<DrawingBoard>> GetAllDrawingBoards();

        Task<DrawingBoard> AddNewDrawingBoard(DrawingBoard newDrawingBoard);

        Task<DrawingBoard> UpdateDrawingBoardContent(int drawingBoardId, string content);
    }
}
