using BarkAndBarker.Ranking;
using BarkAndBarker.Shared.Persistence.Models.CharacterStatistics;
using DC.Packet;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class RankingProcessors
    {
        public static ClassType StringToClassType(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return ClassType.All;

            var types = Enum.GetValues<ClassType>();

            foreach (var type in types)
            {
                if (s.Contains(type.ToString())) return type;
            }

            return ClassType.All;
        }

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

            var topList = RankingCache.CachedTopRankings;

            var classType = StringToClassType(response.CharacterClass);
            var rankType = (RankType)response.RankType;
            foreach (var modelCharacterRankingTop in topList.GetAll)
            {
                if (modelCharacterRankingTop.ClassType == classType && modelCharacterRankingTop.RankType == rankType)
                {
                    var record = new SRankRecord
                    {
                        PageIndex = 0,
                        Rank = (uint)modelCharacterRankingTop.Rank,
                        Score = (uint)modelCharacterRankingTop.Score,
                        Percentage = 100, //TODO
                        AccountId = modelCharacterRankingTop.AccountID.ToString(),
                        NickName = new SACCOUNT_NICKNAME()
                        {
                            OriginalNickName = modelCharacterRankingTop.Nickname,
                            StreamingModeNickName = "RedactedForPrivacy", //TODO
                            KarmaRating = 100, //TODO
                        },
                        CharacterClass = response.CharacterClass
                    };

                    response.Records.Add(record);
                }
            }

            response.AllRowCount = (uint)response.Records.Count;

            var serial = new WrapperSerializer<SS2C_RANKING_RANGE_RES>(response, session.m_currentPacketSequence++, PacketCommand.S2CRankingRangeRes);
            return serial.Serialize();
        }
    }
}
