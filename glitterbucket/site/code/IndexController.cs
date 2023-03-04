using Microsoft.AspNetCore.Mvc;

namespace GlitterBucket
{
    public class IndexController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Content("GlitterBucket - the bucket for GitterAudit");
        }
    }
}
