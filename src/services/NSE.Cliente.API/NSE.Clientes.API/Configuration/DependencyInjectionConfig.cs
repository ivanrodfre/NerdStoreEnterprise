using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSE.Clientes.API.Application.Commands;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Data;
using NSE.Clientes.API.Data.Repository;
using NSE.Clientes.API.Models;
using NSE.Clientes.API.Services;
using NSE.Core.Mediator;

namespace NSE.Clientes.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClientesContext>();

            #region Explicação
            //Este objeto funciona de forma Singleton
            //Não podemos misturar Singleton com Scoped, ex: não podemos injetar o IMediatorHandler dentro de RegistroClienteIntegrationHandler
            //Pois, estamos juntando uma instância Scoped com Singleton,
            //Atenção na injeção, pois temos instâncias que não podem ser Singleton, ex: Contexto não pode ser Singleton,
            //Ou tudo vira Singleton, ou seguiremos por outro caminho
            //Como é um serviço hospedado, ele tem que trabalhar no modelo Singleton, não pode travalhar por request / scoped, tem que trabalhar dentro do pipeline do aspnet, como um só
            //Comentei aqui, pois ficou na responsabilidade da abstração
            #endregion
            //services.AddHostedService<RegistroClienteIntegrationHandler>();
        }
    }
}
