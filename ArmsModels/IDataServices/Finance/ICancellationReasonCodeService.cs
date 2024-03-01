using ArmsModels.BaseModels;
using Core.BaseModels.Finance;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface ICancellationReasonCodeService
    {
        CancellationReasonCodesByDocumentType Update(CancellationReasonCodesByDocumentType model);  //Edit
        int Delete(int? AddressID, string UserID);  //Delete
        IEnumerable<CancellationReasonCodesByDocumentType> Select();
        IEnumerable<CancellationReasonCode> SelectSubById(int? DocumentTypeID);
        CancellationReasonCodesByDocumentType UpdateReverseEntry(CancellationReasonCodesByDocumentType model);
        bool? IsAlreadyReversed(int? DocumentID, int? DocumentTypeID);
    }
}
