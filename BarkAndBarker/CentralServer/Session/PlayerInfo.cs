using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Session
{

    public class PlayerInfo
    {
#if USE_STEAM
        public string SteamID { get; set; }
#else
        public int AccountID { get; set; }
#endif
        public string CurrentHWID { get; set; }
    }
}
