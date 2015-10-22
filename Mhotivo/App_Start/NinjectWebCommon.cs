using Mhotivo.Interface.Interfaces;
using Mhotivo.Implement.Repositories;
using Mhotivo.Implement.Context;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Mhotivo.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Mhotivo.NinjectWebCommon), "Stop")]

namespace Mhotivo
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<MhotivoContext>().ToSelf().InRequestScope();
            kernel.Bind<ISessionManagementRepository>().To<SessionManagementRepository>().InRequestScope();
            kernel.Bind<IAcademicYearRepository>().To<AcademicYearRepository>().InRequestScope();
            kernel.Bind<IAcademicYearDetailsRepository>().To<AcademicYearDetailsRepository>().InRequestScope();
            kernel.Bind<IContactInformationRepository>().To<ContactInformationRepository>().InRequestScope();
            kernel.Bind<ICourseRepository>().To<CourseRepository>().InRequestScope();
            kernel.Bind<INotificationRepository>().To<NotificationRepository>().InRequestScope();
            kernel.Bind<IEnrollRepository>().To<EnrollRepository>().InRequestScope();
            kernel.Bind<IGradeRepository>().To<GradeRepository>().InRequestScope();
            kernel.Bind<ITeacherRepository>().To<TeacherRepository>().InRequestScope();
            kernel.Bind<IParentRepository>().To<ParentRepository>().InRequestScope();
            kernel.Bind<IPeopleRepository>().To<PeopleRepository>().InRequestScope();
            kernel.Bind<IStudentRepository>().To<StudentRepository>().InRequestScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<IPensumRepository>().To<PensumRepository>().InRequestScope();
            kernel.Bind<INotificationTypeRepository>().To<NotificationTypeRepository>().InRequestScope();
            kernel.Bind<ISecurityRepository>().To<SecurityRepository>().InRequestScope();
            kernel.Bind<IHomeworkRepository>().To<HomeworkRepository>().InRequestScope();
            kernel.Bind<IImportDataRepository>().To<ImportDataRepository>().InRequestScope();
            kernel.Bind<IEducationLevelRepository>().To<EducationLevelRepository>().InRequestScope();
            kernel.Bind<IPasswordGenerationService>().To<RandomPasswordGenerationService>().InRequestScope();
            kernel.Bind<INotificationCommentRepository>().To<NotificationCommentRepository>().InRequestScope();
        }        
    }
}
