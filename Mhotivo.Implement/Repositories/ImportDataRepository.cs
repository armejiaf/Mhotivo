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
        private readonly IRoleRepository _roleRepository;
        private readonly IPrivilegeRepository _privilegeRepository;

        public ImportDataRepository(IPasswordGenerationService passwordGenerationService, IParentRepository parentRepository, IStudentRepository studentRepository, IEnrollRepository enrollRepository, IAcademicYearRepository academicYearRepository, IUserRepository userRepository, IRoleRepository roleRepository, IPrivilegeRepository privilegeRepository)
        {
            _passwordGenerationService = passwordGenerationService;
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _enrollRepository = enrollRepository;
            _academicYearRepository = academicYearRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _privilegeRepository = privilegeRepository;
        }

        public void Import(DataSet oDataSet, AcademicYear academicYearGrade)
        {
            var emails = new List<string>();
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
                    ,BirthDate = DateTime.FromOADate(Double.Parse(dtDatos.Rows[indice][9].ToString()))
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
                    ,BirthDate = DateTime.FromOADate(Double.Parse(dtDatos.Rows[indice][24].ToString()))
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
                emails.Add(dtDatos.Rows[indice][28].ToString());
                var listContacts = new List<ContactInformation> {newContactInformation};
                newParent.ContactInformation = listContacts;
                //newParent.MyUser.Email
                newStudents.Tutor1 = newParent;
                listStudents.Add(newStudents);
                listParents.Add(newParent);
            }
            SaveData(listStudents, listParents, academicYearGrade, emails);
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


        private void SaveData(IEnumerable<Student> listStudents, IEnumerable<Parent> listParents, AcademicYear academicYearGrade, List<string> emails)
        {
            var allEnrolls = _enrollRepository.GetAllsEnrolls();
            var enrls = allEnrolls.Where(x => x.AcademicYearGrade.Id == academicYearGrade.Id);
            if (enrls.Any())
                throw new Exception("Ya hay alumos en este grado, borrelos e ingreselos denuevo");

            var allParents = _parentRepository.GetAllParents();
            var allStudents = _studentRepository.GetAllStudents();
            int iterator = 0;
            foreach (var pare in listParents)
            {
                var temp = allParents.Where(x => x.IdNumber == pare.IdNumber);
                if (!temp.Any())
                {
                    var newUser = new User
                    {
                        DisplayName = pare.FirstName,
                        Email = emails[iterator],//TODO: Possibly deprecated.
                        Password = _passwordGenerationService.GenerateTemporaryPassword(),
                        IsUsingDefaultPassword = true,
                        IsActive = true,
                        Role = _roleRepository.Filter(x => x.Name == "Padre").FirstOrDefault()
                    };
                    newUser.DefaultPassword = newUser.Password;
                    newUser = _userRepository.Create(newUser);
                    
                    pare.User = newUser;
                    _parentRepository.Create(pare);
                }
                else
                {
                    pare.Id = temp.First().Id;
                }
                iterator++;
            }
            foreach (var stu in listStudents)
            {
                var temp = allStudents.Where(x => x.IdNumber == stu.IdNumber);
                if (!temp.Any())
                {
                    _studentRepository.Create(stu);   
                }
                else
                    stu.Id = temp.First().Id;
                var enr = allEnrolls.Where(x => x.AcademicYearGrade.Id == academicYearGrade.Id && x.Student.Id == stu.Id);
                if (enr.Any()) continue;
                var te = new Enroll();
                var academicYearTemp = _academicYearRepository.GetById(academicYearGrade.Id);
                var studentTemp = _studentRepository.GetById(stu.Id);
                //te.AcademicYear = academicYearTemp;
                te.Student = studentTemp;
                _enrollRepository.Create(te);
            }
        }
    }
}
