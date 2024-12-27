using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.VideoService
{


    public class VideoService : IVideoService
    {
        private readonly AppDbContext _context;

        public VideoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Video>> GetVideosAsync()
        {
            var videos = await _context.Videos
                .Include(v => v.Modulo)
                .ThenInclude(m => m.Curso)
                .ToListAsync();
            return videos;
        }

        public async Task<Video> GetVideoByIdAsync(int id)
        {
            var video = await _context.Videos
                .Include(v => v.Modulo)
                .SingleOrDefaultAsync(v => v.Id == id);
            return video;
        }
        public async Task<List<Video>> GetVideosByCursoIdAsync(int cursoId)
        {
            return await _context.Videos
                .Include(v => v.Modulo)
                .ThenInclude(m => m.Curso)
                .Where(v => v.Modulo.CursoId == cursoId)
                .ToListAsync();
        }

        public async Task<Video> Create(Video video)
        {
            await _context.AddAsync(video);
            await _context.SaveChangesAsync();
            return video;
        }

        public async Task<bool> Delete(int id)
        {
            var video = await GetVideoByIdAsync(id);
            if (video == null) return false;
            _context.Remove(video);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Video> Update(Video video)
        {
            _context.Update(video);
            await _context.SaveChangesAsync();
            return video;
        }

        public async Task<Video> GetVideoAnteriorAsync(int id)
        {
            var video = await _context.Videos
                .Include(a => a.Modulo)
                .ThenInclude(m => m.Curso)
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync(a => a.Id < id);
            return video;
        }

        public async Task<Video> GetProximoVideoAsync(int id)
        {
            var videoAtual = await _context.Videos
                .Include(v => v.Modulo)
                .ThenInclude(m => m.Curso)
                .FirstOrDefaultAsync(v => v.Id == id);

            var cursoId = videoAtual.Modulo.CursoId;

            var proximoVideo = await _context.Videos
                .Include(v => v.Modulo)
                .ThenInclude(m => m.Curso)
                .Where(v => v.Modulo.CursoId == cursoId)
                .OrderBy(v => v.Id)
                .FirstOrDefaultAsync(v => v.Id > id);
            return proximoVideo;
        }
    }

}