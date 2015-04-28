using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Adapter
{
    public static class EnumAdapter
    {
        public static string ObterDescricao(this Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static List<ItemListaDTO> ToItemListaDTO(this System.Type enumType)
        {
            List<ItemListaDTO> lista = new List<ItemListaDTO>();

            foreach (System.Enum item in System.Enum.GetValues(enumType))
            {
                var valorEnum = item.GetType().GetField(item.ToString()).GetRawConstantValue();
                lista.Add(new ItemListaDTO() { Id = (int)valorEnum, Descricao = ObterDescricao(item) });
            }

            return lista;
        }
    }
}