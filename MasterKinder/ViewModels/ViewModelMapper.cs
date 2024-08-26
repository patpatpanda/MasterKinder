using KinderReader.Models;
using MasterKinder.Models;
using System.Linq;

namespace MasterKinder.ViewModels
{
    public static class ViewModelMapper
    {
        public static ForskolanViewModel ToViewModel(Forskolan forskolan)
        {
            return new ForskolanViewModel
            {
                Id = forskolan.Id,
                Namn = forskolan.Namn,
                Adress = forskolan.Adress,
                Beskrivning = forskolan.Beskrivning,
                TypAvService = forskolan.TypAvService,
                VerksamI = forskolan.VerksamI,
                Organisationsform = forskolan.Organisationsform,
                AntalBarn = forskolan.AntalBarn,
                AntalBarnPerArsarbetare = forskolan.AntalBarnPerArsarbetare,
                AndelLegitimeradeForskollarare = forskolan.AndelLegitimeradeForskollarare,
                Webbplats = forskolan.Webbplats,
                InriktningOchProfil = forskolan.InriktningOchProfil,
                InneOchUtemiljo = forskolan.InneOchUtemiljo,
                KostOchMaltider = forskolan.KostOchMaltider,
                MalOchVision = forskolan.MalOchVision,
                MerOmOss = forskolan.MerOmOss,
                Kontakter = forskolan.Kontakter?.Select(k => ToViewModel(k)).ToList(),
                Latitude = forskolan.Latitude,
                Longitude = forskolan.Longitude
            };
        }

        public static KontaktInfoViewModel ToViewModel(KontaktInfo kontaktInfo)
        {
            return new KontaktInfoViewModel
            {
                Id = kontaktInfo.Id,
                Namn = kontaktInfo.Namn,
                Epost = kontaktInfo.Epost,
                Telefon = kontaktInfo.Telefon,
                Roll = kontaktInfo.Roll
            };
        }
    }
}
