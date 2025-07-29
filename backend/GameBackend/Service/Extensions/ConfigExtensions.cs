using DataBase.Models;
using static Service.Middleware.CustomException;

namespace Service.Extensions
{
    public class ConfigExtensions
    {
        public static int GetEnvInt(string name, int defaultValue)
        {
            var value = Environment.GetEnvironmentVariable($"TICTACTOE_{name}");
            return int.TryParse(value, out var result) ? result : defaultValue;
        }
        public static void ValidateConfiguration(GameConfiguration config)
        {
            if (config.DefaultBoardSize < 3)
            {
                throw new BadRequestException("Размер поля не может быть меньше 3");
            }

            if (config.WinCondition < 3 || config.WinCondition > config.DefaultBoardSize)
            {
                throw new BadRequestException("Условие победы должно быть от 3 до размера поля");
            }

            if (config.RandomMoveChancePercent < 0 || config.RandomMoveChancePercent > 100)
            {
                throw new BadRequestException("Вероятность должна быть от 0 до 100%");
            }
        }
    }
}
