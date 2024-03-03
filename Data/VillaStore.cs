using Tutoriel.DTOs;

namespace Tutoriel.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new VillaDTO{Id=1, Name="Test1"},
                new VillaDTO{Id=2, Name="Test2"}
            };
    }
}
