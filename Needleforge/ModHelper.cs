using Corpus_Callosum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Needleforge
{
    internal class ModHelper
    {
        public static void Log(string msg)
        {
            CorpusCallosumPlugin.Log(msg);
        }

        public static void LogError(string msg)
        {
            CorpusCallosumPlugin.Log(msg);
        }
    }
}
