using WebApiKalum_Backend.Dtos;

namespace WebApiKalum_Backend.Utilities
{
    public class HttpResponsePagination<T> : PaginationDTO<T>
    {
        public HttpResponsePagination(IQueryable<T> source, int number)
        {
            this.Number = number;
            int cantidadRegistrosPorPagina = 5;
            int totalRegistros = source.Count();
            this.TotalPages = (int)Math.Ceiling((Double)totalRegistros / cantidadRegistrosPorPagina);
            this.Content = source.Skip(cantidadRegistrosPorPagina * Number).Take(cantidadRegistrosPorPagina).ToList();
            if (this.Number == 0)
            {
                this.First = true;
            }
            else if ((this.Number + 1) == this.TotalPages)
            {
                this.Last = true;
            }
        }
    }
}