using Microsoft.AspNetCore.SignalR;
using OnlineDrawingBoardApp.Data;
using OnlineDrawingBoardApp.Models;

namespace OnlineDrawingBoardApp.Hubs
{
    public class DrawingHub : Hub<IDrawingClient>
    {
        private readonly IDrawingBoardRepository _drawingBoardRepository;

        public DrawingHub(IDrawingBoardRepository drawingBoardRepository)
        {
            _drawingBoardRepository = drawingBoardRepository;
        }

        public async Task SendContent(int drawingBoardId, string drawingBoardContent)
        {
            await _drawingBoardRepository.UpdateDrawingBoardContent(drawingBoardId, drawingBoardContent);
            await Clients.OthersInGroup(drawingBoardId.ToString()).ReceiveBoardContent(drawingBoardContent);
        }

        public async Task<List<DrawingBoard>> GetAllBoadrs()
        {
            var allDrawingBoards = await _drawingBoardRepository.GetAllDrawingBoards();
            return allDrawingBoards;
        }

        public async Task<DrawingBoard> AddNewDrawingBoard(string newBoardName, string content)
        {
            var newDrawingBoard = new DrawingBoard { Name = newBoardName, Content = content };
            var createdDrawingBoard = await _drawingBoardRepository.AddNewDrawingBoard(newDrawingBoard);

            var allDrawingBoards = await _drawingBoardRepository.GetAllDrawingBoards();
            await Clients.All.ReceiveAllBoards(allDrawingBoards);

            await JoinBoard(createdDrawingBoard.Id);
            return createdDrawingBoard;
        }

        public async Task JoinBoard(int drawingBoardId)
        {
            var drawingBoard = await _drawingBoardRepository.GetDrawingBoard(drawingBoardId);
            await Groups.AddToGroupAsync(Context.ConnectionId, drawingBoardId.ToString());
            await Clients.Caller.ReceiveBoardAfterJoin(drawingBoard);
        }

        public async Task LeaveBoard(int drawingBoardId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, drawingBoardId.ToString());
        }
    }

    public interface IDrawingClient
    {
        Task ReceiveBoardContent(string drawingBoardContent);
        Task ReceiveBoardAfterJoin(DrawingBoard drawingBoard);
        Task ReceiveAllBoards(List<DrawingBoard> drawingBoards);
    }
}
