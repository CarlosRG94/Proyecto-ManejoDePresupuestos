using AutoMapper;
using ManejoDePresupuestos.Models;

namespace ManejoDePresupuestos.Servicios
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta,CuentaCreacionViewModel>();
            CreateMap<TransaccionActualizacionViewModel,Transaccion>().ReverseMap();
        }
    }
}
