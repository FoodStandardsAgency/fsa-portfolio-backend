using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Security.Data
{
    public static class ClientDataProtection
    {
        // TODO: make configuration item?
        private static byte[] _entropy = { 2,7,4,1,8,6,3,6 };
        public static string Protect(string data)
        {
            byte[] dataAsBytes = Encoding.UTF8.GetBytes(data);
            var protectedBytes = ProtectedData.Protect(dataAsBytes, _entropy, DataProtectionScope.CurrentUser);
            return BitConverter.ToString(protectedBytes);
        }
        public static string Unprotect(string data)
        {
            byte[] dataAsBytes = data.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
            var unprotectedBytes = ProtectedData.Unprotect(dataAsBytes, _entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(unprotectedBytes);
        }

    }
}
