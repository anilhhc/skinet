using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        public readonly StoreContext _storeContext ;
        public BuggyController(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing=_storeContext.products.Find(42);
            if(thing==null){
                return NotFound(new ApiResponse(404));
            }
         return Ok();   
        }
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing=_storeContext.products.Find(42);
            var thingToReturn=thing.ToString();
         return Ok();   
        }
        [HttpGet("badrequest")]
        public ActionResult GetNotFound()
        {
         return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
         return Ok();
        }
    }
}