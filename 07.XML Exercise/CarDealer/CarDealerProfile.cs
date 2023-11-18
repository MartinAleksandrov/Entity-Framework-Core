using AutoMapper;
using CarDealer.DTOs.Export;
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
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());

            //Import Customers
            this.CreateMap<ImportCustomerDto, Customer>()
                //In EF better practice is to make datetime as string and parse it in busiines logic
                .ForMember(s => s.BirthDate, opt => opt.MapFrom(d => DateTime.Parse(d.BirthDate)));

            //Import Sales 
            this.CreateMap<ImportSaleDto, Sale>()
               .ForMember(d => d.CarId,
                   opt => opt.MapFrom(s => s.CarId!.Value));


            //Export Car
            this.CreateMap<Car, ExportCarsDto>();

            //Export Supplier 
            this.CreateMap<Supplier, ExportSupplierIdNamePartsCountDto>()
                .ForMember(s => s.Count,opt => opt.MapFrom(d => d.Parts.Count));


            //Export Cars whith Parts for them
            this.CreateMap<Car, ExportCarsWhithPartsDto>()
                .ForMember(s => s.Parts,opt => opt.MapFrom(d => d.PartsCars.Select(p => p.Part)
                                                                           .OrderByDescending(p => p.Price)
                                                                           .ToArray()));
            //This is for nested dto in ExportCarsWhithPartsDto
            this.CreateMap<Part, ExportPartsForCarsDto>();

            //Export Sale
            //this.CreateMap<Sale, ExportSalesDto>();

        }
    }
}
