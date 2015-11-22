using System.Data;
using System.Web;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IDataImportService
    {
        DataSet GetDataSetFromExcelFile(HttpPostedFileBase getFile);
        void Import(DataSet myDataSet, AcademicGrade academicYearGrade);
    }
}
