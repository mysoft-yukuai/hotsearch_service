using System.IO;
using System.Reflection;

namespace Segmenter
{
    public class ConfigManager
    {
        public static string ConfigFileBaseDir => string.Concat(Assembly.GetExecutingAssembly().GetName().Name, '.', "Resources", '.');

        public static Stream MainDictFile => ReadFile(string.Concat(ConfigFileBaseDir, "dict.txt"));

        public static Stream ProbTransFile => ReadFile(string.Concat(ConfigFileBaseDir, "prob_trans.json"));

        public static Stream ProbEmitFile => ReadFile(string.Concat(ConfigFileBaseDir, "prob_emit.json"));

        public static Stream PosProbStartFile => ReadFile(string.Concat(ConfigFileBaseDir, "pos_prob_start.json"));

        public static Stream PosProbTransFile => ReadFile(string.Concat(ConfigFileBaseDir, "pos_prob_trans.json"));

        public static Stream PosProbEmitFile => ReadFile(string.Concat(ConfigFileBaseDir, "pos_prob_emit.json"));

        public static Stream CharStateTabFile => ReadFile(string.Concat(ConfigFileBaseDir, "char_state_tab.json"));

        public static Stream ReadFile(string path) => Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
    }
}
