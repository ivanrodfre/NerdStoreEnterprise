using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {

        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }


        private void SetResponder()
        {
            _bus.RespondAsync<UsuarioRegistratoIntegrationEvent, ResponseMessage>(async request =>
                await RegistrarCliente(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Responder o resultado do método que chama o Handle para criar cliente
            SetResponder();
            //Caso o serviço de filas caia e depois volte
            _bus.AdvancedBus.Connected += OnConnect;

            return Task.CompletedTask;
        }

        //Caso caia,
        //vou registrar novamente a ideia de que estou esperando alguma coisa
        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        //Método que chama o Handle para criar cliente
        //Forma para trabalhar em um scopo singleton onde os objetos são scopeds, no caso o AddScoped<IMediatorHandler>
        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistratoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            ValidationResult sucesso;

            //Criando um scopo Singleton dentro de um método exclusivo
            //Método Scoped
            //ServiceLocator => basicamente mesma coisa de injetar via constrturor, porém não é indicado para todos os cenários, neste é requerido
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                sucesso = await mediator.EnviarComando(clienteCommand);
            }

            return new ResponseMessage(sucesso);

        }
    }
}
