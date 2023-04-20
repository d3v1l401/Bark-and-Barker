using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Network
{
    public enum InventoryType : uint
    {
        INVENTORY_NONE = 0,
        INVENTORY_CHARACTER = 3,
        INVENTORY_STASH = 4,
    }
    public enum LoginResponseResult : uint
    {
        RESULT_NONE = 0,
        SUCCESS = 1,
        SUCCESS_FIRST = 2,
        FAIL_PASSWORD = 3,
        FAIL_CONNECT = 4,
        FAIL_SHORT_ID_OR_PASSWORD = 5,
        FAIL_OVERFLOW_ID_OR_PASSWORD = 6,
        FAIL_IP_PORT = 7,
        FAIL_OVERLAP_LOGIN = 8,
        FAIL_STEAM_BUILD_ID = 11,
        FAIL_LOGIN_BAN_USER = 12,
        FAIL_LOGIN_BAN_USER_CHEATER = 13,
        FAIL_LOGIN_BAN_USER_INAPPROPRIATE_NAME = 14,
        FAIL_LOGIN_BAN_USER_ETC = 15,
        FAIL_LOGIN_BAN_HARDWARE = 16,
        SUCCESS_AGREE_CHECK_RES = 51,
    }

    public enum MatchmakingRequestType
    {
        NONE = 0,
        REGISTER = 1,
        CANCEL = 2,
    }

    public enum MatchmakingResponseResult
    {
        NONE = 0,
        SUCCESS = 1,
        FAIL = 2,
        FAIL_ALREADY_TRYING = 3,
        FAIL_NO_READY_PARTY_MEMBER = 4,
        FAIL_REGION_SELECT = 5,
        FAIL_SERVER_DISABLE = 6,
        FAIL_SHORTAGE_ENTRANCE_FEE = 7,
        FAIL_SOLO_ONLY = 8,
        FAIL_SHORTAGE_LEVEL = 9,
    }
}
