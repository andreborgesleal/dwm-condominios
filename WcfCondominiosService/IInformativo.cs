﻿using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfCondominiosService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInformativo" in both code and config file together.
    [ServiceContract]
    public interface IInformativo
    {
        [OperationContract]
        IEnumerable<InformativoViewModel> ListInformativos(string Token, string PageSize);
    }
}
