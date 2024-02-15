using Microsoft.AspNetCore.SignalR;
using OnlineDrawingBoardApp.Data;

namespace OnlineDrawingBoardApp.Hubs
{
    public class DrawingHub : Hub<IDrawingClient>
    {
        public async Task SendContent(int drawingBoardId, string drawingBoardContent)
        {
            await Clients.OthersInGroup(drawingBoardId.ToString()).ReceiveBoardContent(drawingBoardContent);
        }

        public async Task JoinBoard(int drawingBoardId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, drawingBoardId.ToString());
            await Clients.Caller.ReceiveBoardId(drawingBoardId);
        }

        public async Task LeaveBoard(int drawingBoardId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, drawingBoardId.ToString());
            //await Clients.Caller.ReceiveBoardId(0);
        }
    }

    public interface IDrawingClient
    {
        Task ReceiveBoardContent(string drawingBoardContent);

        Task ReceiveBoardId(int drawingBoardId);
    }
}
