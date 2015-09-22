using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Excel;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;

namespace Mhotivo.Implement.Repositories
{
    public class ImportDataRepository : IImportDataRepository
    {
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollRepository _enrollRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IUserRepository _userRepository;

        public ImportDataRepository(IPasswordGenerationService passwordGenerationService, IParentRepository parentRepository, IStudentRepository studentRepository, IEnrollRepository enrollRepository, IAcademicYearRepository academicYearRepository, IUserRepository userRepository)
        {
            _passwordGenerationService = passwordGenerationService;
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _enrollRepository = enrollRepository;
            _academicYearRepository = academicYearRepository;
            _userRepository = userRepository;
        }

        public void Import(DataSet oDataSet, AcademicYear academicYear)
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
                    ,MyGender = Utilities.DefineGender(dtDatos.Rows[indice][8].ToString())
                    ,BirthDate = DateTime.FromOADate(Double.Parse(dtDatos.Rows[indice][9].ToString())).ToShortDateString()
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
                    ,MyGender = Utilities.DefineGender(dtDatos.Rows[indice][22].ToString())
                    ,BirthDate = DateTime.FromOADate(Double.Parse(dtDatos.Rows[indice][24].ToString())).ToShortDateString()
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
            SaveData(listStudents, listParents, academicYear);
        }

        //modificado
        public DataSet GetDataSetFromExcelFile(HttpPostedFileBase getFile)
        {
            if (getFile != null && getFile.ContentLength > 0)
            {
                IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(getFile.InputStream);
                if (Path.GetExtension(getFile.FileName) == ".xlsx")
                    reader = ExcelReaderFactory.CreateOpenXmlReader(getFile.InputStream);
                DataSet dataSet = reader.AsDataSet();
                return dataSet;
            }
                return new DataSet();
        }


        private void SaveData(IEnumerable<Student> listStudents, IEnumerable<Parent> listParents, AcademicYear academicYear)
        {
            var allParents = _parentRepository.GetAllParents();
            var allStudents = _studentRepository.GetAllStudents();
            var allEnrolls = _enrollRepository.GetAllsEnrolls();
            if (!(((EnrollRepository)_enrollRepository).GeContext().Equals(((ParentRepository)_parentRepository).GeContext())))
                return;
            if (!(((EnrollRepository)_enrollRepository).GeContext().Equals(((StudentRepository)_studentRepository).GeContext())))
                return;
            if (!(((EnrollRepository)_enrollRepository).GeContext().Equals(((AcademicYearRepository)_academicYearRepository).GeContext())))
                return;
            foreach (var pare in listParents)
            {
                var temp = allParents.Where(x => x.IdNumber == pare.IdNumber);
                if (!temp.Any())
                {
                    var newUser = new User
                    {
                        DisplayName = pare.FirstName,
                        Email = "",
                        Password = _passwordGenerationService.GenerateTemporaryPassword(),
                        IsActive = true,
                        Role = Roles.Padre
                    };
                    //TODO: add to newUsers table.
                    newUser = _userRepository.Create(newUser);
                    
                    pare.MyUser = newUser;
                    _parentRepository.Create(pare);
                }
                else
                {
                    pare.Id = temp.First().Id;
                }
                    
            }
            foreach (var stu in listStudents)
            {
                var temp = allStudents.Where(x => x.IdNumber == stu.IdNumber);
                if (!temp.Any())
                {
                    stu.MyUser = stu.Tutor1.MyUser;
                    _studentRepository.Create(stu);   
                }
                else
                    stu.Id = temp.First().Id;
                var enr = allEnrolls.Where(x => x.AcademicYear.Id == academicYear.Id && x.Student.Id == stu.Id);
                if (enr.Any()) continue;
                var te = new Enroll();
                var academicYearTemp = _academicYearRepository.GetById(academicYear.Id);
                var studentTemp = _studentRepository.GetById(stu.Id);
                te.AcademicYear = academicYearTemp;
                te.Student = studentTemp;
                _enrollRepository.Create(te);
            }
        }
    }
}
