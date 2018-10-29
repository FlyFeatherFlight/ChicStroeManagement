using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Configuration;
using System.Web.Hosting;

namespace ChicStoreManagement.WEB.Utils
{
    public static class UploadManager
    {
        #region 属性

        private const string FilesSubdir = "attachments";
        private const string TempExtension = "";

        /// <summary>
        /// 上传到服务器的物理路径
        /// </summary>
        public static string UploadFolderPhysicalPath { get; set; }

        /// <summary>
        /// 上传到服务器的相对路径
        /// </summary>
        public static string UploadFolderRelativePath { get; set; }


        #endregion

        #region Ctor

        static UploadManager()
        {
            //从配置文件中获取上传文件夹
            if (String.IsNullOrWhiteSpace(WebConfigurationManager.AppSettings["UploadFolder"]))
            {
                UploadFolderRelativePath = @"~/upload";
                UploadFolderPhysicalPath = HostingEnvironment.MapPath(UploadFolderRelativePath);
            }

            else
            {
                UploadFolderRelativePath = WebConfigurationManager.AppSettings["UploadFolder"];
                UploadFolderPhysicalPath = UploadFolderRelativePath;
            }
             
           
           
            if (!Directory.Exists(UploadFolderPhysicalPath))
                Directory.CreateDirectory(UploadFolderPhysicalPath);//如果第一级路径不存在则创建目录
        }

        #endregion

        #region 保存文件
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名字</param>
        /// <param name="storeName">店铺名字</param>
        /// <param name="guid">提交id</param>
        /// <param name="fileType">文件类型</param>
        /// <returns>存储的路径</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static String SaveFile(Stream stream, string fileName, string storeName, string guid,string fileType)
        {
            string tempPath = string.Empty, targetPath = string.Empty;

            try
            {
                

                if (storeName != null)
                {
                    var storename = storeName;
                    var contentId = guid;

                    tempPath = GetTempFilePath(fileName);//获取临时文件路径
                    targetPath = GetTargetFilePath(fileName, storename, contentId, fileType);//获取目标文件路径


                    //若上传文件夹中子文件夹未存在则创建
                    var file = new FileInfo(targetPath);
                    var tempFile = new FileInfo(tempPath);
                    if (file.Directory != null && !file.Directory.Exists)
                    {
                        file.Directory.Create();
                        
                    }
                    if (tempFile.Directory != null && !tempFile.Directory.Exists)
                    {
                        tempFile.Directory.Create();
                    }

                    FileStream fs = File.Open(tempPath, FileMode.Append);
                    
                        if (stream.Length > 0)
                        {
                            SaveFile(stream, fs);
                        }
                        fs.Close();
                   
                    if (File.Exists(targetPath))
                    { //如果文件存在，直接替换文件，并建立备份
                        File.Replace(tempPath, targetPath, targetPath + ".bak");
                    }
                    else
                    { 
                        //上传完毕将临时文件移动到目标文件
                        File.Move(tempPath, targetPath);
                    }
                   
                }
            }
            catch (Exception e)
            {
                // 若上传出错，则删除上传到文件夹文件
                if (File.Exists(targetPath))
                    File.Delete(targetPath);

                //// 删除临时文件
                //if (File.Exists(tempPath))
                //    File.Delete(tempPath);

                return null;
            }
            finally
            {
                // 删除临时文件
                //if (File.Exists(tempPath))
                //    File.Delete(tempPath);
            }
            return targetPath;
        }

        /// <summary>
        /// 循环读取流到文件流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fs"></param>
        public static void SaveFile(Stream stream, FileStream fs)
        {
            var buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }

        #endregion

        #region 获取临时文件夹路径

        public static string GetTempFilePath(string fileName)
        {
            fileName = fileName + TempExtension;
            //Path.Combine(@HostingEnvironment.ApplicationPhysicalPath, Path.Combine(UploadFolderPhysicalPath, fileName));
            return Path.Combine(UploadFolderPhysicalPath,"temp", fileName);
        }

        /// <summary>
        /// 目标文件夹路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="storeName"></param>
        /// <param name="contentId"></param>
        /// <param name="culture"></param>
        /// <param name="optionalSubdir"></param>
        /// <returns></returns>
        public static string GetTargetFilePath(string fileName, string storeName, string contentId ,string fileType)
        {
            return Path.Combine(UploadFolderPhysicalPath, storeName, contentId,fileType,
                                 fileName);
        }

        #region 依据路径删除文件

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        #endregion

        #endregion
    }
}