using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
* ******************************************************
* 
* Esta classe abstrata será a base de todos os models, 
* já que a maioria, senão todos, precisarão de Id e um
* validador de concorrência de banco, o RowVersion
* 
* ******************************************************
*/

namespace LendGames.Database.Models
{         
    public abstract class Model
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public static string Key
        {
            get
            {
                // Chave utilizada para criptografar informações importantes que podem ser armazenas em banco de dados
                // Ex.: Senhas, Cartões de Crédito, etc
                return "j0C$VRY9;8H7YxaFZqc-^et!N']G6q5.Y6i>1t~$7-H8,HFtrFgpgZFx@DY$3AQT2:a.+7rPd2(uYNtS!Yl?-1z2eZ4/DM;v;<o!+aSp[4-H7=ZD1v0{A097D&31*G69Gw2u-WAay'nfUBZ;y8t1b-aD(``i}D4,j4eGj4BJHHP2zVB6F[I}5~KX8zUp(";
            }
        }
    }
}
