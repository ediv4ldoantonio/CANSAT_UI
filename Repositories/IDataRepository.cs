using CANSAT_UI.Models;

namespace CANSAT_UI.Repositories;

public interface IDataRepository
{
    Task Create(Data data);
    Task<List<Data>> Read();
}
