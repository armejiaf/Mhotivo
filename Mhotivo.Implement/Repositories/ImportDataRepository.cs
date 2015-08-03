using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class ImportDataRepository : IImportDataRepository
    {
        private readonly MhotivoContext _context;

        public ImportDataRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public void Import(DataSet oDataSet, AcademicYear academicYear, IParentRepository parentRepository, IStudentRepository studentRepository, IEnrollRepository enrollRepository, IAcademicYearRepository academicYearRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            if(oDataSet.Tables.Count == 0)
                return;
            if(oDataSet.Tables[0].Rows.Count <= 15)
                return;
            var dtDatos = oDataSet.Tables[0];
            var listStudents = new List<Student>();
            var listParents = new List<Parent>();
            for (var indice = 15; indice < dtDatos.Rows.Count; indice++)
            {
                if(dtDatos.Rows[indice][2].ToString().Trim().Length == 0)
                    continue;
                var newStudents = new Student
                {
                    IdNumber = dtDatos.Rows[indice][2].ToString()
                    ,LastName = (dtDatos.Rows[indice][3] + " " + dtDatos.Rows[indice][4]).Trim()
                    ,FirstName = dtDatos.Rows[indice][6].ToString()
                    ,Gender = Utilities.IsMasculino(dtDatos.Rows[indice][8].ToString())
                    ,BirthDate = Utilities.ConvertStringToDateTime(dtDatos.Rows[indice][9].ToString()).ToShortDateString()
                    ,Nationality = dtDatos.Rows[indice][13].ToString()
                    ,State = dtDatos.Rows[indice][15].ToString()
                };
                newStudents.FullName = (newStudents.FirstName + " " + newStudents.LastName).Trim();
                var newParent = new Parent
                {
                    Nationality = dtDatos.Rows[indice][16].ToString()
                    ,IdNumber = dtDatos.Rows[indice][18].ToString()
                    ,LastName = (dtDatos.Rows[indice][19] + " " + dtDatos.Rows[indice][20]).Trim()
                    ,FirstName = dtDatos.Rows[indice][21].ToString()
                    ,Gender = Utilities.IsMasculino(dtDatos.Rows[indice][22].ToString())
                    ,BirthDate = Utilities.ConvertStringToDateTime(dtDatos.Rows[indice][24].ToString()).ToShortDateString()
                    ,State = dtDatos.Rows[indice][25].ToString()
                    ,City = dtDatos.Rows[indice][26].ToString()
                };
                newParent.FullName = (newParent.FirstName + " " + newParent.LastName).Trim();
                var newContactInformation = new ContactInformation
                {
                    Type = "Telefono"
                    ,Value = dtDatos.Rows[indice][27].ToString()
                    ,People = newParent
                };
                var listContacts = new List<ContactInformation> {newContactInformation};
                newParent.Contacts = listContacts;
                newStudents.Tutor1 = newParent;
                listStudents.Add(newStudents);
                listParents.Add(newParent);
            }
            SaveData(listStudents, listParents, academicYear, parentRepository, studentRepository, enrollRepository, academicYearRepository, userRepository, roleRepository);
        }

        public DataSet GetDataSetFromExcelFile(string path)
        {
            var excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=1\";";
            var excelConnection = new OleDbConnection(excelConnectionString);
            //Never used?
            var database = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='"+ path +"';Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=1\";");
            var cmd = new OleDbCommand("Select * from [Matricula$]", excelConnection);
            var oDataAdapter = new OleDbDataAdapter(cmd);
            var oSet = new DataSet();
            oDataAdapter.Fill(oSet);
            return oSet;
        }


        private void SaveData(IEnumerable<Student> listStudents, IEnumerable<Parent> listParents, AcademicYear academicYear, IParentRepository parentRepository, IStudentRepository studentRepository, IEnrollRepository enrollRepository, IAcademicYearRepository academicYearRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            var allParents = parentRepository.GetAllParents();
            var allStudents = studentRepository.GetAllStudents();
            var allEnrolls = enrollRepository.GetAllsEnrolls();
            if (!(((EnrollRepository)enrollRepository).GeContext().Equals(((ParentRepository)parentRepository).GeContext())))
                return;
            if (!(((EnrollRepository)enrollRepository).GeContext().Equals(((StudentRepository)studentRepository).GeContext())))
                return;
            if (!(((EnrollRepository)enrollRepository).GeContext().Equals(((AcademicYearRepository)academicYearRepository).GeContext())))
                return;
            foreach (var pare in listParents)
            {
                var temp = allParents.Where(x => x.IdNumber == pare.IdNumber);
                if (!temp.Any())
                {
                    var newUser = new User
                    {
                        DisplayName = pare.FirstName,
                        //TODO: Get rid of this bit.
                        Email =
                            (pare.FirstName.Trim().Replace(" ", "") + "_" + pare.IdNumber.Trim().Substring(10) +
                             "@mhotivo.hn").ToLower(),
                        Password = "123456",
                        Status = true
                    };
                    newUser = userRepository.Create(newUser, roleRepository.GetById(2));
                    pare.User = newUser;
                    parentRepository.Create(pare);
                }
                else
                    pare.Id = temp.First().Id;
            }
            foreach (var stu in listStudents)
            {
                var temp = allStudents.Where(x => x.IdNumber == stu.IdNumber);
                if (!temp.Any())
                {
                    stu.User = stu.Tutor1.User;
                    studentRepository.Create(stu);   
                }
                else
                    stu.Id = temp.First().Id;
                var enr = allEnrolls.Where(x => x.AcademicYear.Id == academicYear.Id && x.Student.Id == stu.Id);
                if (enr.Any()) continue;
                var te = new Enroll();
                var academicYearTemp = academicYearRepository.GetById(academicYear.Id);
                var studentTemp = studentRepository.GetById(stu.Id);
                te.AcademicYear = academicYearTemp;
                te.Student = studentTemp;
                enrollRepository.Create(te);
            }
        }
    }
}
