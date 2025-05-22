using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserContacts.Bll.Dtos;

public class ContactDto : ContactCreateDto
{
    public long ContactId { get; set; }
}
