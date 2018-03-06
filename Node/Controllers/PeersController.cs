using Microsoft.AspNetCore.Mvc;
using Node.Interfaces;

namespace Node.Controllers
{
    [Route("api/peers")]
    public class PeersController : Controller
    {
        private readonly IPeerService _peerService;

        public PeersController(IPeerService peerService)
        {
            this._peerService = peerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("sync")]
        public IActionResult Sync()
        {
            return Ok();
        }
    }
}