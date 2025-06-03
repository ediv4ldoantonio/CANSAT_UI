using CANSAT_UI.Models;

namespace CANSAT_UI.Repositories;

public interface IDataRepository
{
    Task Create(Data data);
    Task CreateAlert(Alert data);
    Task<List<Data>> Read();
    Task<List<Alert>> ReadAlert();
}
