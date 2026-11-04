using Google.Protobuf.Protocol;
using Server.Game.Object;

namespace Server.Game.Room
{
    public partial class GameRoom
    {
        long chatSequence;

        internal void DeliverChat(ClientSession sender, Player player, long context, string text)
        {
            if (sender.ConnectionClosed || sender.IsTransferring || sender.AccountDbId <= 0 ||
                sender.MyPlayer != player || player.Session != sender || player.Room != this ||
                !sender.ChatContextMatches(this, context) || !_players.TryGetValue(player.Id, out var member) ||
                member != player)
                return;
            var message = new S_Chat { Scope = RoomType == RoomType.Town ? ChatScope.ChatTown : ChatScope.ChatDungeon,
                                       RoomId = RoomId,
                                       Sequence = ++chatSequence,
                                       SenderId = player.Id,
                                       SenderName = player.Info.Name,
                                       Text = text };

            foreach (var recipient in _players.Values)
            {
                var session = recipient.Session;

                if (recipient.Room == this && session != null && !session.ConnectionClosed && !session.IsTransferring &&
                    session.MyPlayer == recipient)
                    session.Send(message);
            }
        }
    }
}
