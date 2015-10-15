using Mhotivo.Implement.Context;
using Mhotivo.Implement.Repositories;
using Mhotivo.Interface.Interfaces;
using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Mhotivo.ParentSite.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Mhotivo.ParentSite.NinjectWebCommon), "Stop")]

namespace Mhotivo.ParentSite
{
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
            kernel.Bind<IAcademicYearRepository>().To<AcademicYearRepository>().InRequestScope();
            kernel.Bind<INotificationRepository>().To<NotificationRepository>().InRequestScope();
            kernel.Bind<IPeopleRepository>().To<PeopleRepository>().InRequestScope();
            kernel.Bind<ISessionManagementRepository>().To<SessionManagementRepository>().InRequestScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<ISecurityRepository>().To<SecurityRepository>().InRequestScope();
            kernel.Bind<IParentRepository>().To<ParentRepository>().InRequestScope();
            kernel.Bind<IEducationLevelRepository>().To<EducationLevelRepository>().InRequestScope();
            kernel.Bind<IGradeRepository>().To<GradeRepository>().InRequestScope();
            kernel.Bind<ICourseRepository>().To<CourseRepository>().InRequestScope();
            kernel.Bind<IStudentRepository>().To<StudentRepository>().InRequestScope();
            kernel.Bind<IEnrollRepository>().To<EnrollRepository>().InRequestScope();
            kernel.Bind<IAcademicYearDetailsRepository>().To<AcademicYearDetailsRepository>().InRequestScope();
            kernel.Bind<IHomeworkRepository>().To<HomeworkRepository>().InRequestScope();
            kernel.Bind<INotificationCommentRepository>().To<NotificationCommentRepository>().InRequestScope();
            kernel.Bind<ITeacherRepository>().To<TeacherRepository>().InRequestScope();
        }        
    }
}
