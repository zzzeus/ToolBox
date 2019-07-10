using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolBox.Helpers
{
    class StringHelper
    {
        static Regex reg = new Regex(@"[\u4e00-\u9fa5]");//正则表达式
        public static Boolean isContainKanji(string text)
        {
            
            
            if (reg.IsMatch(text))
                return true;
            else
                return false;
        }
    }
}
