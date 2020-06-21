using NSE.WebApp.MVC.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Handlers
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IUser _user;

        public HttpClientAuthorizationDelegatingHandler(IUser user)
        {
            _user = user;
        }

        /*
         * Quando cria um DelegatingHandler, de certa forma estamos sobrescrevendo o método SendAsync do httpClient
         * Destro do escopo do método, podemos fazer o que quisermos dentro do request
         */
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Saber se temos a chave Authorization dentro do request
            var authorizationHandler = _user.ObterHttpContext().Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHandler))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHandler });
            }

            var token = _user.ObterUserToken();

            if(token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            //Uma vez que adicionamos estas informações dentro do request, podemos continuar com o processo normal de request do SendAsync
            return base.SendAsync(request, cancellationToken);
        }

    }
}
