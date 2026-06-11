using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dental.Shared.Models;
using Dental.API.Data;

namespace Dental.API.Controllers
{
    /*
     * i have used controllers before, so before writing the code i must first explain waht controllers are, why they are necessary for this project structure, and 
     * then waht methods we will need to write then start immediately.
     * All the nots will be written, first lets write the code
     */
    [ApiController]
    [Route("Api/[controller]")]

    public class PatientsController : ControllerBase
    {
        // we need to inject the database context to access the databse
        private readonly AppDbContext _appDbContext; // need to go and write the AppDbContext class first so that this line works

        public PatientsController(AppDbContext appDbContext)
        {
            appDbContext = _appDbContext;
        }
      
    }

}



