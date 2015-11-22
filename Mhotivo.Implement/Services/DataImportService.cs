using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Excel;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly ITutorRepository _tutorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollRepository _enrollRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public DataImportService(IPasswordGenerationService passwordGenerationService, ITutorRepository tutorRepository, IStudentRepository studentRepository, IEnrollRepository enrollRepository, IAcademicYearRepository academicYearRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _passwordGenerationService = passwordGenerationService;
            _tutorRepository = tutorRepository;
            _studentRepository = studentRepository;
            _enrollRepository = enrollRepository;
            _academicYearRepository = academicYearRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public void Import(DataSet oDataSet, AcademicGrade academicYearGrade)
        {
            var emails = new List<string>();
            if(oDataSet.Tables.Count == 0)
                return;
            if(oDataSet.Tables[0].Rows.Count <= 15)
                return;
            var dtDatos = oDataSet.Tables[0];
            var listStudents = new List<Student>();
            var listTutors = new List<Tutor>();
            for (var indice = 15; indice < dtDatos.Rows.Count; indice++)
            {
                if(dtDatos.Rows[indice][2].ToString().Trim().Length == 0)
                    continue;
                var newStudent = new Student
                {
                    IdNumber = dtDatos.Rows[indice][2].ToString(),
                    LastName = (dtDatos.Rows[indice][3] + " " + dtDatos.Rows[indice][4]).Trim(),
                    FirstName = dtDatos.Rows[indice][6].ToString(),
                    MyGender = Utilities.DefineGender(dtDatos.Rows[indice][8].ToString()),
                    BirthDate = DateTime.FromOADate(Double.Parse(dtDatos.Rows[indice][9].ToString())),
                    State = dtDatos.Rows[indice][15].ToString(),
                };
                newStudent.FullName = (newStudent.FirstName + " " + newStudent.LastName).Trim();
                var newTutor = new Tutor
                {
                    IdNumber = dtDatos.Rows[indice][18].ToString()
                    ,LastName = (dtDatos.Rows[indice][19] + " " + dtDatos.Rows[indice][20]).Trim()
                    ,FirstName = dtDatos.Rows[indice][21].ToString()
                    ,MyGender = Utilities.DefineGender(dtDatos.Rows[indice][22].ToString())
                    ,BirthDate = DateTime.FromOADate(Double.Parse(dtDatos.Rows[indice][24].ToString()))
                    ,State = dtDatos.Rows[indice][25].ToString()
                    ,City = dtDatos.Rows[indice][26].ToString()
                };
                newTutor.FullName = (newTutor.FirstName + " " + newTutor.LastName).Trim();
                var newContactInformation = new ContactInformation
                {
                    Type = "Telefono"
                    ,Value = dtDatos.Rows[indice][27].ToString()
                    ,People = newTutor
                };
                emails.Add(dtDatos.Rows[indice][28].ToString());
                var listContacts = new List<ContactInformation> {newContactInformation};
                newTutor.ContactInformation = listContacts;
                newStudent.Tutor1 = newTutor;
                listStudents.Add(newStudent);
                listTutors.Add(newTutor);
            }
            SaveData(listStudents, listTutors, academicYearGrade, emails);
        }

        //modificado
        public DataSet GetDataSetFromExcelFile(HttpPostedFileBase getFile)
        {
            if (getFile == null || getFile.ContentLength <= 0) return new DataSet();
            var reader = ExcelReaderFactory.CreateBinaryReader(getFile.InputStream);
            if (Path.GetExtension(getFile.FileName) == ".xlsx")
                reader = ExcelReaderFactory.CreateOpenXmlReader(getFile.InputStream);
            var dataSet = reader.AsDataSet();
            return dataSet;
        }

        private void SaveData(IEnumerable<Student> listStudents, IEnumerable<Tutor> listTutors, AcademicGrade academicYearGrade, IReadOnlyList<string> emails)
        {
            var enrolls = _enrollRepository.Filter(x => x.AcademicGrade.Id == academicYearGrade.Id);
            if (enrolls.Any())
                throw new Exception("Ya hay alumos en este grado, borrelos e ingreselos de nuevo");
            var allTutors = _tutorRepository.GetAllTutors();
            var allStudents = _studentRepository.GetAllStudents();
            int iterator = 0;
            foreach (var pare in listTutors)
            {
                var temp = allTutors.Where(x => x.IdNumber == pare.IdNumber);
                if (!temp.Any())
                {
                    var newUser = new User
                    {
                        UserOwner = pare,
                        Email = emails[iterator],//TODO: Possibly deprecated.
                        Password = _passwordGenerationService.GenerateTemporaryPassword(),
                        IsUsingDefaultPassword = true,
                        IsActive = true,
                        Role = _roleRepository.Filter(x => x.Name == "Tutor").FirstOrDefault()
                    };
                    newUser.DefaultPassword = newUser.Password;
                    newUser = _userRepository.Create(newUser);
                    
                    pare.User = newUser;
                    _tutorRepository.Create(pare);
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
                //var enr = allEnrolls.Where(x => x.AcademicGrade.Id == academicYearGrade.Id && x.Student.Id == stu.Id);
                //if (enr.Any()) continue;
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
