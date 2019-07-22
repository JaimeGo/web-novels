using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using web_novels_backend.Models;

namespace web_novels_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebnovelsController : ControllerBase
    {
        private readonly WebnovelContext _context;
        private readonly IFileProvider fileProvider;
        private readonly IHostingEnvironment hostingEnvironment;
        public WebnovelsController(WebnovelContext context, IFileProvider fileprovider, IHostingEnvironment env)
        {
            _context = context;
            fileProvider = fileprovider;
            hostingEnvironment = env;
        }
        // GET api/webnovels
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            List<Webnovel> webnovels = await _context.Webnovels.ToListAsync();
            return Ok(webnovels);
        }

        // GET api/webnovels/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(long id)
        {
            Webnovel webnovel = await _context.Webnovels.FindAsync(id);
            if (webnovel==null)
            {
                return NotFound(id);
            }
            return Ok(webnovel);
        }

        // POST api/webnovels
        [HttpPost]
        public async Task<ActionResult> PostWebnovel([Bind("Title,Author,Translator,Description")] Webnovel webnovel, IFormFile file)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Add(webnovel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e);
            }
                
            try
            {
                // Code to upload image if not null
                if (file != null || file.Length != 0)
                {
                    // Create a File Info 
                    FileInfo fi = new FileInfo(file.FileName);

                    // This code creates a unique file name to prevent duplications 
                    // stored at the file location
                    var newFilename = webnovel.Id + "_" + String.Format("{0:d}",
                                        (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                    var webPath = hostingEnvironment.WebRootPath;
                    var path = Path.Combine("", webPath + @"\ImageFiles\" + newFilename);

                    // IMPORTANT: The pathToSave variable will be save on the column in the database
                    var pathToSave = @"/ImageFiles/" + newFilename;

                    // This stream the physical file to the allocate wwwroot/ImageFiles folder
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // This save the path to the record
                    webnovel.ImagePath = pathToSave;
                    _context.Update(webnovel);
                    await _context.SaveChangesAsync();

                } else
                {
                    System.Diagnostics.Debug.WriteLine("No image given");
                }
                    

            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Image was not loaded: {0}", e);
                return BadRequest(e);
            }

            return Ok(webnovel);
  
        }

        // PUT api/webnovels/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(long id, [Bind("Id,Title,Author,Translator,Description")] Webnovel webnovel)
        {
            if (id != webnovel.Id)
            {
                return BadRequest();
            }

            _context.Entry(webnovel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/webnovels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Webnovel webnovel = await _context.Webnovels.FindAsync(id);
            if (webnovel == null)
            {
                return NotFound(id);
            }


            _context.Webnovels.Remove(webnovel);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
