using System.Xml.Linq;
using Vibe.Entities;

namespace Vibe.WebApi.Data
{
    public class SingerRepository : ISingerRepository
    {
        private List<Singer> singers = new()
    {
        new Singer
        {
            Id = 1,
            Name = "Eunice Kathleen Waymon",
            Nickname = "Nina Simone",
            BirthDate = new DateTime(1933,2,21),
            Photo = "nina.jfif"
        },
        new Singer
        {
            Id = 2,
            Name = "Michael Joseph Jackson",
            Nickname = "Michael Jackson",
            BirthDate = new DateTime(1985,8,29),
            Photo = "mickael.jfif"
        },
        new Singer
        {
            Id = 3,
            Name = "Fatima Ibrahim",
            Nickname = "Oum Kalthoum",
            BirthDate = new DateTime(1898,12,18),
            Photo = "kolthoum.jfif"
        },
        new Singer
        {
            Id = 4,
            Name = "Francis Albert Sinatra",
            Nickname = "Frank Sinatra",
            BirthDate = new DateTime(1915,12,12),
            Photo = "sinatra.jfif"
        }
    };
        public async Task<Singer> Add(Singer singer)
        {
            singer.Id = 5;
            singer.Name = "Amy Winehouse";
            singer.Nickname = "Amy Winehouse";
            singer.BirthDate = new DateTime(1983, 9, 14);
            singer.Photo = "amy.jfif";
            await Task.Run(() => singers.Add(singer));
            return singer;
        }

        public async Task<IEnumerable<Singer>> GetAll()
        {
            return await Task.Run(() => singers);
        }

        public async Task<Singer> GetOne(int id)
        {            
            return await Task.Run(() => singers.FirstOrDefault(s => s.Id == id));
        }
    }
}
