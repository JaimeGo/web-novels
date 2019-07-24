using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_novels_backend.Models;

namespace web_novels_backend.Controllers
{
    [Route("api/webnovels/{webnovelId}/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly WebnovelContext _context;
        public ChaptersController(WebnovelContext context)
        {
            _context = context;
        }

        // GET api/webnovels/{webnovelId}/chapters/
        [HttpGet]
        public async Task<IActionResult> IndexChapters([FromRoute] long webnovelId)
        {
            Webnovel webnovel = await _context.Webnovels.FindAsync(webnovelId);
            List<Chapter> chapters = webnovel.Chapters.Where(c => c.Webnovel.Id == webnovelId).ToList();
            return Ok(chapters);
        }

        // GET api/webnovels/{webnovelId}/chapters/{id}
        [HttpGet("{chapterId}")]
        public async Task<ActionResult> GetChapter([FromRoute] long webnovelId, long chapterId)
        {
            Webnovel webnovel = await _context.Webnovels.FindAsync(webnovelId);
            Chapter chapter = webnovel.Chapters.FirstOrDefault(c=>c.Id==chapterId);

            if (chapter == null)
            {
                return NotFound(chapterId);
            }

            return Ok(chapter);
        }



        // POST: api/webnovels/{webnovelId}
        [HttpPost]
        public async Task<ActionResult> PostChapter([FromRoute] long webnovelId, [Bind("Title,Text")]Chapter chapter)
        {
            Webnovel webnovel = await _context.Webnovels.FindAsync(webnovelId);
            webnovel.Chapters.Add(chapter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChapter), new { id = chapter.Id }, chapter);
        }

        // PUT api/webnovels/{webnovelId}/chapters/{chapterId}
        [HttpPut("{chapterId}")]
        public async Task<ActionResult> PutChapter([FromRoute] long webnovelId,long chapterId, [Bind("Title,Text")]Chapter chapter)
        {
            if (chapterId != chapter.Id)
            {
                return BadRequest();
            }
            Webnovel webnovel = await _context.Webnovels.FindAsync(webnovelId);
            Chapter chapterInModel = webnovel.Chapters.FirstOrDefault(c => c.Id == chapterId);
            chapterInModel.Text = chapter.Text;
            chapterInModel.Title = chapter.Title;

            _context.Entry(webnovel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/webnovels/{webnovelId}/chapters/{chapterId}
        [HttpDelete("{chapterId}")]
        public async Task<ActionResult> DeleteChapter([FromRoute] long webnovelId, long chapterId)
        {
            Webnovel webnovel = await _context.Webnovels.FindAsync(webnovelId);
            Chapter chapter = webnovel.Chapters.FirstOrDefault(c => c.Id == chapterId);
            if (webnovel == null)
            {
                return NotFound(webnovelId);
            }


            webnovel.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            return NoContent();

        }


    }


}
