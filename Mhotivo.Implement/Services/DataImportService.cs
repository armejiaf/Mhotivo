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
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAcademicGradeRepository _academicGradeRepository;

        public DataImportService(IPasswordGenerationService passwordGenerationService, ITutorRepository tutorRepository,
            IStudentRepository studentRepository, IUserRepository userRepository,
            IRoleRepository roleRepository, IAcademicGradeRepository academicGradeRepository)
        {
            _passwordGenerationService = passwordGenerationService;
            _tutorRepository = tutorRepository;
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _academicGradeRepository = academicGradeRepository;
        }

        public void Import(DataSet oDataSet, AcademicGrade academicYearGrade)
        {
            var parentageString = new Dictionary<string, Parentage>
            {
                {"Madre", Parentage.Mother},
                {"Padre", Parentage.Father},
                {"Abuela (a)", Parentage.Grandfather},
                {"Tía (a)", Parentage.Uncle},
                {"Hermano (a)", Parentage.Brother},
                {"Otro", Parentage.Other}
            };
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
                    City = dtDatos.Rows[indice][26].ToString(),
                    State = dtDatos.Rows[indice][15].ToString(),
                };
                if (!newStudent.IdNumber.Contains('-'))
                    newStudent.IdNumber = newStudent.IdNumber.Insert(4, "-").Insert(9, "-");
                newStudent.FullName = (newStudent.FirstName + " " + newStudent.LastName).Trim();
                var newTutor = new Tutor
                {
                    IdNumber = dtDatos.Rows[indice][18].ToString(),
                    LastName = (dtDatos.Rows[indice][19] + " " + dtDatos.Rows[indice][20]).Trim(),
                    FirstName = dtDatos.Rows[indice][21].ToString(),
                    MyGender = Utilities.DefineGender(dtDatos.Rows[indice][22].ToString()),
                    Parentage = parentageString[dtDatos.Rows[indice][23].ToString()],
                    BirthDate = DateTime.FromOADate(Double.Parse(dtDatos.Rows[indice][24].ToString())),
                    State = dtDatos.Rows[indice][25].ToString(),
                    City = dtDatos.Rows[indice][26].ToString()
                };
                if (!newTutor.IdNumber.Contains('-'))
                    newTutor.IdNumber = newTutor.IdNumber.Insert(4, "-").Insert(9, "-");
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
            var iterator = 0;
            foreach (var tutor in listTutors)
            {
                var temp = _tutorRepository.Filter(x => x.IdNumber == tutor.IdNumber);
                if (!temp.Any())
                {
                    _tutorRepository.Create(tutor);
                    var newUser = new User
                    {
                        UserOwner = tutor,
                        Email = emails[iterator],//TODO: Possibly deprecated.
                        Password = _passwordGenerationService.GenerateTemporaryPassword(),
                        IsUsingDefaultPassword = true,
                        IsActive = true,
                        Role = _roleRepository.Filter(x => x.Name == "Tutor").FirstOrDefault()
                    };
                    newUser.DefaultPassword = newUser.Password;
                    newUser = _userRepository.Create(newUser);
                    tutor.User = newUser;
                    _tutorRepository.Update(tutor);
                }
                iterator++;
            }
            foreach (var stu in listStudents)
            {
                var temp = _studentRepository.Filter(x => x.IdNumber == stu.IdNumber);
                if (!temp.Any())
                {
                    _studentRepository.Create(stu);
                    var studentTemp = _studentRepository.GetById(stu.Id);
                    academicYearGrade.Students.Add(studentTemp);
                    studentTemp.MyGrade = academicYearGrade;
                    _studentRepository.Update(studentTemp);
                    _academicGradeRepository.Update(academicYearGrade);
                }
                else
                {
                    var studentTemp = temp.FirstOrDefault();
                    if (studentTemp.MyGrade != null) throw new Exception("Uno o mas de los estudiantes esta actualmente matriculado en otra seccion.");
                    academicYearGrade.Students.Add(studentTemp);
                    studentTemp.MyGrade = academicYearGrade;
                    _studentRepository.Update(studentTemp);
                    _academicGradeRepository.Update(academicYearGrade);
                }
            }
        }
    }
}
