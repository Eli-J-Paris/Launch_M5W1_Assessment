using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordCollection.DataAccess;
using Serilog;

namespace RecordCollection.Controllers
{
    [Route("api/albums")]
    [ApiController]
    public class AlbumsAPIController : ControllerBase
    {
        private readonly RecordCollectionContext _context;

        public AlbumsAPIController(RecordCollectionContext context, Serilog.ILogger logger)
        {
            _context = context;
        }

        public IActionResult GetAll()
        {
            var albums = _context.Albums.ToList();
            Log.Information("All albums were requested");

            return new JsonResult(albums);
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);
            if(album== null)
            {
                Log.Warning("Album was not found");
                return BadRequest();
            }
            return new JsonResult(album);
        }

        [HttpDelete("{id}")]
        public void DeleteOne(int id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);
            _context.Albums.Remove(album);
            Log.Fatal($"Success! {album.Title} was removed from the database.");
            _context.SaveChanges();
        }
    }
}
