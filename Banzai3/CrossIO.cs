using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Banzai3
{
    public static class CrossIO
    {
        private const string Extension = "banzai";
        private const string ExtensionSolve = "solve";
        private const string ExtensionEditor = "editor";
        private const string LibraryDirName = "Library";
        private const string UserDirName = "Users.0000";

        private static readonly string PathLibrary =
            $@"{Application.StartupPath}\{LibraryDirName}";

        private static readonly string PathSolve =
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Application.CompanyName}\{
                Application.ProductName}";

        private static string GetFileNameSolve(string dir, string file)
        {
            return $@"{PathSolve}\{dir}\{file}.{ExtensionSolve}";
        }

        private static string GetFileNameEditor(string file, bool isMap)
        {
            var ext = isMap ? ExtensionEditor : Extension;
            var path = GetPathEditor();
            return $@"{path}\{file}.{ext}";
        }

        private static string GetPathSolve(string dir)
        {
            return $@"{PathSolve}\{dir}";
        }

        private static string GetPathEditor()
        {
            return GetPathSolve(UserDirName);
        }

        private static string GetFileNameLibrary(string dir, string file)
        {
            return dir != UserDirName
                ? $@"{PathLibrary}\{dir}\{file}.{Extension}"
                : $@"{GetPathEditor()}\{file}.{Extension}";
        }

        private static string GetPathLibrary(string dir)
        {
            return $@"{PathLibrary}\{dir}";
        }

        public static void Export(string dir, string file, Cross cross)
        {
            var store = GetPathSolve(dir);
            if (!Directory.Exists(store))
                Directory.CreateDirectory(store);

            var name = GetFileNameSolve(dir, file);
            using (var t = File.CreateText(name))
            {
                cross.Export(t, true);
            }
        }

        public static void ExportEditor(string file, Cross cross)
        {
            var store = GetPathEditor();
            if (!Directory.Exists(store))
                Directory.CreateDirectory(store);

            using (var t = File.CreateText(GetFileNameEditor(file, true)))
            {
                cross.Export(t, true);
            }
            using (var t = File.CreateText(GetFileNameEditor(file, false)))
            {
                cross.Export(t, false);
            }
        }

        public static Cross Import(string dir, string file)
        {
            var name = GetFileNameSolve(dir, file);
            if (File.Exists(name))
            {
                try
                {
                    //if load from solve fail or slove not exists, then we try load from library
                    using (var stream = File.OpenText(name))
                    {
                        return new Cross(stream);
                    }
                }
                catch (Exception)
                {
                    //TODO log here
                    //if load from solve fail, we try load from library
                }
            }
            var name2 = GetFileNameLibrary(dir, file);
            using (var stream = File.OpenText(name2))
            {
                return new Cross(stream);
            }
        }

        public static IEnumerable<string> GetListDirs()
        {
            return Directory
                .EnumerateDirectories(PathLibrary)
                .Concat(new[] {UserDirName})
                .OrderBy(Path.GetExtension)
                .Select(Path.GetFileName);
        }

        public static IEnumerable<string> GetListFiles(string dir)
        {
            if (dir != UserDirName)
                return Directory
                    .EnumerateFiles($@"{GetPathLibrary(dir)}\", $@"*.{Extension}")
                    .Select(Path.GetFileNameWithoutExtension);
            var userPath = GetPathSolve(dir);
            if (!Directory.Exists(userPath))
                Directory.CreateDirectory(userPath);
            return Directory
                .EnumerateFiles($@"{userPath}\", $@"*.{Extension}")
                .Select(Path.GetFileNameWithoutExtension);
        }

        public static bool IsUserDir(string currentDir)
        {
            return currentDir == UserDirName;
        }
    }
}
