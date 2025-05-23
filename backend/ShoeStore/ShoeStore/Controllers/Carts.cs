﻿using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoeStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Carts : ControllerBase
    {
        // GET: api/<Carts>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value23322", "value2132132" };
        }

        // GET api/<Carts>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Carts>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Carts>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Carts>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
