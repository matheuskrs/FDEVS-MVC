using FDevs.Models;

namespace FDevs.Services.VideoService;
public interface IVideoService
{
    Task<List<Video>> GetVideosAsync();
    Task<Video> GetVideoByIdAsync(int id);
    Task<Video> Create(Video video);
    Task<Video> Update(Video video);
    Task<bool> Delete(int id);
}
