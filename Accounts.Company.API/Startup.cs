using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using CORE2;
using Accounts.Service.Validators;
using System.Reflection;
using Accounts.Service.Contract.Commands;
using DocumentDB.Repository;
using Accounts.Service.Contract.Repository;
using Accounts.Service.Contract.Repository.Interfaces;
using Accounts.Service.CommandHandlers.Default;
using Accounts.Service.QueryHandlers;
using Accounts.Service.Contract.Queries;
using Accounts.Service.Contract.DTOs;
using Accounts.Service.Authorisers;
using Accounts.Company.API.Middlewares;

namespace Accounts.Company.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }
        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {


            // get the Azure DocumentDB client
            IDbInitializer init = new DbInitializer();

            string endpointUrl = Configuration.GetSection("azure:documentdb:endpointUrl").Value;
            string authorizationKey = Configuration.GetSection("azure:documentdb:authorizationKey").Value;
            string databaseId = Configuration.GetSection("azure:documentdb:dbName").Value;
            // get the Azure DocumentDB client    
            var client = init.GetClient(endpointUrl, authorizationKey);

            builder.Register(c => new DocumentServiceSettings()
            {
                Client = client,
                DatabaseId = databaseId
            }).As<DocumentServiceSettings>(); ;


            builder.RegisterType<GroupRepository>().As<IGroupRepository>().WithParameter("partitionKey", "/groupId"); ;

            builder.RegisterAuthorisersAndValidators();

            builder.RegisterAddGroupCommmandHelpers();
            builder.RegisterDeleteGroupCommmandHelpers();

            builder.RegisterSuspendGroupCommmandHelpers();

            builder.RegisterReactivateGroupCommmandHelpers();

            builder.RegisterGetAllGroupQueryHelpers();


            builder.RegisterType<GetGroupQueryHandler>()
                .As<IQueryHandler<GetGroupSmallDetailDTOQuery, GroupSmallDetailDTO>>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();

        }


    }

    public static class DIExtensions
    {

        public static void RegisterAuthorisersAndValidators(this ContainerBuilder builder)
        {
            // I have to use a type from required assemby to get assembly so I used DefaultAddGroupCommandHandler.
            // DefaultAddGroupCommandHandler is not something special we can use any type from that assembly 
            var serviceLayer = typeof(DefaultAddGroupCommandHandler).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(serviceLayer).Where(type => type.Name.EndsWith("Authoriser") || type.Name.EndsWith("Validator")).AsImplementedInterfaces().SingleInstance();
        }


        private static void RegisterCommandHandlersWithDecorators<TCommand,THandler>(this ContainerBuilder builder,string handlerKey,string validationDecoratorKey) where TCommand : Command where THandler : ICommandHandler<TCommand>
        {
            builder.RegisterType<THandler>().Named<ICommandHandler<TCommand>>(handlerKey);

            //Then register the decorator. The decorator uses the
            // named registrations to get the items to wrap.
            builder.RegisterDecorator<ICommandHandler<TCommand>>(
                (c, inner) => new ValidationDecorator<TCommand>(inner, c.Resolve<IRatifier<TCommand>>()),
                fromKey: handlerKey)
                .Keyed(validationDecoratorKey, typeof(ICommandHandler<TCommand>));

            builder.RegisterDecorator<ICommandHandler<TCommand>>(
               (c, inner) => new AuthoriserDecorator<TCommand>(inner, c.Resolve<ICompanyAuthoriser<TCommand>>()),
               fromKey: validationDecoratorKey);
        }

        public static void RegisterAddGroupCommmandHelpers(this ContainerBuilder builder)
        {
            string handlerKey = "addGroupHandler";
            string validationDecoratorKey = "addGroupValidationDecorator";

            builder.RegisterCommandHandlersWithDecorators<AddGroupCommand, DefaultAddGroupCommandHandler>(handlerKey, validationDecoratorKey);
        }


        public static void RegisterDeleteGroupCommmandHelpers(this ContainerBuilder builder)
        {
            string handlerKey = "deleteGroupHandler";
            string validationDecoratorKey = "deleteGroupValidationDecorator";

            builder.RegisterCommandHandlersWithDecorators<DeleteGroupCommand, DefaultDeleteGroupCommandHandler>(handlerKey, validationDecoratorKey);
        }

        public static void RegisterSuspendGroupCommmandHelpers(this ContainerBuilder builder)
        {
            string handlerKey = "suspendGroupHandler";
            string validationDecoratorKey = "suspendGroupValidationDecorator";

            builder.RegisterCommandHandlersWithDecorators<SuspendGroupCommand, SuspendGroupCommandHandler>(handlerKey, validationDecoratorKey);
        }

        public static void RegisterReactivateGroupCommmandHelpers(this ContainerBuilder builder)
        {
            string handlerKey = "reactivateGroupHandler";
            string validationDecoratorKey = "reactivateGroupValidationDecorator";

            builder.RegisterCommandHandlersWithDecorators<ReactivateGroupCommand, ReactivateGroupCommandHandler>(handlerKey, validationDecoratorKey);
        }

        private static void RegisterQueryHandlersWithDecorators<TQuery,TResult,THandler>(this ContainerBuilder builder, string handlerKey, string validationDecoratorKey) where TResult :DTO  where  TQuery : Query<TResult> where THandler : IQueryHandler<TQuery,TResult>
        {
            builder.RegisterType<THandler>()
               .Named<IQueryHandler<TQuery, TResult>>(handlerKey);

            builder.RegisterDecorator<IQueryHandler<TQuery, TResult>>(
                (c, inner) => new ValidationDecoratorForQueries<TQuery, TResult>(inner, c.ResolveOptional<IRatifier<TQuery>>()),
                fromKey: handlerKey)
                 .Keyed(validationDecoratorKey, typeof(IQueryHandler<TQuery, TResult>));


            builder.RegisterDecorator<IQueryHandler<TQuery, TResult>>(
                      (c, inner) => new AuthoriserDecoratorForQueries<TQuery, TResult>(inner, c.Resolve<ICompanyAuthoriser<TQuery>>()),
                      fromKey: validationDecoratorKey);
        }
        public static void RegisterGetAllGroupQueryHelpers(this ContainerBuilder builder)
        {
            string handlerKey = "getAllGroupQueryHandler";
            string validationDecoratorKey = "getAllGroupValidationDecorator";


            builder.RegisterQueryHandlersWithDecorators<GetAllGroupSmallDetailDTOQuery, ListGroupSmallDetailDTO, GetAllGroupQueryHandler>(handlerKey, validationDecoratorKey);
        }
    }
}
