using KinderReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterKinder.Models
{
    public class Forskolan
    {
        public int Id { get; set; }
        public string? Namn { get; set; }
        public string? Adress { get; set; }
        public string? Beskrivning { get; set; }
        public string? TypAvService { get; set; }
        public string? VerksamI { get; set; }
        public string? Organisationsform { get; set; }
        public int AntalBarn { get; set; }
        public double? AntalBarnPerArsarbetare { get; set; }
        public double? AndelLegitimeradeForskollarare { get; set; }
        public string? Webbplats { get; set; }
        public string? InriktningOchProfil { get; set; }
        public string? InneOchUtemiljo { get; set; }
        public string? KostOchMaltider { get; set; }
        public string? MalOchVision { get; set; }
        public string? MerOmOss { get; set; }
        public List<KontaktInfo>? Kontakter { get; set; }
        public List<PdfResult>? PdfResults { get; set; } // Add this property
    }
}
