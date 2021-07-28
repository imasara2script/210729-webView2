using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

namespace netHTA
{
    class JSON
    {
        public static Dictionary<string, string> デシリアライズ(string コード)
        {
            var json = JsonSerializer.Deserialize<Dictionary<string, string>>(コード);
            return json;
        }
        public static string シリアライズ<TValue>(TValue 引数)
        {
            var 文字列 = JsonSerializer.Serialize(引数);
            return 文字列;
        }
    }
}
