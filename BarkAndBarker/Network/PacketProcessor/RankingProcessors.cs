using DC.Packet;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class RankingProcessors
    {
        public static object HandleRankingReq(ClientSession session, dynamic deserializer)
        {
            var request = ((WrapperDeserializer)deserializer).Parse<SC2S_RANKING_RANGE_REQ>();

            var response = new SS2C_RANKING_RANGE_RES();
            response.RankType = request.RankType;
            response.StartIndex = request.StartIndex;
            response.EndIndex = request.EndIndex;
            response.CharacterClass = request.CharacterClass;

            return response;
        }

        public static MemoryStream HandleRankingRes(ClientSession session, dynamic inputClass)
        {
            var response = (SS2C_RANKING_RANGE_RES)inputClass;

            response.Result = (uint)MatchmakingResponseResult.SUCCESS;

            response.AllRowCount = 1;

            //TODO
            var record1 = new SRankRecord
            {
                PageIndex = 0,
                Rank = 1,
                Score = 6969,
                Percentage = 30,
                AccountId = "421421412",
                NickName = new SACCOUNT_NICKNAME()
                {
                    OriginalNickName = "BestOne",
                    StreamingModeNickName = "BestOne",
                    KarmaRating = 100,
                },
                CharacterClass = response.CharacterClass
            };

            response.Records.Add(record1);

            var record2 = new SRankRecord
            {
                PageIndex = 0,
                Rank = 2,
                Score = 1337,
                Percentage = 20,
                AccountId = "421121412",
                NickName = new SACCOUNT_NICKNAME()
                {
                    OriginalNickName = "Second",
                    StreamingModeNickName = "Second",
                    KarmaRating = 20,
                },
                CharacterClass = response.CharacterClass
            };

            response.Records.Add(record2);


            var serial = new WrapperSerializer<SS2C_RANKING_RANGE_RES>(response, PacketCommand.S2CRankingRangeRes);
            return serial.Serialize();
        }
    }
}
