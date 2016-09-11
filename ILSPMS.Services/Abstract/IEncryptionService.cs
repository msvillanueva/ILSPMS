using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Services
{
    public interface IEncryptionService
    {
        string CreateSalt();
        string EncryptPassword(string password, string salt);
        string Decrypt(string Input);
        string Encrypt(string Input);
        string GenerateCode(int passwordLength);
    }
}
