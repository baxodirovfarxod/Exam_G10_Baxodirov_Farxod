using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserContacts.Bll.Dtos;

public class UserRoleGetDto
{
    public long UserRoleId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
