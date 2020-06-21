using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CatalogoController : MainController
    {
        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> index()
        {
            return View();
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe()
        {
            return View();
        }
    }
}
