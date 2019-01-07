using ChicStoreManagement.Model;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;

namespace ChicStoreManagement.BLL
{

    /// <summary>
    /// 文件类型
    /// </summary>
  public partial  class FileTypeBLL:BaseService<设计_文件类型>,IFileTypeBLL
    {
        private IFileTypeDAL fileTypeDAL;

        public FileTypeBLL(IFileTypeDAL fileTypeDAL)
        {
            this.fileTypeDAL = fileTypeDAL;
            base.Dal = fileTypeDAL;
        }
    }
}
