using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Repository
{
   public interface IFragranceRepository
    {
        ICollection<Fragrance> GetFragrances();

        Fragrance GetFragrance(int fragranceId);

        bool FragranceExists(string brand, string name);

        bool FragranceExists(int id);

        bool CreateFragrance(Fragrance fragrance);

        bool UpdateFragrance(Fragrance fragrance);

        bool DeleteFragrance(Fragrance fragrance);

        bool Save();

    }
}
