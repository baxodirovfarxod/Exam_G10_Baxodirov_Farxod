using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserContacts.Bll.Dtos;
using UserContacts.Core.Errors;
using UserContacts.Dal;
using UserContacts.Dal.Entities;

namespace UserContacts.Bll.Services.ContactServices;

public class ContactService : IContactService
{
    private readonly ILogger<ContactService> logger;
    private readonly MainContext mainContext;
    private readonly IMapper mapper;

    public ContactService(MainContext mainContext, IMapper mapper)
    {
        this.mainContext = mainContext;
        this.mapper = mapper;
    }

    public async Task<long> AddContactAsync(ContactCreateDto contactCreateDto, long userId)
    {
        var contact = mapper.Map<Contact>(contactCreateDto);
        contact.UserId = userId;
        mainContext.Contacts.Add(contact);
        await mainContext.SaveChangesAsync();
        return contact.ContactId;
    }

    public async Task DeleteContactAsync(long contactId, long userId)
    {
        var contact = await GetContactByIdAsync(contactId, userId);
        mainContext.Contacts.Remove(mapper.Map<Contact>(contact));
        await mainContext.SaveChangesAsync();
    }

    public async Task<List<ContactDto>> GetAllContactsAsync(long userId)
    {
        var contacts = await mainContext.Contacts
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return mapper.Map<List<ContactDto>>(contacts);
    }

    public async Task<ContactDto> GetContactByIdAsync(long contactId, long userId)
    {
        var contact = await mainContext.Contacts
            .FirstOrDefaultAsync(c => c.ContactId == contactId && c.UserId == userId);

        if (contact == null)
        {
            throw new EntityNotFoundException();
        }

        return mapper.Map<ContactDto>(contact);
    }

    public async Task UpdateContactAsync(ContactDto contactDto, long userId)
    {
        var contact = mapper.Map<Contact>(GetContactByIdAsync(contactDto.ContactId, userId));
        mapper.Map(contactDto, contact);
        mainContext.Contacts.Update(contact);
        await mainContext.SaveChangesAsync();
    }
}
