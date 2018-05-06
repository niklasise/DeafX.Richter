using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Services
{
    public class VersionService
    {
        private const string FILE_NAME = "version.txt";

        private string _version;

        public string Version
        {
            get
            {
                if (_version == null)
                {
                    _version = GetVersion();
                }

                return _version;
            }
        }

        private string GetVersion()
        {
            string gitVersion = null;
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            if (File.Exists("version.txt"))
            {
                using (var stream = new FileStream(FILE_NAME, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        gitVersion = reader.ReadToEnd();
                    }
                }
            }

            gitVersion = gitVersion ?? "DEVELOPMENT";

            return $"{assemblyVersion}.{gitVersion}";
        }

    }

}
