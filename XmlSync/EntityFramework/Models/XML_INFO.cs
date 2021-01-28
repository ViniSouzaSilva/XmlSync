using System;
using System.Collections.Generic;
using System.Text;

namespace XmlSync.EntityFramework.Models
{
    public class XML_INFO : EntityBase
    {
        public string NOME_XML { get; set; }
        public string CONTEUDO  { get; set; }
        public DateTime DATA_CRIAÇÃO { get; set; }
        public DateTime DATA_SINCRONIZACAO { get; set; }
        public bool SINCRONIZADO { get; set; }

    }
}
