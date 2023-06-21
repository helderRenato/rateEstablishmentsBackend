﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Areas.Identity.Data;
using Projeto.Models;

namespace Projeto.Controllers
{

    [ApiController]
    [Route("EstablishmentTransport")]
    public class EstablishmentTransportController : Controller
    {

        private ApplicationDBContext _dbcontext;

        public EstablishmentTransportController(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        private static ILogger _logger;


        /*public EstablishmentTransportController(ILogger<UserTransportController> logger)
        {
            _logger = logger;
        }*/

        [HttpGet]
        public ActionResult GetUser()
        {
            _logger.LogWarning("Entrou no método Get");
            List<Establishment> lista = new List<Establishment>();

            lista.ForEach((p) =>
            {
                lista.Add(new Establishment { Name = p.Name, Password = p.Password });
            });

            _logger.LogWarning("Saiu do método Get");
            return Ok(lista);
        }


        [HttpPost("create")]
        public IActionResult CreateEstablishment([FromForm] EstablishmentTransportModel establishmentAux)
        {

            //var Idax = _dbcontext.Establishment.Max()!.Id + 1;

            //var Idax = _dbcontext.Establishment.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id) + 1 ;
            //Establishment establishment = new Establishment { Id = Idax };
            
            //var IdMax = _dbcontext.Establishment.MaxBy(e => e.Id);
            var test = _dbcontext.Establishment.OrderByDescending(e=>e.Id).FirstOrDefault();
            

            Establishment establishment = new Establishment();
            //establishment.Id = test.Id +1;
            establishment.Name = establishmentAux.Name;
            establishment.Address = establishmentAux.Address;
            establishment.Email = establishmentAux.Email;
            establishment.City = establishmentAux.City;
            establishment.Password = establishmentAux.Password;
            establishment.TypeEstablishment = establishmentAux.TypeEstablishment;
            establishment.Phone = "911111111";

            _dbcontext.Establishment.Add(establishment);
            _dbcontext.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = establishment.Id }, establishment);
        }


        [HttpPut("update")]
        public IActionResult UpdateEstablishment(Establishment updatedEstablishment)
        {
            var existingEstablishment = _dbcontext.Establishment.FirstOrDefault(wf => wf.Id == updatedEstablishment.Id);

            if (existingEstablishment == null)
            {
                return NotFound();
            }

            existingEstablishment.Name = updatedEstablishment.Name;
            existingEstablishment.Password = updatedEstablishment.Password;
            existingEstablishment.Email = updatedEstablishment.Email;
            existingEstablishment.City = updatedEstablishment.City;
            existingEstablishment.Address = updatedEstablishment.Address;
            existingEstablishment.Phone = updatedEstablishment.Phone;

            return NoContent();
        }


        [HttpDelete("delete/{id}")]
        public IActionResult DeleteEstablishment(int id)
        {
            var existingEstablishment = _dbcontext.Establishment.FirstOrDefault(wf => wf.Id == id);

            if (existingEstablishment == null)
            {
                return NotFound();
            }

            _dbcontext.Establishment.Remove(existingEstablishment);

            return NoContent();
        }

    }
}