using DC.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network.PacketProcessor
{
    public class Notifications
    {
        private static readonly List<FSERVICE_POLICY> m_policyList = new List<FSERVICE_POLICY>()
        {
            new FSERVICE_POLICY() { PolicyType = 9, PolicyValue = 10 },
            new FSERVICE_POLICY() { PolicyType = 7, PolicyValue = 100 },
            new FSERVICE_POLICY() { PolicyType = 12, PolicyValue = 15 },
            new FSERVICE_POLICY() { PolicyType = 4, PolicyValue = 50 },
            new FSERVICE_POLICY() { PolicyType = 8, PolicyValue = 10000 },
            new FSERVICE_POLICY() { PolicyType = 1, PolicyValue = 200 },
            new FSERVICE_POLICY() { PolicyType = 5, PolicyValue = 500 },
            new FSERVICE_POLICY() { PolicyType = 2, PolicyValue = 10000 },
            new FSERVICE_POLICY() { PolicyType = 3, PolicyValue = 1000 },
            new FSERVICE_POLICY() { PolicyType = 6, PolicyValue = 500 },
            new FSERVICE_POLICY() { PolicyType = 10, PolicyValue = 1000 },
            new FSERVICE_POLICY() { PolicyType = 11, PolicyValue = 5 },
        };

        public static object ServicePolicyNotification(ClientSession session)
        {
            var policies = new SS2C_SERVICE_POLICY_NOT();
            policies.PolicyList.AddRange(m_policyList);

            var ser = new WrapperSerializer<SS2C_SERVICE_POLICY_NOT>(policies, session.m_currentPacketSequence++, PacketCommand.S2CServicePolicyNot);
            return ser.Serialize();
        }

        public static object HandleLobbyAccountCurrencyListNot(ClientSession session)
        {
            var response = new SS2C_LOBBY_ACCOUNT_CURRENCY_LIST_NOT();

            response.CurrencyInfos.Add(new SACCOUNT_CURRENCY_INFO()
            {
                CurrencyType = 1,
                CurrencyValue = 9999
            });

            var serial = new WrapperSerializer<SS2C_LOBBY_ACCOUNT_CURRENCY_LIST_NOT>(response, session.m_currentPacketSequence++, PacketCommand.S2CLobbyAccountCurrencyListNot);
            return serial.Serialize();
        }
    }
}
