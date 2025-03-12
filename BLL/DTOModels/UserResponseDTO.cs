using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class UserResponseDTO
    {
        public int Id { get; init; }
        public string Login { get; init; }
        public string Type { get; init; }
        public bool IsActive { get; init; }
        public int GroupId { get; init; }

    }
}
