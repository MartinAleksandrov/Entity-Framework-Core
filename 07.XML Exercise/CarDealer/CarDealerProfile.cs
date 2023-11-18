using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Import Supplier
            this.CreateMap<ImportSuppliersDto, Supplier>();

            //Import Cars
            this.CreateMap<ImportCarsDto, Car>()
                .ForSourceMember(s => s.Pasrts, opt => opt.DoNotValidate());

            //Import Customers
            this.CreateMap<ImportCustomerDto, Customer>()//In EF better practice is to make datetime as string and parse it in busiines logic
                .ForMember(s => s.BirthDate, opt => opt.MapFrom(d => DateTime.Parse(d.BirthDate)));

            //Import Sales 
            this.CreateMap<ImportSaleDto, Sale>()
               .ForMember(d => d.CarId,
                   opt => opt.MapFrom(s => s.CarId!.Value));
        }
    }
}
