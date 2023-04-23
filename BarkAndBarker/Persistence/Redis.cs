using BarkAndBarker.Shared.Settings;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Persistence
{
    public class Redis
    {
        private static Lazy<ConnectionMultiplexer> m_multiPlex = null;
        public Redis(Settings.SData settings)
        {
            m_multiPlex = new Lazy<ConnectionMultiplexer>(() => 
            {
                return ConnectionMultiplexer.Connect(settings.RedisAddress);
            });
        }

        public ConnectionMultiplexer Session
        {
            get { return m_multiPlex.Value; }
        }
    }
}
