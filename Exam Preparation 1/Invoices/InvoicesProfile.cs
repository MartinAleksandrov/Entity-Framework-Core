using AutoMapper;
using Invoices.Data.Models;
using Invoices.DataProcessor.ExportDto;
using System.Globalization;

namespace Invoices
{
    public class InvoicesProfile : Profile
    {
        public InvoicesProfile()
        {
            this.CreateMap<Invoice, ExportInvoices>().
               ForMember(i => i.Number, opt => opt.MapFrom(d => d.Number))
               .ForMember(i => i.Amount, opt => opt.MapFrom(d => d.Amount))
               .ForMember(i => i.DueDate, opt => opt.MapFrom(d => d.DueDate.ToString("d", CultureInfo.InvariantCulture)))
               .ForMember(i => i.Currency, opt => opt.MapFrom(d => d.CurrencyType.ToString()));

            this.CreateMap<Client,ExportClientsInvoices>()
                .ForMember(s => s.Name,opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.NumberVat, opt => opt.MapFrom(d => d.NumberVat))
                .ForMember(s => s.Invoices, opt => opt.MapFrom(d => d.Invoices.ToArray().OrderBy(i => i.IssueDate)
                                        .ThenByDescending(i => i.DueDate)));
        }
    }
}
