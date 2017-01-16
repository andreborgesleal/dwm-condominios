using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models.Interfaces
{
    public interface IPathArquivos
    {
        string Path();
        string Extension(string FileID);
    }
}
