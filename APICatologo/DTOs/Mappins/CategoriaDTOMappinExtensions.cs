using APICatologo.Models;

namespace APICatologo.DTOs.Mappins
{
    public static class CategoriaDTOMappinExtensions
    {

        public static CategoriasDTO? ToCategoriaDto(this Categoria categoria) { 
            
            var dto = new CategoriasDTO();
            dto.CategoriaId = categoria.CategoriaId;
            dto.Nome = categoria.Nome;
            dto.ImagemUrl = categoria.ImagemUrl;

            return dto;      
        }

        public static Categoria? ToCategoria(this CategoriasDTO categoriasDTO)
        {
            var cat = new Categoria();

            cat.CategoriaId = categoriasDTO.CategoriaId;
            cat.Nome = categoriasDTO.Nome;
            cat.ImagemUrl = categoriasDTO.ImagemUrl;
            return cat;
        }

        public static IEnumerable<CategoriasDTO>? TocategoriaDtoList(this IEnumerable<Categoria> catList)
        {
            //catList.Select(catList => new CategoriasDTO { CategoriaId = catList.CategoriaId, ImagemUrl = catList.Nome, Nome = catList.Nome});
            var dto = new List<CategoriasDTO>();

            foreach (var cat in catList)
            {
                dto.Add(new CategoriasDTO() { CategoriaId = cat.CategoriaId, ImagemUrl = cat.ImagemUrl, Nome = cat.Nome });
            }


            return dto;
        }
    }
}
