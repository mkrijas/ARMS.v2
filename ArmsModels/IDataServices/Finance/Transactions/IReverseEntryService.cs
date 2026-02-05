using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices
{
    public interface IReverseEntryService : IbaseInterface<CancellationReasonCodesByDocumentType>
    {

        //CancellationReasonCodesByDocumentType UpdateReverseEntry(CancellationReasonCodesByDocumentType model);
        bool? IsAlreadyReversed(int? DocumentID, int? DocumentTypeID);
        public int TDSReverse(int? ID);
        CancellationReasonCodesByDocumentType GetReverseEntryDetailsByDocumentTypeAndDocumentID(int? DocumentID, int? DocumentTypeID);
        CancellationReasonCodesByDocumentType TDSReverseUpdate(CancellationReasonCodesByDocumentType model);
    }
}