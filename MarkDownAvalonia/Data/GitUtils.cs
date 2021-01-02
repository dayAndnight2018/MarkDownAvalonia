using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MarkDownAvalonia.Data
{
    /// <summary>
    /// git hexo is source /blog
    /// git master is present /editor
    /// </summary>
    public static class GitUtils
    {
        /// <summary>
        /// git push source
        /// </summary>
        /// <returns></returns>
        public static bool GitPush()
        {
            string path = CommonData.config.RootDirectory;
            string address = CommonData.config.GitAddress;
            if (!String.IsNullOrWhiteSpace(path) && !String.IsNullOrWhiteSpace(address))
            {
                StringBuilder sb = new StringBuilder();
                var git = new CommandRunner("git", Path.Combine(path, "blog"));
                // checkout
                var result = git.Run("checkout hexo");
                // add all
                result = git.Run("add .");
                sb.Append(result);
                // commit
                git.Run(@"commit -m 'Commit by MarkdownAvalonia'");
                sb.Append(result);
                // push
                git.Run(@"push");
                sb.Append(result);
                return true;
            }

            return false;
        }

        /// <summary>
        /// pull source
        /// </summary>
        /// <returns></returns>
        public static bool GitPull()
        {
            string path = CommonData.config.RootDirectory;
            string address = CommonData.config.GitAddress;
            if (!String.IsNullOrWhiteSpace(path) && !String.IsNullOrWhiteSpace(address))
            {
                StringBuilder sb = new StringBuilder();
                var git = new CommandRunner("git", Path.Combine(path, "blog"));
                // checkout
                var result = git.Run("checkout hexo");
                sb.Append(result);
                // pull
                result = git.Run("pull");
                sb.Append(result);
                return true;
            }

            return false;
        }

        /// <summary>
        /// git clone source
        /// </summary>
        /// <returns></returns>
        public static bool GitRestore()
        {
            string path = CommonData.config.RootDirectory;
            string address = CommonData.config.GitAddress;
            if (!String.IsNullOrWhiteSpace(path) && !String.IsNullOrWhiteSpace(address))
            {
                StringBuilder sb = new StringBuilder();
                var git = new CommandRunner("git", path);
                // pull
                var result = git.Run("clone -b hexo " + address);
                sb.Append(result);
                return true;
            }

            return false;
        }

        /// <summary>
        /// deploy hexo
        /// </summary>
        /// <returns></returns>
        public static bool HexoDeploy()
        {
            string path = CommonData.config.RootDirectory;
            if (!String.IsNullOrWhiteSpace(path))
            {
                StringBuilder sb = new StringBuilder();
                var hexo = new CommandRunner("hexo", Path.Combine(path, "editor"));
                var result = hexo.Run("clean");
                sb.Append(result);
                // generate
                result = hexo.Run("generate");
                sb.Append(result);
                // deploy
                result = hexo.Run("deploy");
                sb.Append(result);
                return true;
            }

            return false;
        }

        /// <summary>
        /// publish hexo
        /// </summary>
        /// <returns></returns>
        public static bool HexoPublish()
        {
            string path = CommonData.config.RootDirectory;
            if (!String.IsNullOrWhiteSpace(path))
            {
                StringBuilder sb = new StringBuilder();
                var hexo = new CommandRunner("hexo", Path.Combine(path, "editor"));
                // generate
                var result = hexo.Run("generate");
                sb.Append(result);
                // server
                result = hexo.Run("server");
                sb.Append(result);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// init hexo
        /// </summary>
        /// <returns></returns>
        public static bool HexoInit()
        {
            string path = CommonData.config.RootDirectory;
            if (!String.IsNullOrWhiteSpace(path))
            {
                // empty directory
                string realPath = Path.Combine(path, "editor");
                MakeItEmpty(realPath);
                // init
                StringBuilder sb = new StringBuilder();
                var hexo = new CommandRunner("hexo", realPath);
                // generate
                var result = hexo.Run("init");
                sb.Append(result);
                return true;
            }

            return false;
        }

        /// <summary>
        /// make sure the directory exists
        /// </summary>
        /// <param name="path"></param>
        public static void MakeDirectory(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        
        public static void ClearDirectory(String path)
        {
            String[] files = Directory.GetFiles(path);
            if (Directory.Exists(path) && files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (Directory.Exists(file))
                    {
                        Directory.Delete(file, true);
                    }
                    else if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        public static void MakeItEmpty(String path)
        {
            MakeDirectory(path);
            ClearDirectory(path);
        }
    }
}