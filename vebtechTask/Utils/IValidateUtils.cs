using vebtechTask.Models.DTO;

namespace vebtechTask.Utils;

public interface IValidateUtils
{
    Task ValidateCreate(UserDto userDto);
    Task ValidateUpdate(UserDto userDto);
}
