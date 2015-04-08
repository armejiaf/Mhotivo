//using Mhotivo.App_Data;
//using Mhotivo.App_Data.Repositories;
//using Mhotivo.App_Data.Repositories.Interfaces;

using Mhotivo.Interface;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Implement;
using Mhotivo.Implement.Repositories;
using Mhotivo.Implement.Context;


using Mhotivo.Logic;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Mhotivo.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Mhotivo.App_Start.NinjectWebCommon), "Stop")]

namespace Mhotivo.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
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
            kernel.Bind<IBenefactorRepository>().To<BenefactorRepository>().InRequestScope();
            kernel.Bind<IClassActivityRepository>().To<ClassActivityRepository>().InRequestScope();
            kernel.Bind<IContactInformationRepository>().To<ContactInformationRepository>().InRequestScope();
            kernel.Bind<ICourseRepository>().To<CourseRepository>().InRequestScope();
            kernel.Bind<IAppointmentDiaryRepository>().To<AppointmentDiaryRepository>().InRequestScope();
            kernel.Bind<INotificationRepository>().To<NotificationRepositoryRepository>().InRequestScope();
            kernel.Bind<IEnrollRepository>().To<EnrollRepository>().InRequestScope();
            kernel.Bind<IGradeRepository>().To<GradeRepository>().InRequestScope();
            kernel.Bind<IMeisterRepository>().To<MeisterRepository>().InRequestScope();
            kernel.Bind<IParentRepository>().To<ParentRepository>().InRequestScope();
            kernel.Bind<IPeopleRepository>().To<PeopleRepository>().InRequestScope();
            kernel.Bind<IRoleRepository>().To<RoleRepository>().InRequestScope();
            kernel.Bind<IStudentRepository>().To<StudentRepository>().InRequestScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<IPensumRepository>().To<PensumRepository>().InRequestScope();
            kernel.Bind<AcademicYearLogic>().ToSelf().InRequestScope();
            kernel.Bind<INotificationTypeRepository>().To<NotificationTypeRepository>().InRequestScope();
            kernel.Bind<ISecurityRepository>().To<SecurityRepository>().InRequestScope();
            kernel.Bind<IHomeworkRepository>().To<HomeworkRepository>().InRequestScope();
            kernel.Bind<IImportDataRepository>().To<ImportDataRepository>().InRequestScope();
            kernel.Bind<IAreaRepository>().To<AreaRepository>().InRequestScope();
        }        
    }
}
