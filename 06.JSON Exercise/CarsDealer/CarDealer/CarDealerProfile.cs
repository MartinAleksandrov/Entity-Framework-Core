using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Suppliers
            this.CreateMap<ImportSuppliersDto, Supplier>();

            //Parts
            this.CreateMap<ImportPartsDto, Part>();

            //Cars
            this.CreateMap<ImportCarsDto, Car>();

            //Customers
            this.CreateMap<ImportCustomerDto, Customer>();

            //Sales
            this.CreateMap<ImportSalesDto, Sale>();
        }
    }
}
