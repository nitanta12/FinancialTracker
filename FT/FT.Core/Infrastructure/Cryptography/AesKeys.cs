using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Core.Infrastructure.Cryptography
{
    public static class AesKeys
    {
        public static string RefreshTokenAesKey => "ENCRYPT-USER-REFRESH-TOKEN======";
        public static string UserIdAesKey => "ENCRYPT-USER-ID-TOKEN===========";
    }
}
