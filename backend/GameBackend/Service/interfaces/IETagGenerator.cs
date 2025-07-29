using DataBase.DTO.Response;

namespace Service.Service;

public interface IETagGenerator
{
    string Generate(GameDto game);
}