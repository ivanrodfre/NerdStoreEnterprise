using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {

        private readonly HttpClient _httpClient;
        //private readonly AppSettings _settings;

        public AutenticacaoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            //Outra opção de chamar base url
            //Sempre irá setar esta configuração como base
            httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);
            _httpClient = httpClient;
            //opção 2
            //_settings = settings.Value;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {

            var loginContent = ObterConteudo(usuarioLogin);

            //Opção 1
            //var response = await _httpClient.PostAsync($"{_settings.AutenticacaoUrl}/api/identidade/autenticar", loginContent);
            //Opção 2
            var response = await _httpClient.PostAsync("/api/identidade/autenticar", loginContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DesserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DesserializarObjetoResponse<UsuarioRespostaLogin>(response);

        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DesserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DesserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
    }
}
