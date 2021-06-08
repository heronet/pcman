using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pcman.Data;
using pcman.Models;

namespace pcman.Controllers
{
    [Authorize]
    public class ComputersController : DefaultController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<EntityUser> _userManager;
        public ComputersController(ApplicationDbContext dbContext, UserManager<EntityUser> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        [HttpGet(Name = "GetComputers")]
        public async Task<ActionResult<IEnumerable<Computer>>> GetComputers()
        {
            var computers = await _dbContext.Computers.ToListAsync();
            return Ok(computers);
        }
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetComputer")]
        public async Task<ActionResult<Computer>> GetComputer(Guid id)
        {
            var computer = await _dbContext.Computers.Where(c => c.Id == id).SingleOrDefaultAsync();
            if (computer == null)
                return BadRequest("Invalid Computer Id");
            return Ok(computer);
        }
        [HttpPost]
        public async Task<ActionResult<Computer>> AddComputer(Computer computer)
        {
            var newComputer = new Computer
            {
                Name = computer.Name,
                Ram = computer.Ram,
                Hdd = computer.Hdd,
                Motherboard = computer.Motherboard.Trim(),
                Monitor = computer.Monitor.Trim(),
                Cpu = computer.Cpu.Trim(),
                Psu = computer.Psu.Trim(),
                Description = computer.Description.Trim()
            };
            _dbContext.Computers.Add(newComputer);

            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok();
            return BadRequest("Can't Add Computer");
        }
        [HttpPut(Name = "ModifyComputer")]
        public async Task<ActionResult<Computer>> ModifyComputer(Computer computer)
        {
            var pc = await _dbContext.Computers.Where(c => c.Id == computer.Id).SingleOrDefaultAsync();
            if (pc == null)
                return BadRequest("Invalid Computer Id");
            pc.Ram = computer.Ram;
            pc.Motherboard = computer.Motherboard;
            pc.Monitor = computer.Monitor;
            pc.Cpu = computer.Cpu;
            pc.Psu = computer.Psu;
            pc.Description = computer.Description;

            _dbContext.Computers.Update(pc);
            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok(pc);
            return BadRequest("Can't Update Computer");
        }
        [HttpDelete("{id}", Name = "DestroyComputer")]
        public async Task<ActionResult<Computer>> DestroyComputer(Guid id)
        {
            var computer = await _dbContext.Computers.Where(c => c.Id == id).SingleOrDefaultAsync();
            if (computer == null)
                return BadRequest("Invalid Computer Id");
            _dbContext.Remove(computer);
            if (await _dbContext.SaveChangesAsync() > 0)
                return NoContent();
            return BadRequest("Couldn't Destroy Computer");
        }

    }
}