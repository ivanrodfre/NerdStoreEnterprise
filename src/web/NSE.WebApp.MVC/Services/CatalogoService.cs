using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class CatalogoService : Service, ICatalogoService
    {

        private readonly HttpClient _httpClient;

        public CatalogoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
            _httpClient = httpClient;
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            /*
             * Obs: Verificar se o usuário está autenticado para acessar, 
             * existem 2 formas, passando complemento via header como parametro no GetAsync("","")
             * ou interceptando em todas as requisições com DelegateHendlers, passando o token via manipuladores delegados
             * Doc Microsoft: IHttpClientFactory
             */
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

            TratarErrosResponse(response);

            return await DesserializarObjetoResponse<ProdutoViewModel>(response);
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/catalogo/produtos/");

            TratarErrosResponse(response);

            return await DesserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);

        }
    }
}
