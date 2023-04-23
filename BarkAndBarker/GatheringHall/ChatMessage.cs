using DC.Packet;

namespace BarkAndBarker.GatheringHall
{
    internal class ChatMessage
    {
        public string Text { get; set; }
        public ChatType ChatType { get; set; }
        public ClientSession Client { get; set; }
        public DateTime Time { get; set; }

        public ChatMessage(string text, ChatType chatType, ClientSession client)
        {
            Text = text;
            ChatType = chatType;
            Client = client;

            Time = DateTime.Now;
        }

        public static SS2C_GATHERING_HALL_CHANNEL_CHAT_NOT CreateNotPacket(ChatMessage chatMessage)
        {
            var messagePacket = new SS2C_GATHERING_HALL_CHANNEL_CHAT_NOT();

            var chatPacket = new SGATHERING_HALL_CHAT_S2C();
            chatPacket.ChatIndex = 1;
            chatPacket.ChatType = (uint)chatMessage.ChatType;
            chatPacket.Time = 1;

            var schatdata = new SCHATDATA();
            schatdata.Nickname = new SACCOUNT_NICKNAME()
            {
                KarmaRating = chatMessage.Client.m_currentCharacter.KarmaScore,
                OriginalNickName = chatMessage.Client.m_currentCharacter.Nickname,
                StreamingModeNickName = chatMessage.Client.m_currentCharacter.Nickname,
            };
            schatdata.AccountId = chatMessage.Client.m_currentPlayer.AccountID.ToString();
            schatdata.CharacterId = chatMessage.Client.m_currentCharacter.CharID;

            var schatdataPiece = new SCHATDATA_PIECE();
            schatdataPiece.ChatStr = chatMessage.Text;

            schatdata.ChatDataPieceArray.Add(schatdataPiece);

            chatPacket.ChatData = schatdata;

            messagePacket.Chats.Add(chatPacket);

            return messagePacket;
        }
    }

    public enum ChatType
    {
        Normal = 0
    }
}
