using Revemar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revemar.Domain.Entities
{
    public class Veiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        [StringLength(50, ErrorMessage = "A marca pode ter no máximo 50 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(50, ErrorMessage = "O modelo pode ter no máximo 50 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Informe um ano válido.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "A cor é obrigatória.")]
        [StringLength(30, ErrorMessage = "A cor pode ter no máximo 30 caracteres.")]
        public string Cor { get; set; } = string.Empty;

        [Range(0.01, 99999999.99, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        public TipoVeiculo Tipo { get; set; }

        public SituacaoEstoque Situacao { get; set; } = SituacaoEstoque.Disponivel;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; } // Nullable, pois no cadastro não há edição ainda
    }
}
