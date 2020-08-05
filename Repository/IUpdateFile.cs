using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Repository
{
   public interface IUpdateFile
    {
        
        public static void SaveToFile(string path, string text, bool appendText) { }
    }
}
