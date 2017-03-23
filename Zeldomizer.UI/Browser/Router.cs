using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Zeldomizer.UI.Properties;

namespace Zeldomizer.UI.Browser
{
    public class Router
    {
        private readonly Dictionary<string, string> _assets;

        public Router()
        {
            _assets = new Dictionary<string, string>();

            void ReadAsset(ZipArchive archive, string name)
            {
                using (var stream = archive.GetEntry(name).Open())
                using (var reader = new StreamReader(stream))
                {
                    _assets[name] = reader.ReadToEnd();
                }
            }

            using (var stream = new MemoryStream(Resources.Bootstrap))
            using (var zip = new ZipArchive(stream))
            {
                ReadAsset(zip, "js");
                ReadAsset(zip, "jq");
                ReadAsset(zip, "css");
            }
        }

        private Stream BuildContent(string content)
        {
            var mem = new MemoryStream();
            var writer = new StreamWriter(mem);

            writer.Write($"<!DOCTYPE html><html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge;\"/>" +
                         $"<script>\n{_assets["jq"]}\n{_assets["js"]}\n</script>" +
                         $"<style>\n{_assets["css"]}\nhtml{{overflow-x:hidden;padding:8px;}}</style>" +
                         $"</head><body>{content}</body></html>");

            writer.Flush();
            mem.Position = 0;
            return mem;
        }

        public Stream Open(string path)
        {
            if (path.EndsWith("/"))
                path += "index";
            if (path.StartsWith("/"))
                path = path.Substring(1);

            var assembly = typeof(Router).Assembly;
            var doc = assembly.GetManifestResourceStream($"Zeldomizer.UI.Pages.{path.Replace('/', '.')}.html");

            if (doc == null)
            {
                return BuildContent($"This page doesn't exist yet.\nPath: {path}<br /><br /><a href='index'>Start Over.</a>");
            }
            
            using (doc)
            {
                var reader = new StreamReader(doc);
                return BuildContent(reader.ReadToEnd());
            }
        }
    }
}
